using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Compiling;

internal class TransformerCompiler<T> : CSharpCompiler
{
    protected override string[] TemplateVariables
        => [Namespace, GetType().GetGenericArguments()[0].Name];

    protected override string Template
        => @"
                using System;
            
                namespace {1}
                {{                
                    public class TransformerClass
                    {{                
                        public static object Function({2} value)
                        {{
                            return {0};
                        }}
                    }}
                }}
            ";

    public override void Compile(string code)
    {
        base.Compile(code);
        Prepare("TransformerClass", "Function");
    }

    public object? Evaluate(T value)
        => Evaluate([value]);
}
