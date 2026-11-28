using NBi.Model.Items.ResultSet;
using NBi.Model.Items.ResultSet.Lookup;
using NBi.Model.Settings;
using NBi.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints;

public class LookupMatches : AbstractConstraint
{
    public Join? Join { get; set; }

    public Inclusion? Inclusion { get; set; }

    public ResultSetSystem? ResultSet { get; set; }

    [Obsolete("Replaced by result-set")]
    public ResultSetSystem? ResultSetOld
    {
        get => ResultSet;
        set { ResultSet = value; }
    }

    public override Default? Default
    {
        get { return base.Default; }
        set
        {
            base.Default = value;
            if (ResultSet != null)
                ResultSet.Default = value;
        }
    }
}
