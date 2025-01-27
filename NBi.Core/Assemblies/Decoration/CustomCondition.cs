using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies.Decoration;

class CustomCondition : IDecorationCondition
{
    private ICustomCondition Target { get; }

    public CustomCondition(ICustomCondition target) => Target = target;

    public string? Message { get; set; }

    public bool Validate()
    {
        var result = Target.Execute();
        Message = result.Message;
        return result.IsValid;
    }
}
