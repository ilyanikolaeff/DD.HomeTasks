using System.Reflection;

namespace ReflectionBlock
{
    internal class AssemblyHelper
    {
        public Assembly LoadAssembly(string assemblyPath)
        {
            var dll = Assembly.LoadFile(assemblyPath);
            return dll;
        }

        public Type GetTypeInfo(Assembly assembly, string className)
        {
            var typeInfo = assembly.GetTypes().First(type => type.Name.Equals(className, StringComparison.OrdinalIgnoreCase));
            return typeInfo;
        }

        public MethodInfo GetMethodInfo(Type typeInfo, string methodName)
        {
            var methodInfo = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(method => method.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));
            return methodInfo;
        }

        public object? CallMethod(Type typeInfo, MethodInfo methodInfo, object[] methodParameters)
        {
            var instance = Activator.CreateInstance(typeInfo);
            var result = methodInfo.Invoke(instance, methodParameters);
            return result;
        }
    }
}