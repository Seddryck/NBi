using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NBi.Core.Assemblies
{
    public class AssemblyManager
    {
        public object GetInstance(string assemblyPath, string typeName, object[] ctorParameters)
        {
            var assembly = Assembly.LoadFile(assemblyPath);
            var type = assembly.GetType(typeName);
            if (type == null)
                throw new ArgumentException(string.Format("Type {0} not found in assembly located at '{1}'", typeName, assemblyPath), "typeName");

            var classInstance = Activator.CreateInstance(type, ctorParameters);
            return classInstance;
        }

        //public object GetInstance(string assemblyPath, string typeName, object[] ctorParameters)
        //{
        //    var ads = new AppDomainSetup();
        //    ads.PrivateBinPath = @"C:\Program Files (x86)\Elia\Project\NBi 073\NBi.Testing\bin\Release";

        //    var NewAppDomain = System.AppDomain.CreateDomain("NBiAssemblyDomain");
        //    NewAppDomain.AppendPrivatePath(@"C:\Program Files (x86)\Elia\Project\NBi 073\NBi.Testing\bin\Release");

        //    var classInstance = NewAppDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeName, ctorParameters);
        //    return classInstance;     
        //}
         

        public Type GetStatic(string assemblyPath, string typeName)
        {
            var assembly = Assembly.LoadFile(assemblyPath);
            var type = assembly.GetType(typeName);
            if (type == null)
                throw new ArgumentException(string.Format("Type {0} not found in assembly located at '{1}'", typeName, assemblyPath), "typeName");

            return type;
        }

        public object Execute(object target, string methodName, IDictionary<string, object> parameters)
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            MethodInfo methodInfo = target.GetType().GetMethod(methodName, flags);
            if (methodInfo == null)
                throw new ArgumentException(string.Format("Method named '{0}' not found in type '{1}'", methodName, target.GetType()),"methodName");

            var paramList = new List<object>();
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
	        {
                var converter = new TypeConverter();
                var value = converter.Convert(parameters[paramInfo.Name], paramInfo.ParameterType);
                paramList.Add(value);
	        }

            var result = methodInfo.Invoke(target, paramList.ToArray());
            return result;
        }

        public object ExecuteStatic(Type type, string methodName, IDictionary<string, object> parameters)
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            MethodInfo methodInfo = type.GetMethod(methodName, flags);
            if (methodInfo == null)
                throw new ArgumentException(string.Format("Method named '{0}' not found in type '{1}'", methodName, type), "methodName");

            var paramList = new List<object>();
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
            {
                var converter = new TypeConverter();
                var value = converter.Convert(parameters[paramInfo.Name], paramInfo.ParameterType);
                paramList.Add(value);
            }

            var result = methodInfo.Invoke(null, paramList.ToArray());
            return result;
        }
    }
}
