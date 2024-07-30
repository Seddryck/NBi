using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies
{
    public interface IOutputStrategy
    {
        object? Execute(bool isOriginal, bool isDuplicated, int times, int index);
        bool IsApplicable(bool isOriginal);
    }
}
