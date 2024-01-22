using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;
using static Monsajem_Client.App;
using WebAssembly.Browser.MonsajemDomHelpers;
using static Monsajem_Incs.UserControler.Publish;


namespace Calculate_wall
{
    partial class Calculator
    {
        public class RawData
        {
            public string Key;
            public string Name;
            public UniverseSummary Summary;
        }

        public static RawData[] KeysNames;

        public static string RawNames;

        public static async Task GetDataFromServer()
        {
            static byte[] UnGzip(byte[] Data)
            {
                using (GZipStream zipStream = new GZipStream(new MemoryStream(Data), CompressionMode.Decompress))
                {
                    var DData = new byte[4096];
                    var From = 0;
                    var Len = 1;
                    while (Len > 0)
                    {
                        Array.Resize(ref DData, From + 4096);
                        Len = zipStream.Read(DData, From, 4096);
                        Array.Resize(ref DData, From + Len);
                        From += Len;
                    }
                    return DData;
                }
            }

            static string RawToCVS(string Data)
            {
                var str = new StringWriter();
                str.WriteLine("<TICKER>,<DTYYYYMMDD>,<FIRST>,<HIGH>,<LOW>,<CLOSE>,<VALUE>,<VOL>,<OPENINT>,<PER>,<OPEN>,<LAST>");
                var Lines = Data.Split(';');
                for (int i = Lines.Length - 1; i > -1; i--)
                {
                    var WrapData = Lines[i].Split(',');
                    //Date,HIGH,LOW,FIRST,LAST,VOL,CLOSE
                    var Date = WrapData[0];
                    var HIGH = WrapData[1];
                    var LOW = WrapData[2];
                    var FIRST = WrapData[3];
                    var LAST = WrapData[4];
                    var VOL = WrapData[5];
                    var CLOSE = WrapData[6];
                    var VALUE = UInt64.Parse(CLOSE) * UInt64.Parse(VOL);
                    str.WriteLine($"Converted,{Date},{FIRST},{HIGH},{LOW},{CLOSE},{VALUE},{VOL},{FIRST},D,{FIRST},{LAST}");
                }
                return str.ToString();
            }

            if (KeysNames == null)
            {
                RawNames = await js.LoadStringFromBaseURL("/php/AdminActions/WorkStation/Names.txt?"+DateTime.UtcNow.ToString());

                var KeysNames = RawNames.Split(new char[] { "\n"[0], "\r"[0] }, StringSplitOptions.RemoveEmptyEntries);

                Calculator.KeysNames = new RawData[KeysNames.Length / 2];
                for (int i = 0; i < KeysNames.Length; i += 2)
                {
                    Calculator.KeysNames[i / 2] = new RawData()
                    { Key = KeysNames[i], Name = KeysNames[i + 1] };
                }
            }

            ShowAction("Loading...");

            var Tasks = new Func<Task>[KeysNames.Length];
            var Done = 0;
            for (int i=0;i< KeysNames.Length;i++)
            {
                var Pos = i;
                var Key = KeysNames[Pos];
                Tasks[Pos] = async () =>
                {
                    while(true)
                    {
                        try
                        {
                            if (Key.Summary == null)
                            {
                                bool IsCVS = true;
                                var Address = $"http://www.tsetmc.com/tsev2/data/Export-txt.aspx?t=i&a=1&b=0&i={Key.Key}";

                                IsCVS = false;
                                Address = $"http://members.tsetmc.com/tsev2/chart/data/Financial.aspx?i={Key.Key}&t=ph&a=1";

                                var Data = new byte[0];

                                while (Data.Length == 0)
                                {
                                    try
                                    {
                                        Console.WriteLine("Downloading " + Address);
                                        Console.WriteLine("Download Data " +
                                           await Monsajem_Incs.TimeingTester.Timing.run(async () =>
                                           {
                                               Data = await DownloadData(Address);
                                           }));

                                    }
                                    catch
                                    { }

                                    if (Data.Length == 0)
                                    {
                                        await Task.Delay(1000);
                                        Console.WriteLine("Download Retrying...");
                                    }

                                }

                                Console.WriteLine("UnGzip Data " +
                                   await Monsajem_Incs.TimeingTester.Timing.run(async () =>
                                   {
                                       Data = UnGzip(Data);
                                   }));

                                var StringData = "";

                                Console.WriteLine("Convert to CVS " +
                                    Monsajem_Incs.TimeingTester.Timing.run(() =>
                                    {
                                        StringData = RawToCVS(System.Text.Encoding.UTF8.GetString(Data));
                                    }));

                                var Points = new Point[0];

                                Console.WriteLine("Make Points " +
                                    Monsajem_Incs.TimeingTester.Timing.run(() =>
                                    {
                                        Points = GetPoints(StringData);
                                    }));

                                Console.WriteLine("Make UniverseSummary " +
                                    Monsajem_Incs.TimeingTester.Timing.run(() =>
                                    {
                                        Key.Summary = new UniverseSummary(Points);
                                    }));
                            }

                            Key.Summary.Rate = new Rate();

                            var Status = (++Done).ToString() + "/" + KeysNames.Length.ToString();

                            ShowAction(Status);

                            Console.WriteLine(Status);
                            break;
                        }
                        catch
                        {
                            Console.WriteLine("Faild, Retrying...");
                            await Task.Delay(1000);
                        }
                    }
                };
            }

            await Monsajem_Incs.Async.Task_EX.StartWait(3,Tasks);

            await Calculate();
            Console.WriteLine("Done!");
            ShowSuccessMessage("Done!");
            HideAction();
            Monsajem_Client.OnLoadApp.SaveDataOnServer();
        }
    }
}