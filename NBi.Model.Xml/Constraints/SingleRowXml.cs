using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Items.ResultSet;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Constraints.Comparer;
using NBi.Model.Xml.Items.Calculation;
using NBi.Core.Evaluate;

namespace NBi.Model.Xml.Constraints;

public class SingleRowXml : NoRowsXml
{
    public SingleRowXml() : base()
    {  }
}
