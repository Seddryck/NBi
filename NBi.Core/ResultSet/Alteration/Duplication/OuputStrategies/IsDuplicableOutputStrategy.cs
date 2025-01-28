using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies;

class IsDuplicableOutputStrategy : IOutputStrategy
{
    public object Execute(bool isOriginal, bool isDuplicable, int times, int index) 
        => isDuplicable;
    public bool IsApplicable(bool isOriginal) => true;
}
