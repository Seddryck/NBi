using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using NBi.Extensibility;

namespace NBi.Core.Assemblies
{
    /// <summary>
    /// This class let you load an assembly and execute a method of it. It's just a kind of wrapper over the reflection namespace
    /// </summary>
    public class AssemblyManager
    {
        /// <summary>
        /// Create an object of the specified class
        /// </summary>
        /// <param name="assemblyPath">The full or relative path of the assembly containing the class from which you want to instantiate an object</param>
        /// <param name="typeName">The type of the object to instantiate</param>
        /// <param name="ctorParameters">SettingsValue of the parameters for the constructor of the type</param>
        /// <returns>An instance of the specified class</returns>
        public virtual object GetInstance(string assemblyPath, string typeName, object[] ctorParameters)
        {
            if (!Path.IsPathRooted(assemblyPath))
                assemblyPath = Path.GetFullPath(assemblyPath);

            if (!File.Exists(assemblyPath))
                throw new ExternalDependencyNotFoundException(assemblyPath);

            var assembly = Assembly.LoadFile(assemblyPath);
            var type = assembly.GetType(typeName) ?? throw new ArgumentException(string.Format("Type {0} not found in assembly located at '{1}'", typeName, assemblyPath), "typeName");
            var classInstance = Activator.CreateInstance(type, ctorParameters) ?? throw new NullReferenceException();
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
         
        /// <summary>
        /// Returns the type to call static methods
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public virtual Type GetStatic(string assemblyPath, string typeName)
        {
            var assembly = Assembly.LoadFile(assemblyPath);
            var type = assembly.GetType(typeName);
            return type == null
                ? throw new ArgumentException(string.Format("Type {0} not found in assembly located at '{1}'", typeName, assemblyPath), "typeName")
                : type;
        }
        /// <summary>
        /// Execute the method of the an object. Let you specify the value of the parameters of the method called.
        /// </summary>
        /// <param name="target">The object on which you want to apply a method</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="parameters">The pair of names and values for each parameter of the method</param>
        /// <returns></returns>
        public virtual object? Execute(object target, string methodName, IDictionary<string, object> parameters)
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var methodInfo = (target.GetType()?.GetMethod(methodName, flags)) ?? throw new ArgumentException(string.Format("Method named '{0}' not found in type '{1}'", methodName, target.GetType()),"methodName");
            var paramList = new List<object>();
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
	        {
                var converter = new TypeConverter();
                var value = converter.Convert(parameters[paramInfo.Name ?? throw new NotSupportedException()], paramInfo.ParameterType);
                paramList.Add(value);
	        }

            var result = methodInfo.Invoke(target, paramList.ToArray());
            return result;
        }

        /// <summary>
        /// Execute the static method of the a type. Let you specify the value of the parameters of the method called.
        /// </summary>
        /// <param name="target">The type on which you want to apply a method</param>
        /// <param name="methodName">The name of the static method</param>
        /// <param name="parameters">The pair of names and values for each parameter of the method</param>
        /// <returns></returns>
        public virtual object? ExecuteStatic(Type type, string methodName, IDictionary<string, object> parameters)
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            var methodInfo = type.GetMethod(methodName, flags) ?? throw new ArgumentException(string.Format("Static method named '{0}' not found in type '{1}'", methodName, type), "methodName");
            var paramList = new List<object>();
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
            {
                var converter = new TypeConverter();
                var value = converter.Convert(parameters[paramInfo.Name ?? throw new NotSupportedException()], paramInfo.ParameterType);
                paramList.Add(value);
            }

            var result = methodInfo.Invoke(null, [.. paramList]);
            return result;
        }
    }
}
