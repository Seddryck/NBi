using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Scalar.Comparer;

namespace NBi.Core.ResultSet
{
    public class SettingsOrdinalResultSet : SettingsResultSet<int>
    {
        public enum KeysChoice
        {
            [XmlEnum(Name = "first")]
            First = 0,
            [XmlEnum(Name = "all-except-last")]
            AllExpectLast = 1,
            [XmlEnum(Name = "all")]
            All = 2,
            [XmlEnum(Name = "none")]
            None = 3,
        }

        public enum ValuesChoice
        {
            [XmlEnum(Name = "all-except-first")]
            AllExpectFirst = 0,
            [XmlEnum(Name = "last")]
            Last = 1,
            [XmlEnum(Name = "none")]
            None = 2
        }

        public KeysChoice KeysDef { get; set; }
        private ValuesChoice ValuesDef { get; set; }

        protected override bool IsKey(int index)
        {

            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Role != ColumnRole.Key))
                return false;

            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Role == ColumnRole.Key))
                return true;

            return KeysDef switch
            {
                KeysChoice.First => index == 0,
                KeysChoice.AllExpectLast => index != GetLastColumnOrdinal(),
                KeysChoice.All => true,
                _ => false,
            };
        }

        protected override bool IsValue(int index)
        {
            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Role != ColumnRole.Value))
                return false;

            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Role == ColumnRole.Value))
                return true;

            switch (KeysDef)
            {
                case KeysChoice.First:
                    if (index == 0) return false;
                    break;
                case KeysChoice.AllExpectLast:
                    if (index != GetLastColumnOrdinal()) return false;
                    break;
                case KeysChoice.All:
                    return false;
            }

            return ValuesDef switch
            {
                ValuesChoice.AllExpectFirst => index != 0,
                ValuesChoice.Last => index == GetLastColumnOrdinal(),
                ValuesChoice.None => false,
                _ => false,
            };
        }

        public override bool IsRounding(int index)
        {
            return ColumnsDef.Any(
                    c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index
                    && c.Role == ColumnRole.Value
                    && c.RoundingStyle != Rounding.RoundingStyle.None
                    && !string.IsNullOrEmpty(c.RoundingStep));
        }

        public override Rounding? GetRounding(int index)
        {
            if (!IsRounding(index))
                return null;

            return RoundingFactory.Build(ColumnsDef.Single(
                    c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index
                    && c.Role == ColumnRole.Value));
        }

        public override ColumnRole GetColumnRole(int index)
        {
            if (!cacheRole.ContainsKey(index))
            {
                if (IsKey(index))
                    cacheRole.Add(index, ColumnRole.Key);
                else if (IsValue(index))
                    cacheRole.Add(index, ColumnRole.Value);
                else
                    cacheRole.Add(index, ColumnRole.Ignore);
            }

            return cacheRole[index];
        }

        public override ColumnType GetColumnType(int index)
        {
            if (!cacheType.ContainsKey(index))
            {
                if (IsNumeric(index))
                    cacheType.Add(index, ColumnType.Numeric);
                else if (IsDateTime(index))
                    cacheType.Add(index, ColumnType.DateTime);
                else if (IsBoolean(index))
                    cacheType.Add(index, ColumnType.Boolean);
                else
                    cacheType.Add(index, ColumnType.Text);
            }
            return cacheType[index];
        }

        protected override bool IsType(int index, ColumnType type)
        {
            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Type != type))
                return false;

            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Type == type))
                return true;

            return (IsValue(index) && ValuesDefaultType == type);
        }

        public override Tolerance? GetTolerance(int index)
        {
            if (GetColumnType(index) != ColumnType.Numeric && GetColumnType(index) != ColumnType.DateTime && GetColumnType(index) != ColumnType.Text)
                return null;

            var col = ColumnsDef.FirstOrDefault(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index);
            if (col == null || !col.IsToleranceSpecified)
            {
                return GetColumnType(index) switch
                {
                    ColumnType.Text => (DefaultTolerance as TextSingleMethodTolerance) ?? TextTolerance.None,
                    ColumnType.Numeric => (DefaultTolerance as NumericTolerance) ?? NumericAbsoluteTolerance.None,
                    ColumnType.DateTime => (DefaultTolerance as DateTimeTolerance) ?? DateTimeTolerance.None,
                    _ => null,
                };
            }

            return new ToleranceFactory().Instantiate(col);
        }

        public int GetLastColumnOrdinal()
        {
            if (!isLastColumnOrdinalDefined)
                throw new InvalidOperationException("You must call the method ApplyTo() before trying to call GetLastColumnIndex()");

            return lastColumnOrdinal;
        }

        public int GetMinColumnOrdinalDefined()
        {
            if (ColumnsDef.Count > 0)
                return ColumnsDef.Where(cd => cd.Identifier is ColumnOrdinalIdentifier).Min(cd => ((ColumnOrdinalIdentifier)(cd.Identifier)).Ordinal);
            else
                return -1;
        }

        public int GetMaxColumnOrdinalDefined()
        {
            if (ColumnsDef.Count > 0)
                return ColumnsDef.Where(cd => cd.Identifier is ColumnOrdinalIdentifier).Max(cd => ((ColumnOrdinalIdentifier)(cd.Identifier)).Ordinal);
            else
                return -1;
        }

        public int GetLastKeyColumnOrdinal()
        {
            var max = 0;
            for (int i = 0; i < GetLastColumnOrdinal(); i++)
            {
                if (IsKey(i))
                    max = i;
            }

            return max;
        }

        private bool isLastColumnOrdinalDefined = false;
        private int lastColumnOrdinal;

        public void ApplyTo(int columnCount)
        {
            isLastColumnOrdinalDefined = true;
            lastColumnOrdinal = columnCount - 1;
        }

        protected SettingsOrdinalResultSet(ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
        { }

        public SettingsOrdinalResultSet(int columnsCount, KeysChoice keysDef, ValuesChoice valuesDef)
            : this(keysDef, valuesDef, ColumnType.Numeric, NumericAbsoluteTolerance.None, [])
        {
            ApplyTo(columnsCount);
        }

        public SettingsOrdinalResultSet(IReadOnlyCollection<IColumnDefinition> columnsDef)
            : this(KeysChoice.None, ValuesChoice.None, ColumnType.Numeric, NumericAbsoluteTolerance.None, columnsDef)
        { }

        public SettingsOrdinalResultSet(KeysChoice keysDef, ValuesChoice valuesDef, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : this(keysDef, valuesDef, ColumnType.Numeric, NumericAbsoluteTolerance.None, columnsDef)
        {
        }

        public SettingsOrdinalResultSet(KeysChoice keysDef, ValuesChoice valuesDef, Tolerance defaultTolerance)
            : this(keysDef, valuesDef, ColumnType.Numeric, defaultTolerance, [])
        {
        }

        public SettingsOrdinalResultSet(KeysChoice keysDef, ValuesChoice valuesDef, ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
        {
            KeysDef = keysDef;
            ValuesDef = valuesDef;

        }

    }
}
