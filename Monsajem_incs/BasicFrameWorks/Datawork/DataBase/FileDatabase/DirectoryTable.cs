using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
namespace Monsajem_Incs.Database.DirectoryTable
{
    public class DirectoryTable<ValueType,KeyType>:
        Table<ValueType,KeyType>
        where KeyType:IComparable<KeyType>
    {
        public DirectoryTable(
            string DirectoryAddress,
            Func<ValueType, KeyType> GetKey,
            bool IsUpdateAble,
            bool FastSave):
            base(new StreamCollection<ValueType>(
                        File.Open(DirectoryAddress + "\\Data", FileMode.OpenOrCreate)),GetKey,false)
        {
            this.TableName = new DirectoryInfo(DirectoryAddress).Name;
            Directory.CreateDirectory(DirectoryAddress);
            if (File.Exists(DirectoryAddress + "\\PK"))
            {
                var OldTable = File.ReadAllBytes(DirectoryAddress + "\\PK").Deserialize(this);
                this.KeysInfo.Keys = OldTable.KeysInfo.Keys;
                ////StreamCollection.StreamLen = this.StreamCollection.Stream.Length;
                if (IsUpdateAble)
                {
                    ReadyForUpdateAble();
                }
                this.UpdateAble = OldTable.UpdateAble;
            }
            else
            {
                if (IsUpdateAble)
                {
                    ReadyForUpdateAble();
                    this.UpdateAble = new UpdateAbles<KeyType>();
                }
                
            }

            var Stream_PK = File.Open(DirectoryAddress + "\\PK", FileMode.OpenOrCreate);
            this.Serialize();

            Action Save = () =>
            {
                    var sr = this.Serialize();
                    Stream_PK.Seek(0, SeekOrigin.Begin);
                    Stream_PK.SetLength(sr.Length);
                    Stream_PK.Write(sr, 0, sr.Length);
                    Stream_PK.FlushAsync();
            };
        }

        public static void DeleteTable(string DirectoryAddress)
        {
            File.Delete(DirectoryAddress + "/Data");
            File.Delete(DirectoryAddress + "/PK");
        }
    }

}
