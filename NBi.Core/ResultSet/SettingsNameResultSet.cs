using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Scalar.Comparer;
using System.Collections.ObjectModel;

namespace NBi.Core.ResultSet
{
	public class SettingsNameResultSet : SettingsResultSet<string>
	{
        public IReadOnlyCollection<string> KeyNames { get; private set; }
        public IReadOnlyCollection<string> ValueNames { get; private set; }

        protected override bool IsKey(string name)
        {
            if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Role != ColumnRole.Key))
                return false;

            if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Role == ColumnRole.Key))
                return true;

            if (KeyNames.Contains(name))
                return true;

            if (ValueNames.Contains(name))
                return false;

            if (ValueNames.Count>0 && KeyNames.Count==0)
                return true;

            return false;
        }
        
        protected override bool IsValue(string name)
        {
            if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Role != ColumnRole.Value))
                return false;

            if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Role == ColumnRole.Value))
                return true;

            if (KeyNames.Contains(name))
                return false;

            if (ValueNames.Contains(name))
                return true;

            if (ValueNames.Count > 0)
                return false;

            return false;
        }
        
        public override bool IsRounding(string name)
        {
            return ColumnsDef.Any(
                    c => (c.Identifier as ColumnNameIdentifier)?.Name == name
                    && c.Role == ColumnRole.Value
                    && c.RoundingStyle != Rounding.RoundingStyle.None
                    && !string.IsNullOrEmpty(c.RoundingStep));
        }
        
        public override Rounding? GetRounding(string name)
        {
            if (!IsRounding(name))
                return null;

            return RoundingFactory.Build(ColumnsDef.Single(
                    c => (c.Identifier as ColumnNameIdentifier)?.Name == name
                    && c.Role == ColumnRole.Value));
        }
        
        public override ColumnRole GetColumnRole(string name)
        {
            if (!cacheRole.ContainsKey(name))
            {
                if (IsKey(name))
                    cacheRole.Add(name, ColumnRole.Key);
                else if (IsValue(name))
                    cacheRole.Add(name, ColumnRole.Value);
                else
                    cacheRole.Add(name, ColumnRole.Ignore);
            }

            return cacheRole[name];
        }
        
        public override ColumnType GetColumnType(string name)
        {
            if (!cacheType.ContainsKey(name))
            {
                if (IsNumeric(name))
                    cacheType.Add(name, ColumnType.Numeric);
                else if (IsDateTime(name))
                    cacheType.Add(name, ColumnType.DateTime);
                else if (IsBoolean(name))
                    cacheType.Add(name, ColumnType.Boolean);
                else
                    cacheType.Add(name, ColumnType.Text);
            }
            return cacheType[name];
        }
        
        protected override bool IsType(string name, ColumnType type)
        {
            if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Type != type))
                return false;

            if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Type == type))
                return true;

            if (IsKey(name))
                return type ==ColumnType.Text;

            return (IsValue(name) && ValuesDefaultType == type);
        }
        
        public override Tolerance? GetTolerance(string name)
        {
            if (GetColumnType(name) != ColumnType.Numeric && GetColumnType(name) != ColumnType.DateTime)
                return null;

            var col = ColumnsDef.FirstOrDefault(c => (c.Identifier as ColumnNameIdentifier)?.Name == name);
            if (col == null || !col.IsToleranceSpecified)
            {
                if (GetColumnType(name)==ValuesDefaultType)
                    return DefaultTolerance;
                else
                {
                    if (IsNumeric(name))
                        return NumericAbsoluteTolerance.None;
                    else if (IsDateTime(name))
                        return DateTimeTolerance.None;
                    else
                        return null;
                }
            }

            return new ToleranceFactory().Instantiate(col);
        }
        

        public SettingsNameResultSet(string keyNames, string valueNames, ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
        {
            KeyNames = new ReadOnlyCollection<string>([]);
            if (!string.IsNullOrEmpty(keyNames))
            {
                var keys = keyNames.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                KeyNames = new ReadOnlyCollection<string>(keys.Select(x => x.Trim()).ToList());
            }

            ValueNames = new ReadOnlyCollection<string>([]);
            if (!string.IsNullOrEmpty(valueNames))
            {
                var values = valueNames.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                ValueNames = new ReadOnlyCollection<string>(values.Select(x => x.Trim()).ToList());
            }
        }

        public SettingsNameResultSet(ColumnType valuesDefaultType, Tolerance defaultTolerance, IEnumerable<IColumnDefinition> columnsDef)
        : base(valuesDefaultType, defaultTolerance, new ReadOnlyCollection<IColumnDefinition>(columnsDef.ToList()))
        {
            KeyNames = [];
            ValueNames = [];
        }

        internal SettingsNameResultSet(IEnumerable<IColumnDefinition> columnsDef)
        : base(ColumnType.Numeric, NumericAbsoluteTolerance.None, new ReadOnlyCollection<IColumnDefinition>(columnsDef.ToList()))
        {
            KeyNames = [];
            ValueNames = [];
        }

        public IEnumerable<string> GetKeyNames()
        {
            var result = new List<string>(KeyNames);
            result.AddRange(ColumnsDef.Where(c => c.Role==ColumnRole.Key).Select(c => (c.Identifier as ColumnNameIdentifier)?.Name ?? throw new NullReferenceException()));
            return result.Distinct();
        }

        public IEnumerable<string> GetValueNames()
        {
            var result = new List<string>(ValueNames);
            result.AddRange(ColumnsDef.Where(c => c.Role == ColumnRole.Value).Select(c => (c.Identifier as ColumnNameIdentifier)?.Name ?? throw new NullReferenceException()));
            return result.Distinct();
        }

        public IEnumerable<string> GetColumnNames()
            => GetKeyNames().Union(GetValueNames());
    }
}
