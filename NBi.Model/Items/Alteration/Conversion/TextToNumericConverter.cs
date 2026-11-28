using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Conversion;

public class TextToNumericConverter : AbstractConverter
{
    public override string From => "text";

    public override string To => "numeric";
}
