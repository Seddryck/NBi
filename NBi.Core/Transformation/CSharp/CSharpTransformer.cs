using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.CSharp
{
    class CSharpTransformer<T> : ITransformer
    {
        private MethodInfo method;

        public CSharpTransformer()
        {
        }

        public void Initialize(string code)
        {
            method = CreateFunction(code, typeof(T).Name);
        }

        public object Execute(object value)
        {
            if (method == null)
                throw new InvalidOperationException();
            return method.Invoke(null, new[] { value });
        }

        private MethodInfo CreateFunction(string code, string type)
        {
            string codeTemplate = @"
                using System;
            
                namespace NBi.Core.Transformation.Dynamic
                {{                
                    public class TransformationClass
                    {{                
                        public static object Function({1} value)
                        {{
                            return {0};
                        }}
                    }}
                }}
            ";

            string finalCode = string.Format(codeTemplate, code, type);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, finalCode);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }

            Type function = results.CompiledAssembly.GetType("NBi.Core.Transformation.Dynamic.TransformationClass");
            return function.GetMethod("Function");
        }
    }
}
