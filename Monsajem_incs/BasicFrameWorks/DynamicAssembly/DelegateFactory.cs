﻿using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
namespace Monsajem_Incs.DynamicAssembly
{
    public class DelegateTypeFactory
    {
        private readonly ModuleBuilder m_module;

        public DelegateTypeFactory()
        {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("MN_DynamicAssembly." + Guid.NewGuid().ToString()), AssemblyBuilderAccess.RunAndCollect);
            m_module = assembly.DefineDynamicModule("MN_DynamicDelegateTypeFactory");
        }

        public Type CreateDelegateType(MethodInfo method)
        {
            string nameBase = string.Format("{0}{1}", method.DeclaringType.Name, method.Name);
            string name = GetUniqueName(nameBase);

            var typeBuilder = m_module.DefineType(
                name, TypeAttributes.Sealed | TypeAttributes.Public, typeof(MulticastDelegate));

            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public,
                CallingConventions.Standard, new[] { typeof(object), typeof(IntPtr) });
            constructor.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);

            var parameters = method.GetParameters();

            var invokeMethod = typeBuilder.DefineMethod(
                "Invoke", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Public,
                method.ReturnType, parameters.Select(p => p.ParameterType).ToArray());
            invokeMethod.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                _ = invokeMethod.DefineParameter(i + 1, ParameterAttributes.None, parameter.Name);
            }

            return typeBuilder.CreateType();
        }

        private string GetUniqueName(string nameBase)
        {
            int number = 2;
            string name = nameBase;
            while (m_module.GetType(name) != null)
                name = nameBase + number++;
            return name;
        }
    }

}
