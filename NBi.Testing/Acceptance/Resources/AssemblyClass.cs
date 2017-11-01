using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NBi.Testing.Acceptance.Resources
{
    /// <summary>
    /// This class is only used for acceptance testing purpose
    /// </summary>
    class AssemblyClass
    {
        public AssemblyClass()
        {

        }

        /// <summary>
        /// Method returning a valid MDX query
        /// </summary>
        /// <param name="calendarYear">A year between 2005 and 2008 preceded by 'CY'</param>
        /// <returns>Valid MDX query</returns>
        public string GetSelectMdx(string calendarYear)
        {
            var mdx = $"SELECT [Measures].[Reseller Order Count] ON 0, [Date].[Calendar Year].[{calendarYear}] ON 1 FROM [Adventure Works]";
            return mdx;
        }

        public enum Measure
        {
            FirstMeasure = 0,
            OrderCount = 1
        }

        /// <summary>
        /// Method returning a valid MDX query if the param Measure is equal to OrderCount else an invalid MDX statement
        /// </summary>
        /// <param name="year">a calendar year without the 'CY'</param>
        /// <param name="measure"></param>
        /// <returns></returns>
        public string GetSelectMdxWithTwoParams(int year, Measure measure)
        {
            if (measure == Measure.OrderCount)
                return $"SELECT [Measures].[Reseller Order Count] ON 0, [Date].[Calendar Year].[CY {year}] ON 1 FROM [Adventure Works]";
            else
                return "Incorrect Query";
        }

        public string GetTextSelectSql(string prefix)
        {
            return $@"select '{prefix} 2005', 366
                union all select '{prefix} 2006', 1015
                union all select '{prefix} 2007', 1521
                union all select '{prefix} 2008', 894";
        }

        public IDbCommand GetCommandSelectSql(string prefix)
        {
            var cmd = new SqlCommand();

            cmd.CommandText = $@"select '{prefix} 2005', 366
                union all select '{prefix} 2006', 1015
                union all select '{prefix} 2007', 1521
                union all select '{prefix} 2008', 894";

            return cmd;
        }

        public DataSet GetDataSetSelectSql(string prefix, string connectionString)
        {
            using (var da = new SqlDataAdapter($@"select '{prefix} 2005', 366
                union all select '{prefix} 2006', 1015
                union all select '{prefix} 2007', 1521
                union all select '{prefix} 2008', 894", connectionString))
            {
                var ds = new DataSet();
                da.Fill(ds);

                return ds;
            }
        }

        public DataTable GetDataTableSelectSql(string prefix, string connectionString)
        {
            return GetDataSetSelectSql(prefix, connectionString).Tables[0];
        }

    }
}

