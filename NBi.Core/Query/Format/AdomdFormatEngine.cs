using System;
using System.Data;
using System.Diagnostics;
using Microsoft.AnalysisServices.AdomdClient;
using System.IO;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using NBi.Core.Query.Execution;

namespace NBi.Core.Query.Format
{
    /// <summary>
    /// Engine wrapping the Microsoft.AnalysisServices.AdomdClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>ExecutionEngineFactory</see></remarks>
    /// </summary>
    internal class AdomdFormatEngine : AdomdExecutionEngine, IFormatEngine
    {
        protected internal AdomdFormatEngine(AdomdCommand command)
            : base(command)
        { }

        public IEnumerable<string> Parse(CellSet cellSet)
        {
            var formattedResults = new List<string>();

            foreach (var cell in cellSet.Cells)
                formattedResults.Add(cell.FormattedValue);

            return formattedResults;
        }

        public IEnumerable<string> ExecuteFormat()
        {
            using (var connection = NewConnection())
            {
                OpenConnection(connection);
                InitializeCommand(command, CommandTimeout, command.Parameters, connection);
                StartWatch();
                var cs = OnExecuteCellSet(command);
                StopWatch();
                return Parse(cs);
            }
        }

        private CellSet OnExecuteCellSet(IDbCommand command)
        {
            CellSet cellSet = null;
            try
            { cellSet = ((AdomdCommand)command).ExecuteCellSet(); }
            catch (Exception ex)
            { HandleException(ex, command); }
            return cellSet;
        }
    }
}
