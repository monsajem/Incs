using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public interface IWhenCanSerialize
    {
        bool CanSerialize { get; }
    }

    public interface IPreSerialize
    {
        void PreSerialize();
    }

    public interface IAfterDeserialize
    {
        void AfterDeserialize();
    }

    public interface ISerializable<DataType>
    {
        DataType GetData();
        void SetData(DataType Data);
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class NonSerializedAttribute : Attribute
    { }
}