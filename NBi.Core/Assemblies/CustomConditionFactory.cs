using NBi.Extensibility.Condition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies
{
    public class CustomConditionFactory
    {
        public IDecorationCondition Instantiate(ICustomConditionArgs metadata)
        {
            var assembly = GetAssembly(metadata.AssemblyPath);
            try
            {
                var type = GetType(assembly, metadata.TypeName);
                return new CustomCondition(Instantiate(type, metadata.Parameters));
            }
            catch (TypeNotExistingException)
            { throw new NBiException($"The assembly '{assembly.FullName}' doesn't contain any type named '{metadata.TypeName}'. This type was describe in the test as a custom condition."); }
            catch (TypeNotImplementingInterfaceException)
            { throw new NBiException($"The type '{metadata.TypeName}' of the assembly '{assembly.FullName}' is not implementing the interface '{typeof(ICustomCondition).Name}' but is used as a custom condition. Custom conditions must implement this interface."); }
            catch (NoConstructorFoundException)
            { throw new NBiException($"The type '{metadata.TypeName}' of the assembly '{assembly.FullName}' has no constructor matching with the {metadata.Parameters.Count()} parameter{(metadata.Parameters.Count()>1 ? "s" : string.Empty)} that {(metadata.Parameters.Count() > 1 ? "were" : "was")} provided."); }
        }

        protected internal virtual Assembly GetAssembly(string path)
        {
            var assemblyPath = Path.IsPathRooted(path)
                ? path
                : Path.GetFullPath(path);

            if (!File.Exists(assemblyPath))
                throw new ExternalDependencyNotFoundException(assemblyPath);

            var assembly = Assembly.LoadFile(assemblyPath);
            return assembly;
        }

        protected internal Type GetType(Assembly assembly, string typeName)
        {
            var type = typeName.Contains('.') 
                ? assembly.GetType(typeName, false, true) 
                : assembly.GetTypes().FirstOrDefault(x => x.Name == typeName);
            if (type == null)
                throw new TypeNotExistingException();

            if (!type.GetInterfaces().Contains(typeof(ICustomCondition)))
                throw new TypeNotImplementingInterfaceException();
            return type;
        }

        protected internal ICustomCondition Instantiate(Type customConditionType, IReadOnlyDictionary<string, object> parameters)
        {
            var ctor = customConditionType.GetConstructors().FirstOrDefault(
                c => c.GetParameters().All(p => parameters.Keys.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
                && c.GetParameters().Count() == parameters.Count()
            );
            if (ctor == null)
                throw new NoConstructorFoundException();

            var typeConverter = new TypeConverter();
            var ctorParams = ctor.GetParameters().Select(
                p => typeConverter.Convert(
                    parameters.First( x => string.Compare(x.Key, p.Name, true)==0 ).Value
                    , p.ParameterType)
                ).ToArray();
            var instance = ctor.Invoke(ctorParams) as ICustomCondition;

            return instance;
        }

        private class TypeNotExistingException : ArgumentException { }
        private class TypeNotImplementingInterfaceException : ArgumentException { }
        private class NoConstructorFoundException : ArgumentException { }
    }
}
