﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using NBi.Extensibility.Resolving;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NBi.Core.Scalar.Resolver
{
    class CSharpScalarResolver<T> : IScalarResolver<T>
    {
        private readonly CSharpScalarResolverArgs args;

        public CSharpScalarResolver(CSharpScalarResolverArgs args)
        {
            this.args = args;
        }

        public CSharpScalarResolver(string code)
        {
            this.args = new CSharpScalarResolverArgs(code);
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
                using System.Xml;
                using System.Xml.Linq;
                using System.Linq;
                using System.Xml.XPath;
            
                namespace {1}
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

            string finalCode = string.Format(codeTemplate, code, $"{GetType().Namespace}.Dynamic");

            using (var provider = new CSharpCodeProvider())
            {
                var parameters = new CompilerParameters()
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false,
                    ReferencedAssemblies =
                    {
                        "System.Xml.dll",
                        "System.Xml.Linq.dll",
                        "System.Linq.dll",
                        "System.Core.dll",
                        "System.Xml.XPath.dll"
                    }
                };

                var results = provider.CompileAssemblyFromSource(parameters, finalCode);

                if (results.Errors.HasErrors)
                {
                    var sb = new StringBuilder();
                    foreach (CompilerError error in results.Errors)
                        sb.AppendLine($"Error ({error.ErrorNumber}): {error.ErrorText}");

                    throw new InvalidOperationException(sb.ToString());
                }

                var @class = results.CompiledAssembly.GetType($"{GetType().Namespace}.Dynamic.VariableClass");
                return @class.GetMethod("Function");
            }
        }

        object IResolver.Execute() => Execute();
    }
}