using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet.Formatter
{
    class HeaderFormatter
    {
        public HeaderFormatter()
        {
        }

        public IList<string> GetDisplay(Header header)
        {
            var list = new List<string>();
            list.Add(GetFieldNameText(header));
            list.Add(string.Format("{0} - {1}", GetRoleText(header), GetTypeText(header)));
            list.Add(string.Format("{0}{1}", GetToleranceText(header), GetRoundingText(header)));

            return list;
        }

        internal IEnumerable<string> Tabulize(Header header, int totalWidth)
        {
            var table = GetDisplay(header);
            for (int i = 0; i < table.Count(); i++)
                table[i] = table[i].PadRight(totalWidth);
            return table;
        }

        private string GetFieldNameText(Header header)
        {
            return string.IsNullOrEmpty(header.FieldName)? string.Empty : header.FieldName;
        }

        private string GetRoleText(Header header)
        {
            switch (header.Role)
            {
                case ColumnRole.Key:
                    return "KEY";
                case ColumnRole.Value:
                    return "VALUE";
                default:
                    return string.Empty;
            }
        }

        private string GetTypeText(Header header)
        {
            switch (header.Type)
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

        private string GetToleranceText(Header header)
        {
            var toleranceText = string.Empty;
            if (header.Tolerance != null)
                toleranceText += string.Format(" (+/- {0}) ", header.Tolerance.ValueString);
            return toleranceText;
        }

        private string GetRoundingText(Header header)
        {
            var roundingText = string.Empty;
            if (header.Rounding != null)
                roundingText += string.Format(" ({0} {1}) ", GetRoundingStyleText(header), header.Rounding.Step);
            return roundingText;
        }

        private string GetRoundingStyleText(Header header)
        {
            switch (header.Rounding.Style)
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
}
