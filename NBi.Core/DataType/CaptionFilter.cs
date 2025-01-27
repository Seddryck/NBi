using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

public class CaptionFilter
{
    public Target Target { get; private set; }
    public string Caption { get; private set; }

    public CaptionFilter(Target target, string caption)
    {
        Target = target;
        Caption = caption;
    }
}
