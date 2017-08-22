using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NBi.Core.ResultSet
{
    public class ResultSetTextWriter : ResultSetAbstractWriter
    {

        public ResultSetTextWriter() : base(string.Empty) { }
        
        protected override void OnWrite(string filename, System.Data.DataSet ds, string tableName)
        {
            throw new NotImplementedException();
        }

        protected override void OnWrite(string filename, ResultSet rs)
        {
            throw new NotImplementedException();
        }

        protected const int MAX_ROWS_DISPLAYED = 10;

        protected virtual IList<int> GetFieldsLength(IEnumerable<DataRow> rows)
        {

            var fieldsLength = new List<int>();

            if (rows.Count() == 0)
                return new List<int>(0);

            for (int i = 0; i < rows.ElementAt(0).ItemArray.Count(); i++)
            {
                var cells = new List<ICellFormatter>();
                //Header
                var displayHeader = LineFormatter.BuildHeader(rows.ElementAt(0).Table, i);
                if (displayHeader != null)
                    cells.Add(displayHeader);

                //For each row
                foreach (var r in rows)
                {
                    var display = LineFormatter.Build(r, i);
                    cells.Add(display);
                }
                fieldsLength.Add(cells.Max(f => f.GetCellLength()));
            }

            return fieldsLength;
        }

        protected virtual string GetSeparator(IEnumerable<int> fieldLength)
        {
            //Cells'length + "|" & 2 spaces for each cell + the ending "|"
            return new string('-', fieldLength.Sum() + fieldLength.Count() * 3 + 1);
        }

        protected virtual string GetFirstIndentation()
        {
            return new string(' ', 4);
        }

        public virtual IEnumerable<string> BuildContent(IEnumerable<DataRow> rows, int rowCount, bool compare)
        {

            var output = new List<string>();
            //Empty resultset

            if (rows.Count() == 0)
            {
                output.Add("This result set is empty.");
                return output;
            }

            //calculate row count to diplay
            int maxRows = (rowCount <= Math.Min(rows.Count(),MAX_ROWS_DISPLAYED)) ? rowCount : Math.Min(rows.Count(),MAX_ROWS_DISPLAYED);
            var subsetRows = rows.Take(maxRows);
            IList<int> fieldLength = GetFieldsLength(subsetRows);

            //information about #Rows and #Cols
            var sbInfo = new StringBuilder();
            sbInfo.AppendFormat("ResultSet with {0} row", rowCount);
            if (rowCount > 1)
                sbInfo.Append('s');
            output.Add(sbInfo.ToString());

            
            //separator
            output.Add(GetFirstIndentation() + GetSeparator(fieldLength));

            //header
            var sbHeader = new StringBuilder();
            var sbColumnName = new StringBuilder();
            sbHeader.Append(' ', 4);
            for (int i = 0; i < fieldLength.Count; i++)
            {
                var displayHeader = LineFormatter.BuildHeader(rows.ElementAt(0).Table, i);
                if (displayHeader!=null)
                {
                    sbHeader.AppendFormat("| {0} ", displayHeader.GetText(fieldLength[i]));
                    sbColumnName.AppendFormat("| {0} ", displayHeader.GetColumnName(fieldLength[i]));
                }
            }
            sbHeader.Append("|");
            output.Add(sbHeader.ToString());
            
            //separator
            output.Add(GetFirstIndentation() + GetSeparator(fieldLength));

            //TestCases
            for (int i = 0; i < maxRows; i++)
            {
                var sbRow = new System.Text.StringBuilder();

                sbRow.Append(GetFirstIndentation());
                for (int j = 0; j < subsetRows.ElementAt(i).ItemArray.Count(); j++)
                {
                    ICellFormatter display = null;
                    if (compare)
                        display = LineFormatter.Build(rows.ElementAt(i), j);
                    else
                        display = LineFormatter.BuildValue(rows.ElementAt(i), j);
                    sbRow.AppendFormat("| {0} ", display.GetText(fieldLength[j]));
                }
                sbRow.Append("|");
                output.Add(sbRow.ToString());
            }

            //footer (separator)
            output.Add(GetFirstIndentation() + GetSeparator(fieldLength));


            //If needed display # skipped rows
            if (rowCount > MAX_ROWS_DISPLAYED)
            {
                var sbSkip = new System.Text.StringBuilder();
                sbSkip.AppendFormat(GetFirstIndentation());
                sbSkip.Append(new string('.', 3));
                sbSkip.Append(new string(' ', 3));
                sbSkip.AppendFormat("{0} (of {1}) rows skipped for display purpose", rowCount - MAX_ROWS_DISPLAYED, rowCount);
                sbSkip.Append(new string(' ', 3));
                sbSkip.Append(new string('.', 3));
                output.Add(sbSkip.ToString());
            }                    

            output.Add("");

            return output;
        }

    }
}
