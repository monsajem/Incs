using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Array.Base
{
    public class DynamicArray<ArrayType> :
       IArray<ArrayType>
    {
        public delegate System.Array DG_GetArrayFrom(int From, out int Ar_From, out int Ar_Len);
        public delegate System.Array DG_GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len);
        public delegate void DG_SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len);
        public override object MyOptions { get => null; set { } }

        public Func<int, ArrayType> _GetItem;
        public Action<int, ArrayType> _SetItem;
        public Action<int> _DeleteFrom;
        public Action<int> _AddLength;
        public Action<int> _DeleteByPosition;
        public DG_GetArrayFrom _GetArrayFrom;
        public DG_GetArrayPos _GetArrayPos;
        public DG_SetFromTo _SetFromTo;
        public DynamicArray()
        {

        }

        public override ArrayType this[int Pos] { get => _GetItem(Pos); set => _SetItem(Pos, value); }

        public override void DeleteFrom(int from)
        {
            _DeleteFrom(from);
        }

        internal override void AddLength(int Count)
        {
            _AddLength(Count);
        }

        public override void DeleteByPosition(int Position)
        {
            if (_DeleteByPosition != null)
                _DeleteByPosition(Position);
            else
                base.DeleteByPosition(Position);
        }

        internal override System.Array GetArrayFrom(int From, out int Ar_From, out int Ar_Len)
        {
            if (_GetArrayFrom != null)
                return _GetArrayFrom(From, out Ar_From, out Ar_Len);
            else
                return base.GetArrayFrom(From, out Ar_From, out Ar_Len);
        }

        internal override System.Array GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len)
        {
            if (_GetArrayPos != null)
                return _GetArrayPos(Ar_Pos, out Ar_From, out Ar_Len);
            else
                return base.GetArrayPos(Ar_Pos, out Ar_From, out Ar_Len);
        }

        internal override void SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len)
        {
            if (_SetFromTo != null)
                _SetFromTo(From,Ar, Ar_From, Ar_Len);
            else
                base.SetFromTo(From, Ar, Ar_From, Ar_Len);
        }
    }
}