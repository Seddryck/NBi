using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Conversion;

public class Convert : AlterationDefinition
{
    public string Column { get; set; }

    public AbstractConverter Converter { get; set; }
}
