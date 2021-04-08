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
        [Serialization.NonSerialized]
        private volatile bool NeedToSave=true;

        private StreamCollection<ValueType> StreamCollection
        {
            get => _StreamCollection;
            set
            {
                this.BasicActions.Items = value;
                _StreamCollection = value;
            }
        }
        
        private StreamCollection<ValueType> _StreamCollection;

        public DirectoryTable(
            string DirectoryAddress,
            Func<ValueType, KeyType> GetKey,
            bool IsUpdateAble,
            bool FastSave):
            base(new StreamCollection<ValueType>(),GetKey,false)
        {
            this.TableName = new DirectoryInfo(DirectoryAddress).Name;
            _StreamCollection =(StreamCollection<ValueType>) this.BasicActions.Items;
            Directory.CreateDirectory(DirectoryAddress);
            if (File.Exists(DirectoryAddress + "\\PK"))
            {
                var OldTable = File.ReadAllBytes(DirectoryAddress + "\\PK").Deserialize(this);
                this.KeysInfo.Keys = OldTable.KeysInfo.Keys;
                StreamCollection = OldTable.StreamCollection;
                StreamCollection.Collection.Stream = File.Open(DirectoryAddress + "\\Data", FileMode.OpenOrCreate);
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
                StreamCollection.Collection.Stream = File.Open(DirectoryAddress + "\\Data", FileMode.OpenOrCreate);
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

            if(FastSave==true)
            {
                var Save_trd = new System.Threading.Thread(() =>
                {
                    save:
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch
                    {
                        goto save;
                    }
                    lock (this)
                    {
                        if (this.NeedToSave == true)
                        {
                            Save();
                            NeedToSave = false;
                        }
                    }
                    goto save;
                });
                Save_trd.Start();

                this.Events.Inserted += (info) => {

                    lock (this)
                    {
                        if (this.NeedToSave == false)
                            this.NeedToSave = true;
                    }
                };
                this.Events.Deleted += (info) =>
                {
                    lock (this)
                    {
                        if (this.NeedToSave == false)
                            this.NeedToSave = true;
                    }
                };
                this.Events.Updated += (info) => {
                    lock (this)
                    {
                        if (this.NeedToSave == false)
                            this.NeedToSave = true;
                    }
                };
            }
            else
            {
               Runer.Run.OnEndBlocks+=()=>Save();
            }
        }

        public static void DeleteTable(string DirectoryAddress)
        {
            File.Delete(DirectoryAddress + "/Data");
            File.Delete(DirectoryAddress + "/PK");
        }
    }

}
