using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NBi.Core.DataType;

public interface IDataTypeDiscoveryFactory
{
    IDataTypeDiscoveryCommand Instantiate(Target target, IEnumerable<CaptionFilter> filters);
}
