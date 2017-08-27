using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer
{
    public interface IRowsAnalyzer
    {
        List<CompareHelper> Retrieve(Dictionary<KeyCollection, CompareHelper> x, Dictionary<KeyCollection, CompareHelper> y);
    }
}
