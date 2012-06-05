namespace NBi.Core.Analysis
{
    public class DiscoverCommand
    {
        public string ConnectionString { get; set; }
        public string Perspective { get; set; }
        public string Path { get; set; }

        public DiscoverCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool IsMeasureBased
        {
            get
            {
                return Path.StartsWith("[Measures]");
            }
        }
        
    }
}
