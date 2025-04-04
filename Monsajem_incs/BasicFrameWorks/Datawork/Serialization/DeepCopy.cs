﻿using Monsajem_Incs.Collection.Array;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Serialization
{
    public class CopyOrginalObject : Attribute
    { }
    internal class ObjectContainer :
        IEquatable<ObjectContainer>
    {
        public object obj;
        public bool IsUniqueHashCode;
        public int HashCode;
        public int FromPos;
        public (byte[] Bytes, object Obj) Data;

        public Action<object> Set;
        public Func<object> Get;

        public bool Equals(ObjectContainer other)
        {
            return IsUniqueHashCode ? HashCode == other.HashCode : object.Equals(obj, other.obj);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
    public static class ObjectExtensions
    {
        [ThreadStatic]
        private static bool OrginalTargetForDelegates;
        [ThreadStatic]
        private static HashSet<ObjectContainer> visited;
        [ThreadStatic]
        private static Action AtLast;
        public static bool IsPrimitive(this Type type)
        {
            return type == typeof(String)
                ? true
                : (type.IsValueType & type.IsPrimitive) |
                type.IsEnum;
        }

        public static Object Copy(this Object originalObject, bool OrginalTargetForDelegates = false)
        {
            ObjectExtensions.OrginalTargetForDelegates = OrginalTargetForDelegates;
            var Pos = InternalCopy(originalObject.GetType());
            visited = [];
            var Result = InternalCopys[Pos]
                (originalObject);
            AtLast?.Invoke();
            AtLast = null;
            return Result;
        }

        public static T Copy<T>(this T original, bool OrginalTargetForDelegates = false)
        {
            return original == null
                ? default
                : (T)Copy(
                originalObject: original,
                OrginalTargetForDelegates: OrginalTargetForDelegates);
        }

        private static void Clone(
            object originalObject,
            Action<object> SetValue,
            Func<object, object> Copy)
        {
            if (originalObject == null)
                return;
            var Key = new ObjectContainer()
            {
                HashCode = originalObject.GetHashCode(),
                obj = originalObject
            };
            if (visited.TryGetValue(Key, out var VisitedObj))
            {
                AtLast += () => SetValue(VisitedObj.obj);
            }
            else
            {
                _ = visited.Add(Key);
                var Obj = Copy(originalObject);
                SetValue(Obj);
            }
        }

        private static int InternalCopy(Type typeToReflect)
        {
            var pos = System.Array.BinarySearch(TypeCodes, typeToReflect.GetHashCode());
            if (pos > -1)
            {
                return CopyPoss[pos];
            }
            else
            {
                pos = InternalCopys.Length;
                Insert(ref InternalCopys, (Func<object, object>)null);
                Insert(ref CopyPoss, pos, BinaryInsert(ref TypeCodes, typeToReflect.GetHashCode()));

                Func<object, object> MyInternalCopy;

                if (IsPrimitive(typeToReflect))
                    MyInternalCopy = (originalObject) => originalObject;
                else
                {
                    if (typeToReflect.IsArray | typeToReflect == typeof(System.Array))
                    {
                        Func<object, (Type ElementType, int Rank,
                            Func<object, object> InternalCopy)> GetInfo = null;
                        object MyInternalCopy_ArrDelegate(object originalObject) => null;
                        Func<object, object> MyInternalCopy_Arr;
                        {
                            MyInternalCopy_Arr = (originalObject) =>
                            {
                                var Info = GetInfo(originalObject);

                                var ArrayObject = (System.Array)originalObject;
                                var lents = new int[Info.Rank];
                                for (int i = 0; i < Info.Rank; i++)
                                {
                                    lents[i] = ArrayObject.GetUpperBound(i) + 1;
                                }

                                var cloneObject = System.Array.CreateInstance(Info.ElementType, lents);

                                cloneObject.ForEach(lents, (indices) =>
                                {
                                    var StandAloneCurrent = new int[Info.Rank];
                                    for (int i = 0; i < Info.Rank; i++)
                                        StandAloneCurrent[i] = indices[i];
                                    Clone(ArrayObject.GetValue(StandAloneCurrent),
                                        (obj) => cloneObject.SetValue(obj, StandAloneCurrent),
                                        Info.InternalCopy);
                                });
                                _ = visited.Add(new ObjectContainer()
                                {
                                    obj = cloneObject,
                                    HashCode = originalObject.GetHashCode()
                                });
                                return cloneObject;
                            };
                        }

                        if (typeToReflect == typeof(System.Array))
                        {
                            MyInternalCopy = (originalObject) =>
                            {
                                var Pos = InternalCopy(originalObject.GetType());
                                return InternalCopys[Pos](originalObject);
                            };
                        }
                        else
                        {
                            var ElementType = typeToReflect.GetElementType();
                            var Rank = typeToReflect.GetArrayRank();
                            var ICPos = InternalCopy(typeToReflect.GetElementType());
                            var ArrayInternalCopy = InternalCopys[ICPos];
                            GetInfo = (ar) => (ElementType, Rank, ArrayInternalCopy);
                            MyInternalCopy = IsPrimitive(typeToReflect.GetElementType())
                                ? ((ar) =>
                                {
                                    var ArrayObject = (System.Array)ar;
                                    var lents = new int[Rank];
                                    for (int i = 0; i < Rank; i++)
                                        lents[i] = ArrayObject.GetUpperBound(i) + 1;
                                    var cloneObject = System.Array.CreateInstance(ElementType, lents);
                                    ArrayObject.CopyTo(cloneObject, 0);
                                    return cloneObject;
                                })
                                : typeof(Delegate).IsAssignableFrom(typeToReflect) ? MyInternalCopy_ArrDelegate : MyInternalCopy_Arr;
                        }

                    }
                    else
                    {
                        if (typeof(Delegate).IsAssignableFrom(typeToReflect))
                        {
                            MyInternalCopy = (originalObject) =>
                            {
                                var OrginalDelegates = ((Delegate)originalObject).GetInvocationList();
                                var Results = new Delegate[OrginalDelegates.Length];
                                for (int i = 0; i < OrginalDelegates.Length; i++)
                                {
                                    Results[i] = (Delegate)OrginalDelegates[i].Clone();
                                }
                                var cloneObject = Delegate.Combine(Results);
                                for (int i = 0; i < OrginalDelegates.Length; i++)
                                {
                                    if (OrginalDelegates[i].Target != null)
                                    {
                                        if (OrginalTargetForDelegates)
                                        {
                                            Serialization.Deletage_Target.SetValue(Results[i], OrginalDelegates[i].Target);
                                        }
                                        else
                                        {
                                            var Pos = InternalCopy(OrginalDelegates[i].Target.GetType());
                                            var ClonedTarget = InternalCopys[Pos](OrginalDelegates[i].Target);
                                            Serialization.Deletage_Target.SetValue(Results[i], ClonedTarget);
                                        }
                                    }
                                }

                                return cloneObject;
                            };
                        }
                        else
                        {
                            var Fields = new DynamicAssembly.TypeFields(typeToReflect).Fields;
                            var FieldIInfo = new
                                (Func<(object cloneObject, object FieldValue), object> Copy,
                                 DynamicAssembly.FieldControler Field)[Fields.Length];
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                var Field = Fields[i];
                                if (Field.Info.GetCustomAttributes(typeof(CopyOrginalObject)).Count() > 0)
                                    FieldIInfo[i] = ((c) =>
                                    {
                                        var Result = c.FieldValue;
                                        Field.SetValue(c.cloneObject, Result);
                                        return Result;
                                    }, Field);
                                else if (!IsPrimitive(Field.Info.FieldType))
                                    FieldIInfo[i] = ((c) =>
                                    {
                                        var FieldInternalCopy = InternalCopy(c.FieldValue.GetType());
                                        var clonedFieldValue = InternalCopys[FieldInternalCopy](c.FieldValue);
                                        Field.SetValue(c.cloneObject, clonedFieldValue);
                                        return clonedFieldValue;
                                    }, Field);
                                else
                                {
                                    var Pos = InternalCopy(Field.Info.FieldType);
                                    var Copy = InternalCopys[Pos];
                                    FieldIInfo[i] = ((c) =>
                                    {
                                        var clonedFieldValue = Copy(c.FieldValue);
                                        Field.SetValue(c.cloneObject, clonedFieldValue);
                                        return clonedFieldValue;
                                    }, Field);
                                }
                            }

                            var FieldLen = Fields.Length;

                            MyInternalCopy = (originalObject) =>
                            {
                                var cloneObject = GetUninitializedObject(originalObject.GetType());
                                _ = visited.Add(new ObjectContainer
                                {
                                    HashCode = originalObject.GetHashCode(),
                                    obj = cloneObject
                                });
                                for (int i = 0; i < FieldLen; i++)
                                {
                                    var Info = FieldIInfo[i];
                                    Clone(Info.Field.GetValue(originalObject),
                                        (c) => Info.Field.SetValue(cloneObject, c),
                                        (c) => Info.Copy((cloneObject, c)));
                                }
                                return cloneObject;
                            };
                        }
                    }
                }
                InternalCopys[pos] = MyInternalCopy;
                return pos;
            }
        }

        private static Func<object, object>[] InternalCopys = new Func<object, object>[0];
        private static int[] TypeCodes = new int[0];
        private static int[] CopyPoss = new int[0];
    }
}