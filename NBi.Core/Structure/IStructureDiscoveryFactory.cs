using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure;

public interface IStructureDiscoveryFactory
{
    StructureDiscoveryCommand Instantiate(Target target, TargetType type, IEnumerable<IFilter> filters);
}
