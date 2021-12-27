using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class ResultUniqueRows
    {
        public bool AreUnique { get; private set; }
        public int RowCount { get; private set; }
        public IEnumerable<ResultOccurenceUniqueRows> Values { get; private set; }
        public IEnumerable<IResultRow> Rows { get; private set; }


        public ResultUniqueRows(int count, IEnumerable<KeyValuePair<KeyCollection, int>> values)
        {
            RowCount = count;
            Values = values.Select(x => new ResultOccurenceUniqueRows(x.Key, x.Value)).OrderByDescending(x => x.OccurenceCount);
            AreUnique = values.Count() == 0;

            if (!AreUnique)
            {
                var dt = new DataTableResultSet();
                dt.AddColumn("Occurence", typeof(int));

                foreach (var key in Values.ElementAt(0).Keys.Members)
                    dt.AddColumn(Guid.NewGuid().ToString());

                foreach (var value in Values)
                {
                    var items = new List<object>(value.Keys.Members.Count() + 1);
                    items.Add(value.OccurenceCount);
                    items.AddRange(value.Keys.Members);
                    dt.AddRow(items.ToArray());
                }
                Rows = dt.Rows;
            }
        }
    }






}
