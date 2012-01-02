using System.Data.SqlClient;

namespace NBi.Core.Database
{
    public class QueryParser : IQueryParser
    {
        protected string _connectionString;

        public QueryParser(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Result ValidateFormat(string sqlQuery)
        {
            Result res=null;
            
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                var fullSql = string.Format(@"SET FMTONLY ON {0} SET FMTONLY OFF", sqlQuery);
                
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(fullSql, conn))
                {
                    try 
	                {	        
		                cmd.ExecuteNonQuery();
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
