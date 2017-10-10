using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class DuplicatedRowsResult
    {
        public bool HasNoDuplicate { get; private set; }
        public int RowCount { get; private set; }
        public IEnumerable<DuplicatedRowsResultOccurence> Values { get; private set; }
        public IEnumerable<DataRow> Rows { get; private set; }


        public DuplicatedRowsResult(int count, IEnumerable<KeyValuePair<KeyCollection, int>> values)
        {
            RowCount = count;
            Values = values.Select(x => new DuplicatedRowsResultOccurence(x.Key, x.Value)).OrderByDescending(x => x.OccurenceCount);
            HasNoDuplicate = values.Count() == 0;

            if (!HasNoDuplicate)
            {
                var dt = new DataTable();
                dt.Columns.Add(new DataColumn("Occurence", typeof(int)));
                int i = 0;
                foreach (var key in Values.ElementAt(0).Keys.Members)
                {
                    dt.Columns.Add(new DataColumn($"#{i}"));
                    i++;
                }
                foreach (var value in Values)
                {
                    var dr = dt.NewRow();
                    dr[0] = value.OccurenceCount;
                    i = 0;
                    foreach (var key in value.Keys.Members)
                    {
                        dr.SetField($"#{i}", key);
                        i++;
                    }
                    dt.Rows.Add(dr);
                }
                Rows = dt.Rows.Cast<DataRow>();
            }
        }
    }

    public class DuplicatedRowsResultOccurence
    {
        public KeyCollection Keys { get; private set; }
        public int OccurenceCount { get; private set; }

        public DuplicatedRowsResultOccurence(KeyCollection keys, int occurenceCount)
        {
            OccurenceCount = occurenceCount;
            Keys = keys;
        }
    }




}
