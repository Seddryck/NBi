using NBi.Core.Calculation.Grouping.CaseBased;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.SettingsOrdinalResultSet;

namespace NBi.Core.Calculation.Grouping;

public class GroupByFactory
{
    public static IGroupBy None() => new NoneGrouping();

    public IGroupBy Instantiate(IGroupByArgs args)
        => args switch
        {
            NoneGroupByArgs x => None(),
            ColumnGroupByArgs x => Instantiate(x.Columns, x.Context),
            CaseGroupByArgs x => new CaseGrouping(x.Cases, x.Context),
            _ => throw new ArgumentOutOfRangeException(),
        };

    protected virtual IGroupBy Instantiate(IEnumerable<IColumnDefinitionLight> columns, Context context)
    {
        if (!columns?.Any() ?? false)
            return new NoneGrouping();

        var definitions = columns!.Select(column =>  new ColumnDefinition(column.Identifier,column.Type)).ToList();

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(KeysChoice.None, ValuesChoice.None);
        builder.Setup(definitions);
        builder.Build();

        var settings = builder.GetSettings();
        return settings switch
        {
            SettingsOrdinalResultSet ordinal => new OrdinalColumnGrouping(ordinal, context),
            SettingsNameResultSet name => new NameColumnGrouping(name, context),
            _ => throw new ArgumentOutOfRangeException(nameof(settings))
        };
    }

    private class ColumnDefinition(IColumnIdentifier identifier, ColumnType type) : IColumnDefinition
    {
        public IColumnIdentifier Identifier { get; set; } = identifier;
        public ColumnRole Role { get => ColumnRole.Key; set => throw new NotImplementedException(); }
        public ColumnType Type { get; set; } = type;

        public string Tolerance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsToleranceSpecified => false;

        public Rounding.RoundingStyle RoundingStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RoundingStep { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ITransformationInfo Transformation => throw new NotImplementedException();
    }
}
