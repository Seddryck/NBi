using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NBi.Core.Evaluate;

namespace NBi.Xml.Constraints;

public class SomeRowsXml : AllRowsXml
{
    public SomeRowsXml() : base()
    {  }
}
