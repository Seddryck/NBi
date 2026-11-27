using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Conversion;

public class TextToDateConverter : AbstractConverter
{
    public override string From => "text";

    public override string To => "date";
}
