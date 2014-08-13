using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Process
{
    public interface ICubeProcess
    {
        string Cube { get; set; }
        IEnumerable<IDimensionProcess> Dimensions { get; }
        IEnumerable<IMeasureGroupProcess> MeasureGroups { get; }
        IEnumerable<IPartitionProcess> Partitions { get; }
        string ConnectionString { get; }
        string Database { get; }

    }
}
