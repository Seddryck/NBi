using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Model.Constraints.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation;

public class SinglePredication : BasePredication
{
    [Obsolete("Deprecated. Use operand in place of column-index")]
    public int ColumnIndex
    {
        get => throw new InvalidOperationException();
        set => Operand = new ColumnIdentifierFactory().Instantiate($"#{value}");
    }

    public IColumnIdentifier? Operand { get; set; }

    [Obsolete("Deprecated. Use operand in place of name")]
    public string Name { get => Operand!.Label; set => Operand=new ColumnIdentifierFactory().Instantiate(value); }
}

public abstract class BasePredication : AbstractPredication
{
    public BasePredication()
    {
        ColumnType = ColumnType.Numeric;
    }

    public ColumnType ColumnType { get; set; }

    public Predicate? Predicate { get; set; }
}
