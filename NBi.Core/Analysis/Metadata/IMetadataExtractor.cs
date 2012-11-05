using System;
using System.Collections.Generic;
using Microsoft.AnalysisServices.AdomdClient;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Metadata
{
    public interface IMetadataExtractor
    {
        CubeMetadata GetFullMetadata();

        IEnumerable<IField> GetPartialMetadata(MetadataDiscoveryCommand command);
    }
}