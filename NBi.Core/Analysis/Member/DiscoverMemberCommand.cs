using System;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Member
{
    public class DiscoverMemberCommand
    {
        public string ConnectionString { get; set; }
        public string Perspective { get; set; }
        public string Path { get; set; }

        public DiscoverMemberCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        
    }
}
