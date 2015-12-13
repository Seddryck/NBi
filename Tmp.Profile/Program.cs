using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmp.Profile
{
    class Program
    {
        static void Main(string[] args)
        {
            Compare_DifferentLargArrays_ReturnQuicklyDifferent(300000);
        }

        public static void Compare_DifferentLargArrays_ReturnQuicklyDifferent(int count)
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(RandomLargeArrayString(count, 0), RandomLargeArrayDouble(count));
            var actual = BuildDataTable(RandomLargeArrayString(count, Convert.ToInt32(count * 0.8)), RandomLargeArrayDouble(count));

            Console.WriteLine("Starting comparaison for {0} rows", count);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            //Call the method to test
            var res = comparer.Compare(reference, actual);
            stopWatch.Stop();
            Console.WriteLine("Compaired in {0} milliseconds", stopWatch.Elapsed.TotalMilliseconds);
            //Assertion
        }

        protected static ResultSetComparisonSettings BuildSettingsKeyValue()
        {
            return BuildSettingsKeyValue(0, ColumnType.Text);
        }

        protected static ResultSetComparisonSettings BuildSettingsKeyValue(decimal tolerance, ColumnType keyType)
        {
            var columnsDef = new List<IColumnDefinition>();
            columnsDef.Add(
                    new Column() { Index = 0, Role = ColumnRole.Key, Type = keyType }
                    );
            columnsDef.Add(
                    new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.Numeric, Tolerance = tolerance.ToString() }
                    );

            return new ResultSetComparisonSettings(
                ResultSetComparisonSettings.KeysChoice.First,
                ResultSetComparisonSettings.ValuesChoice.Last,
                columnsDef
                );
        }

        protected static DataTable BuildDataTable(string[] keys, double[] values)
        {
            return BuildDataTable(keys, values, null);
        }

        protected static DataTable BuildDataTable(string[] keys, double[] values, string[] useless)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey", typeof(string));
            var valueCol = dt.Columns.Add("myValue", typeof(double));
            var uselessCol = useless != null ? dt.Columns.Add("myUseless", typeof(string)) : null;

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<string>(keyCol, keys[i]);
                dr.SetField<double>(valueCol, values[i]);
                if (uselessCol != null)
                    dr.SetField<string>(uselessCol, useless[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected static string[] RandomLargeArrayString(int count, int start)
        {
            var array = new List<string>();
            for (int i = start; i < start + count; i++)
                array.Add(i.ToString());
            return array.ToArray();
        }

        private static Random random = new Random();

        protected static double[] RandomLargeArrayDouble(int count)
        {
            var array = new List<double>();
            for (int i = 0; i < count; i++)
                array.Add(random.NextDouble() * 100000);
            return array.ToArray();
        }
    }
}
