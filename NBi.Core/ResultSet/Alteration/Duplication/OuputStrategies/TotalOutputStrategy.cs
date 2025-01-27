using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies;

class TotalOutputStrategy : IOutputStrategy
{
    public object Execute(bool isOriginal, bool isDuplicated, int times, int index)
        => isDuplicated ? times : 1;
    public bool IsApplicable(bool isOriginal) => !isOriginal;
}