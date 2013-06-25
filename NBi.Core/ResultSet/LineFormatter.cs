using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NBi.Core.ResultSet;

namespace NBi.Core.ResultSet
{
    public interface ICellFormatter
    {
        int GetCellLength();
        string GetText(int length);
    }

    public class LineFormatter
    {

        public static ICellFormatter BuildHeader(DataTable table, int columnIndex)
        {
            var role = (ColumnRole)table.Columns[columnIndex].ExtendedProperties["NBi::Role"];
            var type = (ColumnType)table.Columns[columnIndex].ExtendedProperties["NBi::Type"];
            var tolerance = (decimal)table.Columns[columnIndex].ExtendedProperties["NBi::Tolerance"];

            return Build(role, type, tolerance);
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

        protected static ICellFormatter Build(ColumnRole role, ColumnType type, decimal tolerance)
        {
            return new HeaderFormatter(role, type, tolerance);
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

            public ColumnRole Role { get; set; }
            public ColumnType Type { get; set; }
            public decimal Tolerance { get; set; }

            public HeaderFormatter(ColumnRole role, ColumnType type, decimal tolerance)
            {
                Role = role;
                Type = type;
                Tolerance = tolerance;
            }

            public virtual int GetCellLength()
            {
                if (Role == ColumnRole.Ignore)
                    return 0;

                return GetRoleText().Length + GetTypeText().Length + GetToleranceText().Length + 5;
            }

            public virtual string GetText(int length)
            {
                var roleText = GetRoleText();

                var typeText = GetTypeText();

                var value = string.Format("{0} ({1})", roleText, typeText);

                var toleranceText = GetToleranceText();

                value += new string(' ', Math.Max(0, length - (value.Length + toleranceText.Length))) + toleranceText;

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
                if (Tolerance > 0)
                    toleranceText += string.Format(" (+/- {0}) ", Tolerance);
                return toleranceText;
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
                if (Value is DBNull)
                    return "(null)";
                return Value.ToString();
            }

            public virtual string GetText(int length)
            {
                return GetValueText() + new string(' ', Math.Max(0, length - GetValueText().Length));
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
                return Value.ToString();
            }

            private string GetComparedText()
            {
                if (Compared is DBNull)
                    return "(null)";
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
        }
    }
}
