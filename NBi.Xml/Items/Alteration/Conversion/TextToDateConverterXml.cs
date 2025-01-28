using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Items.Alteration.Conversion;

public class TextToDateConverterXml : AbstractConverterXml
{
    public override string From => "text";

    public override string To => "date";
}
