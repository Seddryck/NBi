using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public class ResultSetRow
    {
        public IList<ResultSetField> Fields {private get; set; }
        protected ResultSetComparaisonSettings _settings;

        public ResultSetRow(string[] fields, ResultSetComparaisonSettings settings)
        {
            _settings = settings;
            Fields = new List<ResultSetField>();
            
            int i = 0;
            foreach (string field in fields)
            {
                if (settings.KeyColumnIndexes.Contains(i))
                    Fields.Add(new ResultSetKey(field));
                else if (settings.ValueColumnIndexes.Contains(i))
                    Fields.Add(new ResultSetValue(field));
                i++;
            }
        }

        public ResultSetCompareResult EquivalentTo(ResultSetRow other)
        {
            return EquivalentTo(other, _settings);
        }

        private ResultSetCompareResult EquivalentTo(ResultSetRow other, ResultSetComparaisonSettings settings)
        {
            if (this.Fields.Count != other.Fields.Count)
                return ResultSetCompareResult.StructureNotComparable;
            
            int i=0;
            var res = new ResultSetCompareResult();
            IEnumerator<ResultSetField> iterX = this.Fields.GetEnumerator();
            IEnumerator<ResultSetField> iterY = other.Fields.GetEnumerator();

            while (iterX.MoveNext() && iterY.MoveNext())
            {
                if (iterX.Current is ResultSetKey)
                    if (!((ResultSetKey)iterX.Current).EqualTo((ResultSetKey)iterY.Current))
                    {
                        res.Value = false;
                        //res.Difference.Add(ResultSetDifferenceType.Key, i);
                        return res;
                    }
                        

                if (iterX.Current is ResultSetValue)
                {
                    var tol = settings.Tolerances(i++);
                    if (!((ResultSetValue)iterX.Current).EqualTo((ResultSetValue)iterY.Current, tol))
                    {
                        res.Value = false;
                        //res.Difference.Add(Diff.Value, i);
                        return res;
                    }
                }
            }
            return res;
        }
    }
}
