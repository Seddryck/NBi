using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Xml.Items.Alteration.Conversion;

public class TextToNumericConverterXml : AbstractConverterXml
{
    public override string From => "text";

    public override string To => "numeric";
}
