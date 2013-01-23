using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    public abstract class AdomdDiscoveryCommand : IDisposable
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public string ConnectionString { get; private set; }

        public AdomdDiscoveryCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        protected AdomdCommand CreateCommand()
        {
            var conn = new AdomdConnection();
            conn.ConnectionString = ConnectionString;
            try
            {
                conn.Open();
            }
            catch (AdomdConnectionException ex)
            {

                throw new ConnectionException(ex, conn.ConnectionString);
            }


            var cmd = new AdomdCommand();
            cmd.Connection = conn;
            return cmd;
        }

        protected AdomdDataReader ExecuteReader(AdomdCommand cmd)
        {
            AdomdDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
        }


        protected void Inform(string text)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(text));
        }

        public IEnumerable<IFilter> Filters {get; set;}
        public abstract IEnumerable<IField> Execute();

        protected abstract string Build(CaptionFilter filter);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }
            disposed = true;
        }
    }
}
