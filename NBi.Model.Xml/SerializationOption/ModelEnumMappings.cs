using System.Xml.Serialization;

namespace NBi.Model.Xml.SerializationOption;

internal static class ModelEnumMappings
{
    public static XmlAttributeOverrides Create()
    {
        var attributes = new XmlAttributeOverrides();
        AddTo(attributes);
        return attributes;
    }

    public static void AddTo(XmlAttributeOverrides attributes)
    {
        Add(attributes, NBi.Core.Analysis.Member.Pattern.Contain, "contain");
        Add(attributes, NBi.Core.Analysis.Member.Pattern.EndWith, "end-with");
        Add(attributes, NBi.Core.Analysis.Member.Pattern.Exact, "exact");
        Add(attributes, NBi.Core.Analysis.Member.Pattern.StartWith, "start-with");
        Add(attributes, NBi.Core.Calculation.CombinationOperator.And, "and");
        Add(attributes, NBi.Core.Calculation.CombinationOperator.Or, "or");
        Add(attributes, NBi.Core.Calculation.CombinationOperator.XOr, "xor");
        Add(attributes, NBi.Core.Format.CurrencyPattern.Prefix, "$n");
        Add(attributes, NBi.Core.Format.CurrencyPattern.PrefixSpace, "$ n");
        Add(attributes, NBi.Core.Format.CurrencyPattern.Suffix, "n$");
        Add(attributes, NBi.Core.Format.CurrencyPattern.SuffixSpace, "n $");
        Add(attributes, NBi.Core.Members.Predefined.PredefinedMembers.DaysOfWeek, "days-of-week");
        Add(attributes, NBi.Core.Members.Predefined.PredefinedMembers.MonthsOfYear, "months-of-year");
        Add(attributes, NBi.Core.Members.Ranges.PositionValue.Prefix, "prefix");
        Add(attributes, NBi.Core.Members.Ranges.PositionValue.Suffix, "suffix");
        Add(attributes, NBi.Core.ResultSet.Alteration.Duplication.OutputClass.Index, "index");
        Add(attributes, NBi.Core.ResultSet.Alteration.Duplication.OutputClass.IsDuplicable, "is-duplicable");
        Add(attributes, NBi.Core.ResultSet.Alteration.Duplication.OutputClass.IsOriginal, "is-original");
        Add(attributes, NBi.Core.ResultSet.Alteration.Duplication.OutputClass.Script, "script");
        Add(attributes, NBi.Core.ResultSet.Alteration.Duplication.OutputClass.Static, "static");
        Add(attributes, NBi.Core.ResultSet.Alteration.Duplication.OutputClass.Total, "total");
        Add(attributes, NBi.Core.ResultSet.Alteration.Merging.ColumnIdentity.Name, "name");
        Add(attributes, NBi.Core.ResultSet.Alteration.Merging.ColumnIdentity.Ordinal, "ordinal");
        Add(attributes, NBi.Core.ResultSet.ColumnRole.Ignore, "ignore");
        Add(attributes, NBi.Core.ResultSet.ColumnRole.Key, "key");
        Add(attributes, NBi.Core.ResultSet.ColumnRole.Value, "value");
        Add(attributes, NBi.Core.ResultSet.ColumnType.Boolean, "boolean");
        Add(attributes, NBi.Core.ResultSet.ColumnType.DateTime, "dateTime");
        Add(attributes, NBi.Core.ResultSet.ColumnType.Numeric, "numeric");
        Add(attributes, NBi.Core.ResultSet.ColumnType.Text, "text");
        Add(attributes, NBi.Core.ResultSet.Combination.SequenceCombinationOperation.CartesianProduct, "cartesian-product");
        Add(attributes, NBi.Core.Scalar.Comparer.Rounding.RoundingStyle.Ceiling, "ceiling");
        Add(attributes, NBi.Core.Scalar.Comparer.Rounding.RoundingStyle.Floor, "floor");
        Add(attributes, NBi.Core.Scalar.Comparer.Rounding.RoundingStyle.None, "none");
        Add(attributes, NBi.Core.Scalar.Comparer.Rounding.RoundingStyle.Round, "round");
        Add(attributes, NBi.Core.Scalar.Projection.ProjectionType.RowCount, "row-count");
        Add(attributes, NBi.Core.Sequence.Resolver.Loop.IntervalMode.Close, "close");
        Add(attributes, NBi.Core.Sequence.Resolver.Loop.IntervalMode.HalfOpen, "half-open");
        Add(attributes, NBi.Core.Transformation.LanguageType.CSharp, "c-sharp");
        Add(attributes, NBi.Core.Transformation.LanguageType.Format, "format");
        Add(attributes, NBi.Core.Transformation.LanguageType.Native, "native");
        Add(attributes, NBi.Core.Transformation.LanguageType.NCalc, "ncalc");
    }

    private static void Add<TEnum>(XmlAttributeOverrides attributes, TEnum value, string name)
        where TEnum : struct, Enum
        => attributes.Add(typeof(TEnum), value.ToString(), new XmlAttributes { XmlEnum = new XmlEnumAttribute(name) });
}
