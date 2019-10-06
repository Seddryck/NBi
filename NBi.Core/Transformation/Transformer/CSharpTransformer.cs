using Microsoft.CSharp;
using NBi.Core.Injection;
using NBi.Core.Scalar.Casting;
using NBi.Core.Variable;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer
{
    class CSharpTransformer<T> : ITransformer
    {
        private ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }
        private MethodInfo method;

        public CSharpTransformer() : this(null, null) { }
        public CSharpTransformer(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public void Initialize(string code)
        {
            method = CreateFunction(code, typeof(T).Name);
        }

        public object Execute(object value)
        {
            if (method == null)
                throw new InvalidOperationException();

            var factory = new CasterFactory<T>();
            var caster = factory.Instantiate();
            var typedValue = caster.Execute(value);
            var transformedValue = method.Invoke(null, new object[] { typedValue });

            return transformedValue;
        }

        private MethodInfo CreateFunction(string code, string type)
        {
            string codeTemplate = $@"
                using System;
            
                namespace NBi.Core.Transformation.Dynamic
                {{                
                    public class TransformationClass
                    {{                
                        public static object Function({type} value)
                        {{
                            return {code};
                        }}
                    }}
                }}
            ";

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, codeTemplate);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine($"Error ({error.ErrorNumber}): {error.ErrorText}");
                }

                throw new InvalidOperationException(sb.ToString());
            }

            Type function = results.CompiledAssembly.GetType("NBi.Core.Transformation.Dynamic.TransformationClass");
            return function.GetMethod("Function");
        }
    }
}
