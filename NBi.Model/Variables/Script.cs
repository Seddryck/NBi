using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Variables;

public class Script
{
    public LanguageType Language { get; set; }

    public string Code { get; set; }
}
