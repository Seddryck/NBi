using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataQuery
    {
        public string ConnectionString { get; set; }
        public string Path { get; set; }
        public string Perspective { get; set; }
    }
}
