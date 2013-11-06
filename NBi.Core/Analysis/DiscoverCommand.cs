namespace NBi.Core.Analysis
{
    public class DiscoverCommand
    {
        public string ConnectionString { get; protected set; }
        public string Perspective { get; set; }
        public string Path { get; set; }
        public string MeasureGroup { get; set; }
        public DiscoverTarget Target { get; protected set; }
        public string Function { get; set; }

        public DiscoverCommand(DiscoverTarget target, string connectionString)
        {
            Target = target;
            ConnectionString = connectionString;
        }

        public DiscoverCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
    }
}
