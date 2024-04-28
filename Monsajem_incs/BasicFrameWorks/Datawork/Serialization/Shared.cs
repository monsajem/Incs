using Monsajem_Incs.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Monsajem_Incs
{
    namespace Assembly
    {
        public class Assembly
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            static Assembly()
            {
                AddAssembly(typeof(Assembly).Assembly);
                AddAssembly(typeof(int).Assembly);
                void LasLoader()
                {
                    var Asms = LoadedAssemblies;
                    var IsFirst = true;
                    foreach (var Asm in Asms)
                    {
                        var InnerAsms = Asm.GetReferencedAssemblies();
                        foreach (var InnerAsm in InnerAsms)
                        {
                            var InnerAsm_Name = InnerAsm;
                            void Loader()
                            {
                                try
                                {
                                    AddAssembly(System.Reflection.Assembly.Load(InnerAsm_Name));
                                }
                                catch { }
                            }
                            if (IsFirst)
                            {
                                Loader();
                                IsFirst = false;
                            }
                            else
                                Insert(ref LoadAssemblies, Loader);
                        }
                    }
                }
                LoadAssemblies = new Action[]
                {
                    () => AddAssembly(AppDomain.CurrentDomain.GetAssemblies()),
                    () => AddAssembly(System.Reflection.Assembly.GetExecutingAssembly()),
                    () => AddAssembly(System.Reflection.Assembly.GetCallingAssembly()),
                    () => AddAssembly(System.Reflection.Assembly.GetEntryAssembly()),
                    LasLoader
                };
            }

            public static bool AllAppAssembliesLoaded { get => LoadAssemblies == null; }
            public static System.Reflection.Assembly[] AllAppAssemblies
            {
                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (AllAppAssembliesLoaded == false)
                    {
                        while (LoadDeeperAssemblys()) { }
                        LoadAssemblies = null;
                    }
                    return LoadedAssemblies;
                }
            }

            private static Action[] LoadAssemblies;
            private static System.Reflection.Assembly[] LoadedAssemblies = new System.Reflection.Assembly[0];

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public static void AddAssembly(params System.Reflection.Assembly[] Assemblys)
            {
                if (Assemblys == null)
                    return;
                Assemblys = Assemblys.Where((c) => c != null).ToArray();
                if (Assemblys.Length == 0)
                    return;
                Insert(ref LoadedAssemblies, Assemblys);
                LoadedAssemblies = LoadedAssemblies.GroupBy((c) => c.FullName).Select((c) => c.First()).ToArray();
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public static void TryLoadAssembely(string Name)
            {
                try
                {
                    AddAssembly(System.Reflection.Assembly.Load(Name));
                }
                catch { }
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public static bool LoadDeeperAssemblys()
            {
                if (LoadAssemblies != null)
                {
                    if (LoadAssemblies.Length > 0)
                    {
                        Pop(ref LoadAssemblies)();
                        if (LoadAssemblies.Length == 0)
                            LoadAssemblies = null;
#if TRACE
                        Console.WriteLine("Deeper Assemblys Loaded!");
#endif
                        return true;
                    }
                }
#if TRACE
                Console.WriteLine("All Assemblies Loaded!");
#endif
                return false;
            }

            private static HashSet<TypeHolder> Types = [];
            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            internal static Type GetType(string TypeName)
            {
                static void AddType(Type Type, string TypeName)
                {
                    if (Types.Contains(new TypeHolder(TypeName)) == false)
                        Types.Add(new TypeHolder(Type, TypeName));
                    if (Types.Contains(new TypeHolder(Type.ToString())) == false)
                        Types.Add(new TypeHolder(Type, Type.ToString()));
                    if (Types.Contains(new TypeHolder(Type.FullName)) == false)
                        Types.Add(new TypeHolder(Type, Type.FullName));
                }
                static System.Type GetTypeByName(string TypeName)
                {
                    if (Types.TryGetValue(new TypeHolder(TypeName), out var TypeGot))
                        return TypeGot.Type;
#if TRACE
                    var LoadMorAsms = false;
#endif
                    while (true)
                    {
                        foreach (var Asm in LoadedAssemblies)
                        {
                            var Type = Asm.GetType(TypeName);
                            if (Type != null)
                            {
#if TRACE
                                if(LoadMorAsms)
                                    Console.WriteLine(Asm.FullName + " Assembly Auto Loaded!");
#endif
                                AddType(Type, TypeName);
                                return Type;
                            }
                        }
#if TRACE
                        LoadMorAsms = true;
#endif
                        if (LoadDeeperAssemblys() == false)
                            throw new TypeLoadException("Type Not Found >> " + TypeName);
                    }
                }

                if (Types.TryGetValue(new TypeHolder(TypeName), out var TypeGot))
                    return TypeGot.Type;

                var FirstTypeName = TypeName;

                Type Type = null;
                var Type_P = new Function<char>((c, p) => c[p] != '[' && c[p] != ']' && c[p] != ',')
                { Info = "Type" };
                var Next_P = new Function<char>((c, p) => c[p] == ',')
                { Info = "Next" };
                var Inner_P = new Function<char>((c, p) => (c[p] == '[' || c[p] == ',', 1))
                { Info = "SubType" };

                Type_P.AddSub(Next_P, Inner_P);
                Inner_P.AddSub(Type_P);

                var Browsed = Type_P.Browse(TypeName.ToCharArray());
                if (Browsed.SubFunctions.Length > 1 &&
                    Browsed.SubFunctions[1].SubFunctions.Length > 1)
                    TypeName = new string(Browsed.SubFunctions[0].Values);

                Type = GetTypeByName(TypeName);
                if (Type.IsGenericType)
                {
                    var Types = new Type[0];
                    var AllSubs = Browsed[1][1].SubFunctions.AsEnumerable();
                    while (AllSubs.Count() > 0)
                    {
                        var Subs = AllSubs.TakeWhile((c) => c.Info != "Next");
                        AllSubs = AllSubs.SkipWhile((c) => c.Info != "Next").Skip(1);
                        var SubType = "";
                        foreach (var Sub in Subs)
                            SubType += new string(Sub.Compile);
                        Insert(ref Types, GetType(SubType));
                    }
                    Type = Type.MakeGenericType(Types);
                    if (Browsed.SubFunctions.Length > 2)
                    {
                        var Rank = Browsed[2].Compile.Length - 1;
                        Type = Rank == 1 ? Type.MakeArrayType() : Type.MakeArrayType(Rank);
                    }
                }
                AddType(Type, FirstTypeName);
                return Type;
            }

            private class TypeHolder : IEquatable<TypeHolder>
            {
                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public TypeHolder(Type Type, string TypeName) : this(TypeName)
                {
                    this.Type = Type;
                }
                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public TypeHolder(string TypeName)
                {
                    this.TypeName = TypeName;
                    HashCode = TypeName.GetHashCode();
                }

                public Type Type;
                public string TypeName;
                public int HashCode;

                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public bool Equals(TypeHolder other)
                {
                    return TypeName == other.TypeName;
                }

                [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
                public override int GetHashCode()
                {
                    return HashCode;
                }
            }

        }
    }
    internal static class Shared
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        internal static string MidName(this Type Type)
        {
            return Type.ToString();
        }
    }
}
