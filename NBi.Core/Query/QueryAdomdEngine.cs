using System;
using System.Data;
using System.Diagnostics;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
	/// <summary>
	/// Engine wrapping the Microsoft.AnalysisServices.AdomdClient namespace for execution of NBi tests
	/// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
	/// </summary>
	internal class QueryAdomdEngine: IQueryEnginable, IQueryExecutor, IQueryParser, IQueryPerformance, IQueryFormat
	{
		/// <summary>
		/// The query to execute
		/// </summary>
		protected readonly AdomdCommand command;


		protected internal QueryAdomdEngine(AdomdCommand command)
		{
			this.command = command;
		}

		/// <summary>
		/// Method exposed by the interface IQueryPerformance to execute a test of performance
		/// </summary>
		/// <returns></returns>
		public PerformanceResult CheckPerformance()
		{
			return CheckPerformance(0);
		}

		public PerformanceResult CheckPerformance(int timeout)
		{
			bool isTimeout = false;
			DateTime tsStart, tsStop = DateTime.Now;

			if (command.Connection.State == ConnectionState.Closed)
				command.Connection.Open();

			tsStart = DateTime.Now;
			try
			{
				command.ExecuteNonQuery();
				tsStop = DateTime.Now;
			}
			catch (AdomdException e)
			{
				if (!e.Message.StartsWith("Timeout expired."))
					throw;
				isTimeout = true;
			}

			if (command.Connection.State == ConnectionState.Open)
				command.Connection.Close();

			if (isTimeout)
				return PerformanceResult.Timeout(timeout);
			else
				return new PerformanceResult(tsStop.Subtract(tsStart));
		}

		/// <summary>
		/// Method exposed by the interface IQueryExecutor to execute a test of execution and get the result of the query executed
		/// </summary>
		/// <returns>The result of  execution of the query</returns>
		public virtual DataSet Execute()
		{
			float i;
			return Execute(out i);
		}

		/// <summary>
		/// Method exposed by the interface IQueryExecutor to execute a test of execution and get the result of the query executed and also the time needed to retrieve this result
		/// </summary>
		/// <returns>The result of  execution of the query</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public virtual DataSet Execute(out float elapsedSec)
		{
			// Open the connection
			using (var connection = new AdomdConnection())
			{
				var connectionString = command.Connection.ConnectionString;
				try
					{ connection.ConnectionString = connectionString; }
				catch (ArgumentException ex)
				{ throw new ConnectionException(ex, connectionString); }
				//TODO
				//try
				//    {connection.Open();}
				//catch (AdomdException ex)
				//    {throw new ConnectionException(ex);}

				Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, command.CommandText);
				// capture time before execution
				DateTime timeBefore = DateTime.Now;
				command.Connection = connection;
				var adapter = new AdomdDataAdapter(command);
				var ds = new DataSet();
				
				adapter.SelectCommand.CommandTimeout = 0;
				try
				{
					adapter.Fill(ds);
				}
				catch (AdomdConnectionException ex)
				{
					throw new ConnectionException(ex, connectionString);
				}
				catch (AdomdErrorResponseException ex)
				{
					throw new ConnectionException(ex, connectionString);
				}

				// capture time after execution
				DateTime timeAfter = DateTime.Now;

				// setting query runtime
				elapsedSec = (float) timeAfter.Subtract(timeBefore).TotalSeconds;
				Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Time needed to execute query: {0}", timeAfter.Subtract(timeBefore).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

				return ds;
			}
		}

		/// <summary>
		/// Method exposed by the interface IQueryExecutorFormat to execute a test of execution and get the result of the query executed and also the time needed to retrieve this result
		/// </summary>
		/// <returns>The result of  execution of the query</returns>
		public virtual CellSet ExecuteCellSet()
		{
			float i;
			return ExecuteCellSet(out i);
		}

		/// <summary>
		/// Method exposed by the interface IQueryExecutorFormat to execute a test of execution and get the result of the query executed and also the time needed to retrieve this result
		/// </summary>
		/// <returns>The result of  execution of the query</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public virtual CellSet ExecuteCellSet(out float elapsedSec)
		{
			// Open the connection
			using (var connection = new AdomdConnection())
			{
				var connectionString = command.Connection.ConnectionString;
				try
				{ connection.ConnectionString = connectionString; }
				catch (ArgumentException ex)
				{ throw new ConnectionException(ex, connectionString); }
				//TODO
				//try
				//    {connection.Open();}
				//catch (AdomdException ex)
				//    {throw new ConnectionException(ex);}

				Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, command.CommandText);
				// capture time before execution
				DateTime timeBefore = DateTime.Now;
				CellSet cellSet = null;

				try
				{
					if (command.Connection.State != ConnectionState.Open)
						command.Connection.Open();
					cellSet = command.ExecuteCellSet();
				}
				catch (AdomdConnectionException ex)
				{
					throw new ConnectionException(ex, connectionString);
				}
				catch (AdomdErrorResponseException ex)
				{
					throw new ConnectionException(ex, connectionString);
				}
				finally
				{
					if (command.Connection.State == ConnectionState.Open)
						command.Connection.Close();
				}

				// capture time after execution
				DateTime timeAfter = DateTime.Now;

				// setting query runtime
				elapsedSec = (float)timeAfter.Subtract(timeBefore).TotalSeconds;
				Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Time needed to execute query: {0}", timeAfter.Subtract(timeBefore).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

				return cellSet;
			}
		}

		public FormattedResults GetFormats()
		{
			var cellSet = ExecuteCellSet();
			var formattedResults = new FormattedResults();

			foreach (var cell in cellSet.Cells)
			{
				formattedResults.Add(cell.FormattedValue);
			}
			return formattedResults;
		}

		/// <summary>
		/// Method exposed by the interface IQueryParser to execute a test of syntax on a MDX or SQL statement
		/// </summary>
		/// <remarks>This method makes usage the scommand behaviour named 'SchemaOnly' to not effectively execute the query but check the validity of this query</remarks>
		/// <returns>The result of parsing test</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public virtual ParserResult Parse()
		{
			ParserResult res = null;

			using (var connection = new AdomdConnection())
			{
				var connectionString = command.Connection.ConnectionString;
				try
				{
					connection.ConnectionString = connectionString;
					connection.Open();
				}
				catch (ArgumentException ex)
				{ throw new ConnectionException(ex, connectionString); }
				
				using (AdomdCommand cmdIn = new AdomdCommand(command.CommandText, connection))
				{
					foreach (AdomdParameter param in command.Parameters)
					{
						var p = param.Clone();
						cmdIn.Parameters.Add(p);
					}
					try
					{
						cmdIn.ExecuteReader(CommandBehavior.SchemaOnly);
						res = ParserResult.NoParsingError();
					}
					catch (AdomdException ex)
					{
						res = new ParserResult(ex.Message.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
					}

				}

				if (connection.State != System.Data.ConnectionState.Closed)
					connection.Close();
			}

			return res;
		}

		/// <summary>
		/// Method exposed by the interface IQueryPerformance of engine but useless in the case of an SSAS cube
		/// </summary>
		public void CleanCache()
		{
			throw new NotImplementedException("Hé man what's the goal to clean the cache for an MDX query?");
		}

		
	}
}
