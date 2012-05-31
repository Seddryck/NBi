using System;
using System.Collections.Generic;
using Microsoft.AnalysisServices.AdomdClient;
using System.Linq;

namespace NBi.Core.Analysis.Metadata
{
    public interface IMetadataExtractor
    {
        CubeMetadata GetFullMetadata();

        IEnumerable<IElement> GetPartialMetadata(string path, string perspective);
    }
}