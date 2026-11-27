using NBi.Core.ResultSet;
using NBi.Model.Items.ResultSet;
using System.Collections.Generic;

namespace NBi.Model.Constraints;

public class UniqueRows : AbstractConstraint
{

    public UniqueRows()
    {
        KeysSet = KeySelection.All;
        ValuesSet = ValueSelection.None;
    }

    public KeySelection KeysSet { get; set; }

    public ValueSelection ValuesSet { get; set; }

    public List<ColumnDefinition> Columns { get; set; }

}
