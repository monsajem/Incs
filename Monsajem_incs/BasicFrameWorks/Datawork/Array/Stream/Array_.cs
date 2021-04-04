//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using Monsajem_Incs.Serialization;
//using System.Runtime.InteropServices;
//namespace Monsajem_Incs.Collection.Array.StreamBased
//{
//    public class Array<ArrayType> :
//        Base.IArray<ArrayType, Array<ArrayType>>
//    {
//        private Stream DataStream;
//        private Stream AddressStream;
//        private static int ItemSize;

//        public override ArrayType this[int Pos]
//        {
//            get
//            {
//                if (ItemSize > 0)
//                {
//                    var IS = ItemSize;
//                    DataStream.Seek(Pos * IS, SeekOrigin.Begin);
//                    var Bytes = new byte[IS];
//                    while(IS!=0)
//                        IS-=DataStream.Read(Bytes,ItemSize-IS, IS);

//                    var ptr = Marshal.AllocHGlobal(ItemSize);
//                    Marshal.Copy(Bytes, 0,ptr, ItemSize);
//                    var Result = Marshal.PtrToStructure<ArrayType>(ptr);
//                    Marshal.FreeHGlobal(ptr);
//                    return Result;
//                }
//                else
//                {
//                    var IS = 8;
//                    AddressStream.Seek(Pos * IS, SeekOrigin.Begin);
//                    var Bytes = new byte[IS];
//                    while (IS != 0)
//                        IS -= DataStream.Read(Bytes, 8 - IS, IS);
//                    var Address = BitConverter.ToInt32(Bytes);
//                    IS = BitConverter.ToInt32(Bytes,4);
//                    var ItemSize = IS;

//                    DataStream.Seek(Address, SeekOrigin.Begin);
//                    Bytes = new byte[IS];
//                    while (IS != 0)
//                        IS -= DataStream.Read(Bytes, ItemSize - IS, IS);
//                    return Bytes.Deserialize<ArrayType>();
//                }
//            }
//            set
//            {
//                if (ItemSize > 0)
//                {
//                    var IS = ItemSize;
//                    DataStream.Seek(Pos * IS, SeekOrigin.Begin);
//                    var Bytes = new byte[IS];
//                    DataStream.Write(Bytes, 0, IS);
//                    var ptr = Marshal.AllocHGlobal(ItemSize);
//                    Marshal.Copy(Bytes, 0, ptr, ItemSize);
//                    var Result = Marshal.PtrToStructure<ArrayType>(ptr);
//                    Marshal.FreeHGlobal(ptr);
//                    return Result;
//                }
//                else
//                {
//                    var IS = 8;
//                    AddressStream.Seek(Pos * IS, SeekOrigin.Begin);
//                    var Bytes = new byte[IS];
//                    while (IS != 0)
//                        IS -= DataStream.Read(Bytes, 8 - IS, IS);
//                    var Address = BitConverter.ToInt32(Bytes);
//                    IS = BitConverter.ToInt32(Bytes, 4);
//                    var ItemSize = IS;

//                    DataStream.Seek(Address, SeekOrigin.Begin);
//                    Bytes = new byte[IS];
//                    while (IS != 0)
//                        IS -= DataStream.Read(Bytes, ItemSize - IS, IS);
//                    return Bytes.Deserialize<ArrayType>();
//                }
//            }
//        }

//        public override object MyOptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public override void DeleteByPosition(int Position)
//        {
//            throw new NotImplementedException();
//        }

//        public override void Insert(ArrayType Value, int Position)
//        {
//            throw new NotImplementedException();
//        }

//        protected override Array<ArrayType> MakeSameNew()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}