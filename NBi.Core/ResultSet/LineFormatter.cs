using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet
{
    public interface ICellFormatter
    {
        int GetCellLength();
        string GetText(int length);
        string GetColumnName(int length);
    }

    public class LineFormatter
    {

        public static ICellFormatter BuildHeader(DataTable table, int columnIndex)
        {
            if (table.Columns[columnIndex].ExtendedProperties.Count == 0)
                return null;

            var name = table.Columns[columnIndex].ColumnName;
            var role = (ColumnRole)table.Columns[columnIndex].ExtendedProperties["NBi::Role"];
            var type = (ColumnType)table.Columns[columnIndex].ExtendedProperties["NBi::Type"];
            var tolerance = (Tolerance)table.Columns[columnIndex].ExtendedProperties["NBi::Tolerance"];
            var rounding = (Rounding)table.Columns[columnIndex].ExtendedProperties["NBi::Rounding"];

            return Build(name, role, type, tolerance, rounding);
        }

        public static ICellFormatter Build(DataRow row, int columnIndex)
        {
            var value = row.ItemArray[columnIndex];
            var compared = row.GetColumnError(columnIndex);
            var isDifferent = !string.IsNullOrEmpty(row.GetColumnError(columnIndex));
            if (isDifferent)
                return Build(value, compared, isDifferent);
            else
                return Build(value);
        }

        public static ICellFormatter BuildValue(DataRow row, int columnIndex)
        {
            var value = row.ItemArray[columnIndex];
            return Build(value);
        }

        protected static ICellFormatter Build(string name,ColumnRole role, ColumnType type, Tolerance tolerance, Rounding rounding)
        {
            return new HeaderFormatter(name, role, type, tolerance, rounding);
        }

        protected static ICellFormatter Build(object value)
        {
            return new CellFormatter(value);
        }

        protected static ICellFormatter Build(object value, object compared, bool isDifferent)
        {
            return new CellComparedFormatter(value, compared, isDifferent);
        }



        private class HeaderFormatter : ICellFormatter
        {
            public string Name { get; set; }
            public ColumnRole Role { get; set; }
            public ColumnType Type { get; set; }
            public Tolerance Tolerance { get; set; }
            public Rounding Rounding { get; set; }

            public HeaderFormatter(string name, ColumnRole role, ColumnType type, Tolerance tolerance, Rounding rounding)
            {
                Name = name;
                Role = role;
                Type = type;
                Tolerance = tolerance;
                Rounding = rounding;
            }

            public virtual int GetCellLength()
            {
                if (Role == ColumnRole.Ignore)
                    return 0;

                return Math.Max(Name.Length + 1, GetRoleText().Length + GetTypeText().Length + GetToleranceText().Length + 5);
            }

            public virtual string GetText(int length)
            {
                var roleText = GetRoleText();

                var typeText = GetTypeText();

                var value = string.Format("{0} ({1})", roleText, typeText);

                var toleranceText = GetToleranceText();
                var roundingText = GetRoundingText();

                value += new string(' ', Math.Max(0, length - (value.Length + toleranceText.Length + roundingText.Length))) + toleranceText + roundingText;

                return value;
            }

            public virtual string GetColumnName(int length)
            {
                var value = Name + new string(' ', Math.Max(0, length - (Name.Length)));

                return value;
            }

            private string GetRoleText()
            {
                switch (Role)
                {
                    case ColumnRole.Key:
                        return "KEY";
                    case ColumnRole.Value:
                        return "VALUE";
                    default:
                        return string.Empty;
                }
            }

            private string GetTypeText()
            {
                switch (Type)
                {
                    case ColumnType.Numeric:
                        return "Numeric"; 
                    case ColumnType.Text:
                        return "Text"; 
                    case ColumnType.DateTime:
                        return "DateTime";
                    case ColumnType.Boolean:
                        return "Boolean"; 
                }
                return "?";
            }

            private string GetToleranceText()
            {
                var toleranceText = string.Empty;
                if (Tolerance!=null)
                    toleranceText += string.Format(" (+/- {0}) ", Tolerance.ValueString);
                return toleranceText;
            }

            private string GetRoundingText()
            {
                var roundingText = string.Empty;
                if (Rounding != null)
                    roundingText += string.Format(" ({0} {1}) ", GetRoundingStyleText(), Rounding.Step);
                return roundingText;
            }

            private string GetRoundingStyleText()
            {
                switch (this.Rounding.Style)
                {
                    case Rounding.RoundingStyle.None:
                        return string.Empty;
                    case Rounding.RoundingStyle.Floor:
                        return "floor";
                    case Rounding.RoundingStyle.Round:
                        return "round";
                    case Rounding.RoundingStyle.Ceiling:
                        return "ceiling";
                }
                return "?";
            }

            
        }

        private class CellFormatter : ICellFormatter
        {

            public object Value { get; set; }


            public CellFormatter(object value)
            {
                Value = value;
            }

            public virtual int GetCellLength()
            {
                return GetValueText().Length + 2;
            }

            private string GetValueText()
            {
                if (Value is DBNull || Value==null)
                    return "(null)";
                if (Value is string && ((string)Value).Length == 0)
                    return "(empty)";
                if (Value is string && ((string)Value).Trim().Length == 0)
                    return "(blank)";
                return Value.ToString();
            }

            public virtual string GetText(int length)
            {
                return GetValueText() + new string(' ', Math.Max(0, length - GetValueText().Length));
            }

            public string GetColumnName(int length)
            {
                throw new NotImplementedException();
            }
        }

        private class CellComparedFormatter : ICellFormatter
        {

            public object Value { get; set; }
            public object Compared { get; set; }
            public bool IsDifferent { get; set; }


            public CellComparedFormatter(object value, object compared, bool isDifferent)
            {
                Value = value;
                Compared = compared;
                IsDifferent = isDifferent;
            }

            public virtual int GetCellLength()
            {
                if (IsDifferent)
                    return GetValueText().Length + GetComparedText().Length + 7;
                else
                    return GetValueText().Length + 2;
            }

            private string GetValueText()
            {
                if (Value is DBNull)
                    return "(null)";
                if (Value is string && ((string)Value).Length == 0)
                    return "(empty)";
                return Value.ToString();
            }

            private string GetComparedText()
            {
                if (Compared is DBNull)
                    return "(null)";
                if (Compared is string && ((string)Compared).Length == 0)
                    return "(empty)";
                return Compared.ToString();
            }

            private string GetIsDifferentText()
            {
                if (IsDifferent)
                    return "<>";
                return string.Empty;
            }

            public virtual string GetText(int length)
            {
                return string.Format(" {0} {1} {2} {3} "
                    , GetValueText()
                    , GetIsDifferentText()
                    , GetComparedText()
                    , new string(' ', Math.Max(0, length - GetValueText().Length - GetIsDifferentText().Length - GetComparedText().Length - 5)));
            }

            public string GetColumnName(int length)
            {
                throw new NotImplementedException();
            }
        }
    }
}
