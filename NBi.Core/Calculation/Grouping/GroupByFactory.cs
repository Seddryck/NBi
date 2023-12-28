﻿using NBi.Core.Calculation.Grouping.CaseBased;
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

namespace NBi.Core.Calculation.Grouping
{
    public class GroupByFactory
    {
        public static IGroupBy None() => new NoneGrouping();

        public IGroupBy Instantiate(IGroupByArgs args)
        {
            switch (args)
            {
                case NoneGroupByArgs x: return None();
                case ColumnGroupByArgs x: return Instantiate(x.Columns, x.Context);
                case CaseGroupByArgs x: return new CaseGrouping(x.Cases, x.Context);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private IGroupBy Instantiate(IEnumerable<IColumnDefinitionLight> columns, Context context)
        {
            if (!columns?.Any() ?? false)
                return new NoneGrouping();

            var definitions = new List<ColumnDefinition>();
            foreach (var column in columns!)
            {
                var definition = new ColumnDefinition()
                {
                    Identifier = column.Identifier,
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
                return new OrdinalColumnGrouping(settings as SettingsOrdinalResultSet, context);

            else if (settings is SettingsNameResultSet)
                return new NameColumnGrouping(settings as SettingsNameResultSet, context);

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
