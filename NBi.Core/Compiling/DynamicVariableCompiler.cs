using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Compiling
{
    internal class DynamicVariableCompiler : CSharpCompiler
    {
        protected override string[] TemplateVariables => [Namespace];
        protected override string Template
            => @"
                using System;
                using System.Xml;
                using System.Xml.Linq;
                using System.Linq;
                using System.Xml.XPath;
            
                namespace {1}
                {{                
                    public class DynamicVariableClass
                    {{                
                        public static object? Function()
                        {{
                            return {0};
                        }}
                    }}
                }}
            ";

        public override void Compile(string code)
        {
            base.Compile(code);
            Prepare("DynamicVariableClass", "Function");
        }

        public object? Evaluate()
            => Evaluate([]);
    }
}
