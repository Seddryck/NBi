using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.SettingsOrdinalResultSet;

namespace NBi.Core.Calculation.Grouping
{
    public class ByColumnGroupingFactory
    {
        public IByColumnGrouping None() => new NoneGrouping();

        public IByColumnGrouping Instantiate(IEnumerable<IColumnDefinitionLight> columns)
        {
            if ((columns?.Count() ?? 0) == 0)
                return new NoneGrouping();

            var definitions = new List<ColumnDefinition>();
            foreach (var column in columns)
            {
                var definition = new ColumnDefinition()
                {
                    Identifier = column.Identifier as ColumnOrdinalIdentifier,
                    Type = column.Type
                };
                definitions.Add(definition);
            }

            var builder = new SettingsEquivalerBuilder();
            builder.Setup(KeysChoice.None, ValuesChoice.None);
            builder.Setup(definitions);
            builder.Build();

            var settings = builder.GetSettings();
            if (settings is SettingsOrdinalResultSet)
                return new OrdinalByColumnGrouping(settings as SettingsOrdinalResultSet);

            else if (settings is SettingsNameResultSet)
                return new NameByColumnGrouping(settings as SettingsNameResultSet);

            throw new ArgumentOutOfRangeException(nameof(settings));
        }

        private class ColumnDefinition : IColumnDefinition
        {
            public IColumnIdentifier Identifier { get; set; }
            public ColumnRole Role { get => ColumnRole.Key; set => throw new NotImplementedException(); }
            public ColumnType Type { get; set; }


            public string Tolerance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool IsToleranceSpecified => false;

            public Rounding.RoundingStyle RoundingStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string RoundingStep { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public ITransformationInfo Transformation => throw new NotImplementedException();
        }
    }
}
