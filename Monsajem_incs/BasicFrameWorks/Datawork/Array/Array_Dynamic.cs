using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.Base
{
    public class DynamicArray<ArrayType> :
       IArray<ArrayType>
    {
        public override object MyOptions { get => null; set { } }

        public Func<int, ArrayType> _GetItem;
        public Action<int, ArrayType> _SetItem;
        public Action<int> _DeleteByPosition;
        public Action<ArrayType,int> _insert;
        public DynamicArray()
        {

        }

        public new int Length { get =>base.Length;set => base.Length = value; }

        public override ArrayType this[int Pos] { get => _GetItem(Pos); set => _SetItem(Pos, value); }

        public override void DeleteByPosition(int Position)
        {
            _DeleteByPosition(Position);
        }

        public override void Insert(ArrayType Value, int Position)
        {
            _insert(Value,Position);
        }
    }
}