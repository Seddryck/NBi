using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    class CSharpScalarResolver<T> : IScalarResolver<T>
    {
        private readonly CSharpScalarResolverArgs args;

        public CSharpScalarResolver(CSharpScalarResolverArgs args)
        {
            this.args = args;
        }

        public T Execute()
        {
            var method = CreateFunction(args.Code);
            if (method == null)
                throw new InvalidOperationException();

            var value = method.Invoke(null, new object[] { });

            return (T)Convert.ChangeType(value, typeof(T));
        }

        private MethodInfo CreateFunction(string code)
        {
            string codeTemplate = @"
                using System;
            
                namespace NBi.Core.Variable.Dynamic
                {{                
                    public class VariableClass
                    {{                
                        public static object Function()
                        {{
                            return {0};
                        }}
                    }}
                }}
            ";

            string finalCode = string.Format(codeTemplate, code);

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

            Type function = results.CompiledAssembly.GetType("NBi.Core.Variable.Dynamic.VariableClass");
            return function.GetMethod("Function");
        }
    }
}