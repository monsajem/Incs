using System;

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

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class NonSerializedAttribute : Attribute
    { }
}