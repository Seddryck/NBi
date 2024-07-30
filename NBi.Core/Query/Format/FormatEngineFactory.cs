using System;

namespace NBi.Core.Query.Format
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class FormatEngineFactory : EngineFactory<IFormatEngine>
    {
        public FormatEngineFactory()
        {
            RegisterEngines([
                typeof(AdomdFormatEngine),
                typeof(OdbcFormatEngine),
                typeof(OleDbFormatEngine),
                typeof(SqlFormatEngine) ]
            );
        }
    }
}
