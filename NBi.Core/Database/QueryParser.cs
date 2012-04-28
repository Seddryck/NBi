using System.Data;
using System.Data.SqlClient;

namespace NBi.Core.Database
{
    public class QueryParser : IQueryParser
    {

        public QueryParser()
        {
        }

        public Result Validate(IDbCommand cmd)
        {
            Result res=null;
            
            using(SqlConnection conn = new SqlConnection(cmd.Connection.ConnectionString))
            {
                var fullSql = string.Format(@"SET FMTONLY ON {0} SET FMTONLY OFF", cmd.CommandText);
                
                conn.Open();

                using (SqlCommand cmdIn = new SqlCommand(fullSql, conn))
                {
                    try 
	                {
                        cmdIn.ExecuteNonQuery();
                        res = Result.Success();
	                }
	                catch (SqlException ex)
	                {
                        res = Result.Failed(ex.Message.Split(new string[] {"\r\n"},System.StringSplitOptions.RemoveEmptyEntries));
	                }
                    
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                conn.Close();
            }

            return res;
        }
    }
}
