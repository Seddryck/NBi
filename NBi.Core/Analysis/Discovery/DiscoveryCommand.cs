using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public abstract class DiscoveryCommand
    {
        public string ConnectionString { get; private set; }
        public DiscoveryTarget Target { get; protected set; }
        public Filter Filter { get; private set; }
        public Depth Depth { get; private set; }

        public DiscoveryCommand(string connectionString)
        {
            ConnectionString = connectionString;
            Filter = Filter.Empty;
            Depth = new Depth();
        }

        protected internal abstract void Initialize();
    }
}
