using Microsoft.CSharp;
using NBi.Core.Transformation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    class CSharpTestVariable : ITestVariable
    {
        private object value;
        private bool isEvaluated;
        private readonly string code;

        public string Code { get { return code; } }
        public LanguageType Language { get { return LanguageType.CSharp; } }

        public CSharpTestVariable(string code)
        {
            this.code = code;
        }

        public object GetValue()
        {
            if (!IsEvaluated())
            {
                value = Evaluate();
                isEvaluated = true;
            }

            return value;
        }

        public bool IsEvaluated()
        {
            return isEvaluated;
        }

        protected virtual object Evaluate()
        {
            var method = CreateFunction(code);
            if (method == null)
                throw new InvalidOperationException();

            var transformedValue = method.Invoke(null, new object[] { });

            return transformedValue;
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
