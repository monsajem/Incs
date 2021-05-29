using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Monsajem_Incs.Data.StreamExtentions;

namespace Monsajem_Incs.Net.Web.WebSocket.Server
{
    public enum WebSocketOpCode : int
    {
        ContinuationFrame = 0x0,
        Text = 0x1,
        Binary = 0x2,
        ConnectionClose = 0x8,
        Ping = 0x9,
        Pong = 0xA
    }

    public class WebSocketSession : IDisposable
    {
        private static readonly Random Random = new Random();

        private TcpClient Client { get; }
        private Stream ClientStream { get; }

        public string Id { get; }

        public event Action<byte[], WebSocketOpCode> MessageReceived;

        public WebSocketSession(TcpClient client)
        {
            Client = client;
            ClientStream = client.GetStream();
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Internal, do not use :)
        /// </summary>
        internal void Start()
        {
            if (!DoHandshake())
                throw new Exception("Handshake Failed.");
        }

        public void StartMessageLoop()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                MessageLoop();
            });
        }

        private bool DoHandshake()
        {
            while (Client.Available == 0 && Client.Connected) { }
            if (!Client.Connected) return false;

            byte[] handshake;
            using (var handshakeBuffer = new MemoryStream())
            {
                while (Client.Available > 0)
                {
                    var buffer = new byte[Client.Available];
                    ClientStream.Read(buffer, 0, buffer.Length);
                    handshakeBuffer.Write(buffer, 0, buffer.Length);
                }

                handshake = handshakeBuffer.ToArray();
            }

            if (!Encoding.UTF8.GetString(handshake).StartsWith("GET")) return false;

            var response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                                                  + "Connection: Upgrade" + Environment.NewLine
                                                  + "Upgrade: websocket" + Environment.NewLine
                                                  + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                                                      SHA1.Create().ComputeHash(
                                                          Encoding.UTF8.GetBytes(
                                                              new Regex("Sec-WebSocket-Key: (.*)").Match(Encoding.UTF8.GetString(handshake)).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                                          )
                                                      )
                                                  ) + Environment.NewLine
                                                  + Environment.NewLine);

            ClientStream.Write(response, 0, response.Length);
            return true;
        }
        private void MessageLoop()
        {
            var session = this;
            var client = session.Client;
            var stream = session.ClientStream;

            var packet = new List<byte>();

            WebSocketOpCode messageOpcode = 0x0;
            using (var messageBuffer = new MemoryStream())
                while (client.Connected)
                {
                    packet.Clear();

                    var ab = client.Available;
                    if (ab == 0) continue;

                    packet.Add((byte)stream.ReadByte());
                    var fin = (packet[0] & (1 << 7)) != 0;
                    var rsv1 = (packet[0] & (1 << 6)) != 0;
                    var rsv2 = (packet[0] & (1 << 5)) != 0;
                    var rsv3 = (packet[0] & (1 << 4)) != 0;

                    var opcode = (WebSocketOpCode)(packet[0] & ((1 << 4) - 1));

                    switch (opcode)
                    {
                        case WebSocketOpCode.ContinuationFrame:
                            break;
                        case WebSocketOpCode.Text:
                        case WebSocketOpCode.Binary:
                        case WebSocketOpCode.ConnectionClose:
                            messageOpcode = opcode;
                            break;
                        case WebSocketOpCode.Ping:
                            continue;
                        case WebSocketOpCode.Pong:
                            continue;
                        default:
                            continue; // Reserved
                    }

                    packet.Add((byte)stream.ReadByte());
                    var masked = (packet[1] & (1 << 7)) != 0;
                    var pseudoLength = packet[1] - (masked ? 128 : 0);

                    ulong actualLength = 0;
                    if (pseudoLength > 0 && pseudoLength < 125) actualLength = (ulong)pseudoLength;
                    else if (pseudoLength == 126)
                    {
                        var length = new byte[2];
                        stream.Fill(length);
                        packet.AddRange(length);
                        Array.Reverse(length);
                        actualLength = BitConverter.ToUInt16(length, 0);
                    }
                    else if (pseudoLength == 127)
                    {
                        var length = new byte[8];
                        stream.Fill(length);
                        packet.AddRange(length);
                        Array.Reverse(length);
                        actualLength = BitConverter.ToUInt64(length, 0);
                    }

                    var mask = new byte[4];
                    if (masked)
                    {
                        stream.Fill(mask);
                        packet.AddRange(mask);
                    }

                    if (actualLength > 0)
                    {
                        var data = new byte[actualLength];
                        stream.Fill(data);
                        packet.AddRange(data);

                        if (masked)
                            ApplyMask(data, mask);

                        messageBuffer.Write(data, 0, data.Length);
                    }


                    if (!fin) continue;
                    var message = messageBuffer.ToArray();

                    switch (messageOpcode)
                    {
                        case WebSocketOpCode.Text:
                            MessageReceived?.Invoke(message, WebSocketOpCode.Text);
                            break;
                        case WebSocketOpCode.Binary:
                            MessageReceived?.Invoke(message, WebSocketOpCode.Binary);
                            break;
                        case WebSocketOpCode.ConnectionClose:
                            Close();
                            break;
                        default:
                            throw new Exception("Invalid opcode: " + messageOpcode);
                    }

                    messageBuffer.SetLength(0);
                }
        }

        public void Close(bool Mask=false)
        {
            if (!Client.Connected) return;

            var mask = new byte[4];
            if (Mask) Random.NextBytes(mask);
            Send(new byte[] { }, WebSocketOpCode.ConnectionClose, Mask, mask);

            Client.Close();
        }

        public void Send(byte[] payload, bool isBinary) => Send(Client, payload, isBinary, false);

        public void Send(byte[] payload, bool isBinary = true, bool masking = false) => Send(Client, payload, isBinary, masking);
        public void Send(byte[] payload, WebSocketOpCode opcode, bool masking, byte[] mask) => Send(Client, payload, opcode, masking, mask);

        static void Send(TcpClient client, byte[] payload, bool isBinary = false, bool masking = false)
        {
            var mask = new byte[4];
            if (masking) Random.NextBytes(mask);
            Send(client, payload, isBinary ? WebSocketOpCode.Binary : WebSocketOpCode.Text , masking, mask);
        }
        static void Send(TcpClient client, byte[] payload, WebSocketOpCode opcode, bool masking, byte[] mask)
        {
            if (masking && mask == null) throw new ArgumentException(nameof(mask));

            using (var packet = new MemoryStream())
            {
                byte firstbyte = 0b0_0_0_0_0000;

                firstbyte |= 0b1_0_0_0_0000;

                firstbyte += (byte)opcode;
                packet.WriteByte(firstbyte);

                byte secondbyte = 0b0_0000000;

                if (masking)
                    secondbyte |= 0b1_0000000;

                var payload_Length = payload.LongLength;

                if (payload_Length <= 0b0_1111101) // 125
                {
                    secondbyte |= (byte)payload_Length;
                    packet.WriteByte(secondbyte);
                }
                else if (payload_Length <= UInt16.MaxValue) // If length takes 2 bytes
                {
                    secondbyte |= 0b0_1111110; // 126
                    packet.WriteByte(secondbyte);

                    var len = BitConverter.GetBytes(payload_Length);
                    Array.Reverse(len, 0, 2);
                    packet.Write(len, 0, 2);
                }
                else // if (payload.LongLength <= Int64.MaxValue) // If length takes 8 bytes
                {
                    secondbyte |= 0b0_1111111; // 127
                    packet.WriteByte(secondbyte);

                    var len = BitConverter.GetBytes(payload_Length);
                    Array.Reverse(len, 0, 8);
                    packet.Write(len, 0, 8);
                }

                if (masking)
                {
                    packet.Write(mask, 0, 4);
                    ApplyMask(payload, mask);
                }


                var stream = client.GetStream();

                var Head = packet.ToArray();
                stream.Write(Head, 0, Head.Length);
                stream.Write(payload, 0, payload.Length);
                stream.Flush();
            }
        }

        static void ApplyMask(byte[] msg, byte[] mask)
        {
            var Count = msg.Length;
            for (var i = 0; i < Count; i++)
                msg[i] = (byte)(msg[i] ^ mask[i % 4]);
        }

        public void Dispose()
        {
            Close();

            (Client as IDisposable)?.Dispose();
            ClientStream?.Dispose();
        }
    }
}