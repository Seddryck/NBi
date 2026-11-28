using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Conversion;

public abstract class AbstractConverter
{
    public string Culture { get; set; }

    public string DefaultValue { get; set; }

    public abstract string From { get; }
    public abstract string To { get; }
}
