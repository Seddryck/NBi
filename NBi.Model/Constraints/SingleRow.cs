using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Model.Items;
using NBi.Model.Items.ResultSet;
using NBi.Model.Settings;
using NBi.Model.Constraints.Comparer;
using NBi.Model.Items.Calculation;
using NBi.Core.Evaluate;

namespace NBi.Model.Constraints;

public class SingleRow : NoRows
{
    public SingleRow()
        : base()
    {  }
}
