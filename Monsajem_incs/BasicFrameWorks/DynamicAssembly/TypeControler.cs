using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Runtime.CompilerServices;
using static Monsajem_Incs.Collection.Array.Extentions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Monsajem_Incs.DynamicAssembly
{
    public static class TypeController<T>
    {
        public static FieldInfo[] GetAllFields(
            BindingFlags? bindingFlags = null,
            Func<FieldInfo, bool> filter = null) =>
                TypeController.GetAllFields(
                    typeof(T), bindingFlags, filter);

        public static MethodInfo[] GetAllMethods(
            BindingFlags? bindingFlags = null,
            Func<MethodInfo, bool> filter = null) =>
                TypeController.GetAllMethods(
                    typeof(T), bindingFlags, filter);

        public static MemberInfo[] GetAllMembers(
            BindingFlags? bindingFlags = null,
            Func<MemberInfo, bool> filter = null) =>
                TypeController.GetAllMembers(
                    typeof(T), bindingFlags, filter);

        public static T CreateInstance() => _CreateInstance();
        private static Func<T> _CreateInstance = ((Func<Func<T>>)(() =>
        {
            var t = typeof(T);
            var Rt = Type.GetType("System.RuntimeType");
            var nativeGetUninitializedObject = typeof(System.Runtime.Serialization.FormatterServices).
                GetMethod("nativeGetUninitializedObject", BindingFlags.Static | BindingFlags.NonPublic);
            return DynamicMethodBuilder.Delegate<Func<T>>("CreateInstance_",
            (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, nativeGetUninitializedObject);
                il.Emit(OpCodes.Castclass, t);
                il.Emit(OpCodes.Ret);
            }, Rt, t);
        }))();


        private static int _ArRank = ((Func<int>)(() =>
        {
            try { return typeof(T).GetArrayRank(); }
            catch { return 0; }
        }))();

        public static T CreateArray(params int[] Len) =>
            _CreateArray(Len);
        private static Func<int[], T> _CreateArray =
        ((Func<Func<int[], T>>)(() =>
        {
            var Type = typeof(T);
            if (Type.IsArray == false)
                return (a) => throw new Exception($"Type of {Type} is not array type.");
            var Rank = Type.GetArrayRank();
            Type = Type.GetElementType();
            Type = System.Array.CreateInstance(Type, new int[Rank]).GetType();

            return DynamicMethodBuilder.Delegate<Func<int[], T>>("CreateInstance_",
            (il) =>
            {
                for (int i = 0; i < Rank; i++)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4_S, i);
                    il.Emit(OpCodes.Ldelem_I4);
                }

                il.Emit(OpCodes.Newobj, Type.GetConstructors()[0]);
                il.Emit(OpCodes.Ret);
            });
        }))();

        private static int[] _ArLen = new int[_ArRank];
        public static T CreateArray() =>
            CreateArray(_ArLen);

        public static object GetValueFromArray(T Array, params int[] Position) =>
            _GetValueFromArray(Array, Position);
        private static Func<T, int[], object> _GetValueFromArray =
        ((Func<Func<T, int[], object>>)(() =>
        {
            var Type = typeof(T);
            if (Type.IsArray == false)
                return (a, b) => throw new Exception($"Type of {Type} is not array type.");
            var Rank = Type.GetArrayRank();
            return DynamicMethodBuilder.Delegate<Func<T, int[], object>>("GetValue_", (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);

                for (int i = 0; i < Rank; i++)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldc_I4_S, i);
                    il.Emit(OpCodes.Ldelem_I4);
                }

                il.Emit(OpCodes.Call, Type.GetMethod("Get"));
                il.Emit(OpCodes.Box, Type.GetElementType());
                il.Emit(OpCodes.Ret);
            });
        }))();

        public static void SetValueToArray(T Array, object Value, params int[] Position) =>
            _SetValueToArray(Array, Value, Position);
        private static Action<T, object, int[]> _SetValueToArray =
        ((Func<Action<T, object, int[]>>)(() =>
        {
            var Type = typeof(T);
            if (Type.IsArray == false)
                return (a, b, c) => throw new Exception($"Type of {Type} is not array type.");
            var Rank = Type.GetArrayRank();
            return DynamicMethodBuilder.Delegate<Action<T, object, int[]>>("SetValue_",
            (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);

                for (int i = 0; i < Rank; i++)
                {
                    il.Emit(OpCodes.Ldarg_2);
                    il.Emit(OpCodes.Ldc_I4_S, i);
                    il.Emit(OpCodes.Ldelem_I4);
                }

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Unbox_Any, Type.GetElementType());

                il.Emit(OpCodes.Call, Type.GetMethod("Set"));

                il.Emit(OpCodes.Ret);
            });
        }))();
    }

    public class TypeController
    {

        public static FieldInfo[] GetAllFields(
            Type typeToReflect,
            BindingFlags? bindingFlags = null,
            Func<FieldInfo, bool> filter = null)
        {
            return GetAll(
                    typeToReflect,
                    (c) => c.Type.GetFields(c.BindingFlags),
                    bindingFlags,
                    filter);
        }

        public static MethodInfo[] GetAllMethods(
            Type typeToReflect,
            BindingFlags? bindingFlags = null,
            Func<MethodInfo, bool> filter = null)
        {
            return GetAll(
                    typeToReflect,
                    (c) => c.Type.GetMethods(c.BindingFlags),
                    bindingFlags,
                    filter);
        }

        public static MemberInfo[] GetAllMembers(
            Type typeToReflect,
            BindingFlags? bindingFlags = null,
            Func<MemberInfo, bool> filter = null)
        {
            return GetAll(
                    typeToReflect,
                    (c) => c.Type.GetMembers(c.BindingFlags),
                    bindingFlags,
                    filter);
        }

        public static T[] GetAll<T>(
            Type typeToReflect,
            Func<(Type Type, BindingFlags BindingFlags), T[]> GetMembers,
            BindingFlags? bindingFlags,
            Func<T, bool> filter = null)
            where T : MemberInfo
        {
#if DEBUG
            if (typeToReflect == null)
                throw new NullReferenceException("typeToReflect is null!");
#endif
            if (bindingFlags == null)
                bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;
            var Memebrs = GetMembers((typeToReflect, bindingFlags.Value));
            if (filter != null)
                Memebrs = Memebrs.Where(filter).ToArray();
            if (typeToReflect.BaseType != null)
            {
                var OldFilter = filter;
                if (filter == null)
                    filter = info => Memebrs.All((c) => c.MetadataToken != info.MetadataToken);
                else
                    filter = info => OldFilter(info) && Memebrs.All((c) => c.MetadataToken != info.MetadataToken);
                Insert(ref Memebrs,
                       GetAll(typeToReflect.BaseType, GetMembers,
                              bindingFlags,
                              filter));
            }
            return Memebrs;
        }

        public static Func<object, object> GetValue(Type Type, string FieldName)
        {
            var Field = Type.GetField(FieldName,
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.CreateInstance |
                    BindingFlags.Instance);
            return GetValue(Field);
        }

        public static Func<object, object> GetValue(FieldInfo Field)
        {
            return DynamicMethodBuilder.Delegate<Func<object, object>>("GetValue_", (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, Field.ReflectedType);
                il.Emit(OpCodes.Ldfld, Field);
                il.Emit(OpCodes.Box, Field.FieldType);
                il.Emit(OpCodes.Ret);
            });
        }

        public static Action<object, object> SetValue(Type Type, string FieldName)
        {
            var Field = Type.GetField(FieldName,
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.CreateInstance |
                    BindingFlags.Instance);
            return SetValue(Field);
        }

        public static Action<object, object> SetValue(FieldInfo Field)
        {
            if (Field.IsInitOnly)
                return (Target, Value) => Field.SetValue(Target, Value);

            return DynamicMethodBuilder.Delegate<Action<object, object>>("SetValue_", (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);
                if (Field.ReflectedType.IsValueType)
                    il.Emit(OpCodes.Unbox, Field.ReflectedType);
                else
                    il.Emit(OpCodes.Unbox_Any, Field.ReflectedType);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Unbox_Any, Field.FieldType);
                il.Emit(OpCodes.Stfld, Field);
                il.Emit(OpCodes.Ret);
            });
        }

        public static Func<object, int[], object> GetArray(Type Type)
        {
            var Rank = Type.GetArrayRank();
            return DynamicMethodBuilder.Delegate<Func<object, int[], object>>("GetValue_", (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, Type);

                for (int i = 0; i < Rank; i++)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldc_I4_S, i);
                    il.Emit(OpCodes.Ldelem_I4);
                }

                il.Emit(OpCodes.Call, Type.GetMethod("Get"));
                il.Emit(OpCodes.Box, Type.GetElementType());
                il.Emit(OpCodes.Ret);
            });
        }

        public static Action<object, object, int[]> SetArray(Type Type)
        {
            var Rank = Type.GetArrayRank();
            return DynamicMethodBuilder.Delegate<Action<object, object, int[]>>("SetValue_",
            (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, Type);

                for (int i = 0; i < Rank; i++)
                {
                    il.Emit(OpCodes.Ldarg_2);
                    il.Emit(OpCodes.Ldc_I4_S, i);
                    il.Emit(OpCodes.Ldelem_I4);
                }

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Unbox_Any, Type.GetElementType());

                il.Emit(OpCodes.Call, Type.GetMethod("Set"));

                il.Emit(OpCodes.Ret);
            });
        }

        public static Func<int[], object> CreateArray(Type Type, int Rank)
        {
            Type = System.Array.CreateInstance(Type, new int[Rank]).GetType();

            var methodName = "CreateInstance_";
            var dynMethod = new DynamicMethod(methodName, Type, new Type[] { typeof(int[]) }, true);
            ILGenerator il = dynMethod.GetILGenerator();

            for (int i = 0; i < Rank; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4_S, i);
                il.Emit(OpCodes.Ldelem_I4);
            }

            il.Emit(OpCodes.Newobj, Type.GetConstructors()[0]);
            il.Emit(OpCodes.Ret);
            return (Func<int[], object>)dynMethod.CreateDelegate(typeof(Func<int[], object>));
        }

        public static Func<int[], object> CreateArray(Type Type)
        {
            return CreateArray(Type.GetElementType(), Type.GetArrayRank());
        }

        //public static Delegate CreateDelegate(
        //    Type[] Params,
        //    Type Result,
        //    Action<object[]> Action,
        //    Func<object[], object> Func,
        //    Type DelegateType)
        //{
        //    var ParamsLength = Params.Length;
        //    var HaveResult = false;
        //    if (Result != null)
        //        if (Result != typeof(void))
        //            HaveResult = true;

        //    if (HaveResult)
        //        Insert(ref Params, typeof(Func<object[], object>), 0);
        //    else
        //        Insert(ref Params, typeof(Action<object[]>), 0);

        //    var dynMethod = DynamicMethodBuilder.Method("CreateInstance_", Params, Result,
        //    (il) =>
        //    {
        //        var local = il.DeclareLocal(typeof(object[]));
        //        il.Emit(OpCodes.Ldc_I4, ParamsLength);
        //        il.Emit(OpCodes.Newarr, typeof(object));
        //        MethodInfo invoke;
        //        if (HaveResult)
        //            invoke = typeof(Func<object[], object>).GetMethod("Invoke");
        //        else
        //            invoke = typeof(Action<object[]>).GetMethod("Invoke");
        //        for (int i = 1; i < ParamsLength + 1; i++)
        //        {
        //            il.Emit(OpCodes.Dup);
        //            var x = i - 1;
        //            il.Emit(OpCodes.Ldc_I4_S, x);
        //            il.Emit(OpCodes.Ldarg_S, i);
        //            il.Emit(OpCodes.Box, Params[i]);
        //            il.Emit(OpCodes.Stelem_Ref);
        //        }
        //        il.Emit(OpCodes.Stloc_0);
        //        il.Emit(OpCodes.Ldarg_0);
        //        il.Emit(OpCodes.Ldloc_0);
        //        il.Emit(OpCodes.Call, invoke);
        //        if (HaveResult)
        //            il.Emit(OpCodes.Unbox_Any, Result);
        //        il.Emit(OpCodes.Ret);
        //    });

        //    if (HaveResult)
        //        return dynMethod.CreateDelegate(DelegateType, Func);
        //    else
        //        return dynMethod.CreateDelegate(DelegateType, Action);
        //}

        public async static Task<T> Convert<T>(Task<object> task) => (T)(await task);

        private static SortedDictionary<Type, (int DGSelector, Func<object, Delegate> Wrapper)> DelegateWrappers =
            new SortedDictionary<Type, (int DGSelector, Func<object, Delegate> Wrapper)>(
                            Comparer<Type>.Create((c1,c2)=> c1.FullName.CompareTo(c2.FullName)));
        public static Delegate CreateDelegateWrapper(
            Type DelegateType,
            Action<object[]> Action,
            Func<object[], object> Func,
            Func<object[], Task> TaskAction,
            Func<object[], Task<object>> TaskFunc)
        {
            (int DGSelector, Func<object, Delegate> Wrapper) DelegateWrapper;

            if (DelegateWrappers.TryGetValue(DelegateType, out DelegateWrapper) == false)
            {
                var Method = DelegateType.GetMethod("Invoke");
                var Params = Method.GetParameters().Select((c) => c.ParameterType).ToArray();
                var Result = Method.ReturnType;
                object DGValue = null;

                var HaveResult = false;

                if (Result != null)
                    if (Result != typeof(void))
                        HaveResult = true;
                if (HaveResult)
                {
                    if (Result == typeof(Task))
                        DelegateWrapper.DGSelector = 3;
                    else if (Result.IsAssignableTo(typeof(Task<string>).BaseType))
                        DelegateWrapper.DGSelector = 4;
                    else
                        DelegateWrapper.DGSelector = 2;
                }
                else
                {
                    DelegateWrapper.DGSelector = 1;
                }

                switch (DelegateWrapper.DGSelector)
                {
                    case 1: //Action
                        DGValue = Action;
                        break;
                    case 2: //Func
                        DGValue = Func;
                        break;
                    case 3: //TaskAction
                        DGValue = TaskAction;
                        break;
                    case 4: //TaskFunc
                        DGValue = TaskFunc;
                        break;
                }

                var ParamsLength = Params.Length;

                var dynType = new TypeBuilder("Warpper", TypeAttributes.Public | TypeAttributes.Class);

                FieldBuilder Field;

                Field = dynType.DeclareField(FieldAttributes.Public, "DG", DGValue.GetType());

                var dynMethod = dynType.Method(MethodAttributes.Public, "Warp_Method", Params, Result,
                (il) =>
                {
                    var local = il.DeclareLocal(typeof(object[]));
                    il.Emit(OpCodes.Ldc_I4, ParamsLength);
                    il.Emit(OpCodes.Newarr, typeof(object));

                    for (int i = 1; i < ParamsLength + 1; i++)
                    {
                        il.Emit(OpCodes.Dup);
                        var x = i - 1;
                        il.Emit(OpCodes.Ldc_I4_S, x);
                        il.Emit(OpCodes.Ldarg_S, i);
                        il.Emit(OpCodes.Box, Params[i - 1]);
                        il.Emit(OpCodes.Stelem_Ref);
                    }
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, Field);
                    il.Emit(OpCodes.Ldloc_0);

                    switch (DelegateWrapper.DGSelector)
                    {
                        case 1: //Action
                            il.Emit(OpCodes.Call, typeof(Action<object[]>).GetMethod("Invoke"));
                            break;
                        case 2: //Func
                            il.Emit(OpCodes.Call, typeof(Func<object[], object>).GetMethod("Invoke"));
                            il.Emit(OpCodes.Unbox_Any, Result);
                            break;
                        case 3: //TaskAction
                            il.Emit(OpCodes.Call, typeof(Func<object[], Task>).GetMethod("Invoke"));
                            break;
                        case 4: //TaskFunc
                            il.Emit(OpCodes.Call, typeof(Func<object[], Task<object>>).GetMethod("Invoke"));
                            il.Emit(OpCodes.Call, typeof(TypeController).GetMethod("Convert").MakeGenericMethod(
                                Result.GetGenericArguments()[0]));
                            break;
                    }

                    il.Emit(OpCodes.Ret);
                });

                var Wrapper_Type = dynType.Create();
                var Wrapper_Ctor = Wrapper_Type.GetConstructors().First();
                var Wrapper_DG = Wrapper_Type.GetField("DG");
                var Wrapper_Method = Wrapper_Type.GetMethod("Warp_Method");

                DelegateWrapper.Wrapper = (DGValue) =>
                {
                    var ResultObj = Wrapper_Ctor.Invoke(null);
                    Wrapper_DG.SetValue(ResultObj, DGValue);
                    return Wrapper_Method.CreateDelegate(DelegateType, ResultObj);
                };

                DelegateWrappers.Add(DelegateType, DelegateWrapper);
            }

            {
                object DGValue = null;
                switch (DelegateWrapper.DGSelector)
                {
                    case 1: //Action
                        DGValue = Action;
                        break;
                    case 2: //Func
                        DGValue = Func;
                        break;
                    case 3: //TaskAction
                        DGValue = TaskAction;
                        break;
                    case 4: //TaskFunc
                        DGValue = TaskFunc;
                        break;
                }
                return DelegateWrapper.Wrapper(DGValue);
            }
        }

        public static Delegate CreateDelegateWrapper(
            Type DelegateType,
            Func<object[], object> Body)
        {
            return CreateDelegateWrapper(DelegateType, null, Body, null, null);
        }

        public static Delegate CreateDelegateWrapper(
            Type DelegateType,
            Action<object[]> Body)
        {
            return CreateDelegateWrapper(DelegateType, Body, null, null, null);
        }

        public static Delegate CreateDelegateWrapper(
            Delegate Delegate,
            Action<object[]> Action,
            Func<object[], object> Func,
            Func<object[], Task> TaskAction,
            Func<object[], Task<object>> TaskFunc)
        {
            return CreateDelegateWrapper(Delegate.GetType(), Action, Func, TaskAction, TaskFunc);
        }

        public static t CreateDelegateWrapper<t>(
            Action<object[]> Action,
            Func<object[], object> Func,
            Func<object[], Task> TaskAction,
            Func<object[], Task<object>> TaskFunc)
            where t : MulticastDelegate
        {
            return (t)CreateDelegateWrapper(typeof(t), Action, Func, TaskAction, TaskFunc);
        }

        public static t CreateDelegateWrapper<t>(
            t SampleDelegate,
            Action<object[]> Action,
            Func<object[], object> Func,
            Func<object[], Task> TaskAction,
            Func<object[], Task<object>> TaskFunc)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(Action, Func, TaskAction, TaskFunc);

        public static t CreateDelegateWrapper<t>(Action<object[]> Action)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(Action, null, null, null);
        public static t CreateDelegateWrapper<t>(Func<object[], object> Func)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(null, Func, null, null);
        public static t CreateDelegateWrapper<t>(Func<object[], Task> TaskAction)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(null, null, TaskAction, null);
        public static t CreateDelegateWrapper<t>(Func<object[], Task<object>> TaskFunc)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(null, null, null, TaskFunc);
        public static t CreateDelegateWrapper<t>(t SampleDelegate, Action<object[]> Action)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(Action, null, null, null);
        public static t CreateDelegateWrapper<t>(t SampleDelegate, Func<object[], object> Func)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(null, Func, null, null);
        public static t CreateDelegateWrapper<t>(t SampleDelegate, Func<object[], Task> TaskAction)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(null, null, TaskAction, null);
        public static t CreateDelegateWrapper<t>(t SampleDelegate, Func<object[], Task<object>> TaskFunc)
            where t : MulticastDelegate => CreateDelegateWrapper<t>(null, null, null, TaskFunc);

        public static Func<object> CreateInstance(Type Type)
        {
            var Rt = Type.GetType("System.RuntimeType");
            var nativeGetUninitializedObject = typeof(System.Runtime.Serialization.FormatterServices).
                GetMethod("nativeGetUninitializedObject", BindingFlags.Static | BindingFlags.NonPublic);
            return DynamicMethodBuilder.Delegate<Func<object>>("CreateInstance_",
            (il) =>
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, nativeGetUninitializedObject);
                il.Emit(OpCodes.Ret);
            }, Rt, Type);
        }

        public static Func<object> CreateInstance_Struct(Type Type)
        {

            var methodName = "CreateInstance_";
            var dynMethod = new DynamicMethod(methodName, typeof(object), null, true);

            ILGenerator il = dynMethod.GetILGenerator();
            var local = il.DeclareLocal(Type);
            il.Emit(OpCodes.Ldloca_S, local);
            il.Emit(OpCodes.Initobj, Type);
            il.Emit(OpCodes.Ldloc_S, local);
            il.Emit(OpCodes.Box, Type);
            il.Emit(OpCodes.Ret);
            return (Func<object>)dynMethod.CreateDelegate(typeof(Func<object>));
        }

        public static void Inject(MethodInfo Target, MethodInfo Inject)
        {
            RuntimeHelpers.PrepareMethod(Target.MethodHandle);
            RuntimeHelpers.PrepareMethod(Inject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)Inject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)Target.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                    // Version x86 Debug

                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Console.WriteLine("\nVersion x86 Release\n");
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)Inject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)Target.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    // Version x64 Debug
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Console.WriteLine("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
                }
            }
        }

        public static void Inject(Delegate Target, Delegate Inject)
        {
            TypeController.Inject(Target.Method, Inject.Method);
        }
        public static void Inject<t>(t Target, t Inject)
            where t:MulticastDelegate
        {
            TypeController.Inject(Target.Method, Inject.Method);
        }
    }

    public class TypeFields
    {
        public TypeFields(Type Type)
        {
            this.Type = Type;
            var Fields = TypeController.GetAllFields(Type, filter: (c) => c.IsStatic == false);
            this.Fields = new FieldControler[Fields.Length];
            for (int i = 0; i < Fields.Length; i++)
                this.Fields[i] = new FieldControler(Fields[i]);
        }
        public readonly FieldControler[] Fields;
        public readonly Type Type;
    }

    public class methodController
    {
        private static void HaveArgument(SDILReader.ILInstruction[] ils)
        {
            foreach (var op in ils)
            {
                if (op.Code == OpCodes.Ldarg_0)
                {
                    op.Code = OpCodes.Ldarg_1;
                }
                else if (op.Code == OpCodes.Ldarg_1)
                {
                    op.Code = OpCodes.Ldarg_2;
                }
                else if (op.Code == OpCodes.Ldarg_2)
                {
                    op.Code = OpCodes.Ldarg_3;
                }
                else if (op.Code == OpCodes.Ldarg_3)
                {
                    op.Code = OpCodes.Ldarg_S;
                    op.Operand = (byte)4;
                }
                else if (op.Code == OpCodes.Ldarg_S)
                {
                    op.Operand = (byte)((byte)op.Operand + 4);
                }

                if (op.Code == OpCodes.Starg_S)
                {
                    var t = op.Operand.GetType();
                    op.Operand = (byte)((byte)op.Operand + 1);
                }
            }
        }

        public static output join<output>(params output[] Methods)
        {
            object target = null;
            SDILReader.ILInstruction[][] ils = new SDILReader.ILInstruction[Methods.Length][];
            for (int mtd_i = 0; mtd_i < Methods.Length; mtd_i++)
            {

                var Dg = ((Delegate)(object)Methods[mtd_i]);
                var MethodReader = new SDILReader.MethodBodyReader(Dg.Method);
                ils[mtd_i] = MethodReader.instructions.ToArray();

                if (Dg.Target == null)
                    HaveArgument(ils[mtd_i]);
                else
                {
                    if (target != null)
                    {
                        if (Dg.Target.GetHashCode() != target.GetHashCode())
                            throw new Exception("multyTaget Not Suppport!");
                    }
                    else
                        target = Dg.Target;
                }
            }

            var OutputDg = typeof(output);
            MethodInfo Methodinfo = OutputDg.GetMethod("Invoke");

            var Parametrs = Methodinfo.GetParameters().Select((c) => c.ParameterType).ToArray();
            if (target != null)
                Insert(ref Parametrs, target.GetType(), 0);
            else
                Insert(ref Parametrs, typeof(object), 0);
            var methodName = "Joined_";
            var dynMethod = new DynamicMethod(methodName, Methodinfo.ReturnType, Parametrs, true);

            ILGenerator ilg = dynMethod.GetILGenerator();
            for (int mtd_i = 0; mtd_i < Methods.Length; mtd_i++)
            {

                var Dg = ((Delegate)(object)Methods[mtd_i]);

                var mb = Dg.Method.GetMethodBody();
                foreach (var lc in mb.LocalVariables)
                {
                    ilg.DeclareLocal(lc.LocalType);
                }

                foreach (var op in ils[mtd_i])
                {
                    if (op.Code == OpCodes.Br_S)
                    {
                        var OpLabel = System.Array.Find(ils[mtd_i], (c) => c.Offset == (int)op.Operand);
                        op.PointTo = OpLabel;
                        OpLabel.Label = ilg.DefineLabel();
                    }
                }
                foreach (var op in ils[mtd_i])
                {
                    if (op.Code != OpCodes.Ret)
                    {
                        op.IlEmiter(ilg);
                    }
                }
            }
            ilg.Emit(OpCodes.Ret);
            return (output)(object)dynMethod.CreateDelegate(typeof(output), target);
        }

        public static System.Collections.Generic.List<SDILReader.MethodBodyReader>
            GetILs(
            MethodInfo Method,
            System.Collections.Generic.List<SDILReader.MethodBodyReader> Methods = null)
        {

            var MethodReader = new SDILReader.MethodBodyReader(Method);
            if (Methods == null)
                Methods = new System.Collections.Generic.List<SDILReader.MethodBodyReader>();
            Methods.Add(MethodReader);
            if (MethodReader.instructions != null)
            {
                var NewCalls = MethodReader.instructions.Where((c) => c.Code == OpCodes.Call).
                    Where((c) => ((MethodInfo)c.Operand).ReturnParameter == null).
                    Where((c) => Methods.FirstOrDefault((m) => m.MethodInfo == (MethodInfo)c.Operand) == null);

                foreach (var Call in NewCalls)
                {
                    GetILs((MethodInfo)Call.Operand, Methods);
                }
            }

            return Methods;
        }

        public static output Inject<output>(output Method, output ReplaceInjected)
        {
            var OutputDg = typeof(output);
            MethodInfo Methodinfo = OutputDg.GetMethod("Invoke");
            var Dg = ((Delegate)(object)Method);

            var methodName = "Joined_";
            var dynMethod = new DynamicMethod(methodName, Methodinfo.ReturnType,
                Methodinfo.GetParameters().Select((c) => c.ParameterType).ToArray(), true);

            ILGenerator il = dynMethod.GetILGenerator();
            var MethodReader = new SDILReader.MethodBodyReader(Dg.Method);
            var ils = MethodReader.instructions.ToArray();

            {
                var mb = Dg.Method.GetMethodBody();
                foreach (var lc in mb.LocalVariables)
                {
                    il.DeclareLocal(lc.LocalType);
                }


                foreach (var op in ils)
                {
                    if (op.Code == OpCodes.Br_S)
                    {
                        var OpLabel = System.Array.Find(ils, (c) => c.Offset == (int)op.Operand);
                        op.PointTo = OpLabel;
                        OpLabel.Label = il.DefineLabel();
                    }
                }
            }


            var jMethodReader = new SDILReader.MethodBodyReader(((Delegate)(object)ReplaceInjected).Method);
            var jils = jMethodReader.instructions.ToArray();

            {
                var mb = Dg.Method.GetMethodBody();
                foreach (var lc in mb.LocalVariables)
                {
                    il.DeclareLocal(lc.LocalType);
                }


                foreach (var op in jils)
                {
                    if (op.Code == OpCodes.Br_S)
                    {
                        var OpLabel = System.Array.Find(ils, (c) => c.Offset == (int)op.Operand);
                        op.PointTo = OpLabel;
                        OpLabel.Label = il.DefineLabel();
                    }
                }
            }

            foreach (var op in ils)
            {
                if (op.Code != OpCodes.Ret)
                {
                    if (op.Code == OpCodes.Call && ((MethodInfo)op.Operand) == ((Delegate)(Action)Injected).Method)
                    {
                        foreach (var jop in jils)
                        {
                            if (jop.Code != OpCodes.Ret)
                            {
                                jop.IlEmiter(il);
                            }
                        }
                    }
                    else
                        op.IlEmiter(il);
                }
            }
            il.Emit(OpCodes.Ret);
            return (output)(object)dynMethod.CreateDelegate(typeof(output));
        }

        public static void Injected()
        {
            Console.WriteLine("ij");
        }
    }
}