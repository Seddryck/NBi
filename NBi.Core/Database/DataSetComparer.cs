using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NBi.Core.Database
{
    public class DataSetComparer : IDataSetComparer
    {
        protected string _expectedConnectionString;
        protected string _actualConnectionString;
        protected string _expectedSql;

        public DataSetComparer(string expectedConnectionString, string actualConnectionString)
            : this(expectedConnectionString, null, actualConnectionString)
        {
        }

        public DataSetComparer(string expectedConnectionString, string expectedSql, string actualConnectionString)
        {
            _expectedConnectionString = expectedConnectionString;
            _actualConnectionString = actualConnectionString;
            _expectedSql = expectedSql;
        }

        protected DataSet FillDataSet(string connectionString, string sql)
        {
            DataSet dataset = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(sql, conn);

                    dataset = new DataSet();
                    adapter.Fill(dataset);
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }
            return dataset;
        }

        public Result Validate(string actualSql)
        {
            var expectedDs = FillDataSet(_expectedConnectionString,String.IsNullOrEmpty(_expectedSql)?actualSql:_expectedSql);
            var actualDs = FillDataSet(_actualConnectionString, actualSql);

            var resStructure = ValidateStructure(expectedDs, actualDs);
            if (resStructure.Value == Result.ValueType.Success)
                return ValidateContent(expectedDs, actualDs);
            else
                return resStructure;
        }

        #region Structure

        protected internal Result ValidateStructure(DataSet expectedDs, DataSet actualDs)
        {
            Result res = null;

            if (expectedDs.GetXmlSchema() == actualDs.GetXmlSchema())
                res = Result.Success();
            else
            {
                res = Result.Failed(DifferenceDataSetSchema(expectedDs.GetXmlSchema(), actualDs.GetXmlSchema()));
            }

            return res;
        }

        public Result ValidateStructure(string actualSql)
        {
            var expectedDs = FillDataSet(_expectedConnectionString, String.IsNullOrEmpty(_expectedSql) ? actualSql : _expectedSql);
            var actualDs = FillDataSet(_actualConnectionString, actualSql);

            return ValidateStructure(expectedDs, actualDs);
        }

        
        protected string DifferenceDataSetSchema(string expectedSchema, string actualSchema)
        {
            var expectedLines = expectedSchema.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            var actualLines = actualSchema.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            var i=0;
            foreach (var l in expectedLines)
            {
                var el=l.Replace("\t","").Trim();
                var al=actualLines[i].Replace("\t","").Trim();

                if (el!=al)
                    try
                    {
                        return InterpretDifference(el, al);
                    }
                    catch (Exception)
                    {
                        return string.Format("Expected: {0}\r\n But Was: {1}", el, al);
                    }
                i++;
            }

            throw new Exception("DataSet schemas are quivalent");
        }



        protected string InterpretDifference(string expectedLine, string actualLine)
        {
            var et = expectedLine.Replace("xs:","").Replace("\"", "").Split(new char[] { ' ' , '=' }, StringSplitOptions.RemoveEmptyEntries);
            var expectedToken = new List<string>(et);

            var at = actualLine.Replace("xs:", "").Replace("\"", "").Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
            var actualToken = new List<string>(at);

            for (int i = 0; i < expectedToken.Count; i+=2)
			{
                if (expectedToken[i] != actualToken[i])
                {
                    if (expectedToken[i-1]=="name")
                        return String.Format("Object named \"{0}\" was missing or not correctly positionned in actual result, \"{1}\" was found at its place.", expectedToken[i], actualToken[i]);
                    if (expectedToken[i - 1] == "type")
                        return String.Format("Object named \"{0}\" was defined as \"{1}\" in expected result and \"{2}\" in actual result.", expectedToken[i - 2], expectedToken[i], actualToken[i]);
                }
			}

            throw new Exception("Lines not comparable");
        }

        #endregion

        #region Content

        public Result ValidateContent(string actualSql)
        {
            var expectedDs = FillDataSet(_expectedConnectionString, String.IsNullOrEmpty(_expectedSql) ? actualSql : _expectedSql);
            var actualDs = FillDataSet(_actualConnectionString, actualSql);

            return ValidateContent(expectedDs, actualDs);
        }

        protected internal Result ValidateContent(DataSet expectedDs, DataSet actualDs)
        {

            DataTable expectedTable = expectedDs.Tables[0];
            DataTable actualTable = actualDs.Tables[0];

            if(expectedTable.Rows.Count != actualTable.Rows.Count)
                return Result.Failed(String.Format("Different number of rows, {0} expected and was {1}", expectedTable.Rows.Count, actualTable.Rows.Count));
            
            // iterate over all rows
            for (int row = 0; row < expectedTable.Rows.Count; row++)
            {
                DataRow expectedRow = expectedTable.Rows[row];
                DataRow currentRow = actualTable.Rows[row];

                // iterate over all columns
                for (int col = 0; col < actualTable.Columns.Count; col++)
                {
                    Object currentValue = currentRow.ItemArray[col];
                    Object expectedValue = expectedRow.ItemArray[col];

                    double currentValueAsDouble;
                    double expectedValueAsDouble;

                    // try to parse values as double
                    if (Double.TryParse(currentValue.ToString(), out currentValueAsDouble)
                    && Double.TryParse(expectedValue.ToString(), out expectedValueAsDouble))
                    {
                        if(currentValueAsDouble!=expectedValueAsDouble)
                        {
                            return Result.Failed(String.Format("At row {0}, numeric value of column \"{1}\" are different in both datasets, expected was \"{2}\" and actual was \"{3}\"."
                                                                , (row+1).ToString()
                                                                , actualTable.Columns[col].ColumnName
                                                                , expectedValueAsDouble.ToString()
                                                                , currentValueAsDouble.ToString()));
                        }
                    }
                    else
                    {
                        if (currentValue.ToString() != expectedValue.ToString())
                        {
                            return Result.Failed(String.Format("At row {0}, value of column \"{1}\" are different in both datasets, expected was \"{2}\" and actual was \"{3}\"."
                                                               , (row + 1).ToString()
                                                               , actualTable.Columns[col].ColumnName
                                                               , expectedValue
                                                               , currentValue));
                        }
                    }
                }
            }
            
            return Result.Success();
        }            
        
        #endregion
    }
}