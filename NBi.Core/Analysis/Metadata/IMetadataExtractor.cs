using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata
{
    public interface IMetadataExtractor
    {
        CubeMetadata GetFullMetadata();

        IEnumerable<IField> GetPartialMetadata(MetadataDiscoveryRequest command);
    }
}