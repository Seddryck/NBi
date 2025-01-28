using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies;

class IndexOutputStrategy : IOutputStrategy
{
    public object Execute(bool isOriginal, bool isDuplicated, int times, int index)
        => index;
    public bool IsApplicable(bool isOriginal) => !isOriginal;
}
