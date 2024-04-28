using Microsoft.JSInterop;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{
    public enum ConvertEnum
    {
        Default,
        ToLower,
        ToUpper,
        Numeric
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field,
        AllowMultiple = true, Inherited = false)]
    public class ExportAttribute : Attribute
    {
        public ExportAttribute() : this(null, null)
        {
        }

        public ExportAttribute(Type contractType) : this(null, contractType)
        {
        }

        public ExportAttribute(string contractName) : this(contractName, null)
        {
        }

        public ExportAttribute(string contractName, Type contractType)
        {
            ContractName = contractName;
            ContractType = contractType;
        }

        public string ContractName { get; }

        public Type ContractType { get; }
        public ConvertEnum EnumValue { get; set; }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExportClassAttribute : ExportAttribute, IComparable<ExportClassAttribute>
    {
        private static Array<ExportClassAttribute> Info = new(20);
        private ExportClassAttribute(Type contractType)
        {
            ContractHash = contractType.GetHashCode();
        }
        public ExportClassAttribute(string contractName, Type contractType) : base(contractName, contractType)
        {
            ContractHash = contractType.GetHashCode();
            if (Info.BinarySearch(this).Value == null)
                _ = Info.BinaryInsert(this);
            jSObjectStatic = js.JsGetStaticValue(ContractName);
        }
        private int ContractHash;
        public readonly IJSInProcessObjectReference jSObjectStatic;
        public static ExportClassAttribute GetExportOf<t>()
        {
            var Pos = Info.BinarySearch(new ExportClassAttribute(typeof(t)));
            if (Pos.Value == null)
            {
                _ = typeof(t).GetCustomAttributes(false);
                Pos = Info.BinarySearch(new ExportClassAttribute(typeof(t)));
                if (Pos.Value == null)
                    throw new EntryPointNotFoundException(
                        $"Export of type '{typeof(t)}' not found!");
            }
            return Pos.Value;
        }

        public int CompareTo(ExportClassAttribute other)
        {
            return ContractHash - other.ContractHash;
        }
    }
}
