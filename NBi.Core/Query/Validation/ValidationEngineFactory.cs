using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;

namespace NBi.Core.Query.Validation
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ValidationEngineFactory : EngineFactory<IValidationEngine>
    {
        public ValidationEngineFactory()
        {
            RegisterEngines([
                typeof(AdomdValidationEngine),
                typeof(OdbcValidationEngine),
                typeof(OleDbValidationEngine),
                typeof(SqlValidationEngine) ]
            );
        }
    }
}