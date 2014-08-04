using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataAdomdExtractor : IMetadataExtractor
    {
        public event ProgressStatusHandler ProgressStatusChanged;
        
        public string ConnectionString { get; private set; }
        protected CubeMetadata Metadata;//TODO REmove 

        public MetadataAdomdExtractor(string connectionString) 
        {
            ConnectionString = connectionString;
            Metadata = new CubeMetadata();
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
  
        public CubeMetadata GetFullMetadata()
        {
            var cube = new CubeMetadata();
            
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting investigation ..."));

            using (var cmd = new PerspectiveDiscoveryCommand(ConnectionString))
                cube.Import(cmd.Discover(null));

            using (var cmd = new DimensionDiscoveryCommand(ConnectionString))
                cube.Import(cmd.Discover(null));

            using (var cmd = new HierarchyDiscoveryCommand(ConnectionString))
                cube.Import(cmd.Discover(null));

            using (var cmd = new LevelDiscoveryCommand(ConnectionString))
                cube.Import(cmd.Discover(null));

            using (var cmd = new PropertyDiscoveryCommand(ConnectionString))
                cube.Import(cmd.Discover(null));

            using (var cmd = new MeasureGroupDiscoveryCommand(ConnectionString))
            {
                var rows = cmd.Discover(null);
                cube.Import(rows);
                //cube.Link(rows);
            }
            using (var cmd = new MeasureDiscoveryCommand(ConnectionString))
                cube.Import(cmd.Discover(null));
          
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Cube investigated"));

            return cube;
        }

        /// <summary>
        /// Retrieve partial metadata from an Olap cube. The zone of the cube investigated is delimited by the MetadataDiscoveryCommand parameter
        /// </summary>
        /// <param name="command">limit the scope of the metadata's investigation</param>
        /// <returns>An enumeration of fields</returns>
        //public IEnumerable<IField> GetPartialMetadata(MetadataDiscoveryRequest request)
        //{
        //    var factory= new AdomdDiscoveryCommandFactory(ConnectionString);
        //     cmd.Execute(request.GetAllFilters());
        //}
    }
}
