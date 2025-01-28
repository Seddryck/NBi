using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies;

class ValueOutputStrategy : IOutputStrategy
{
    public object Value { get; }

    public ValueOutputStrategy(object value)
        => Value = value;

    public object Execute(bool isOriginal, bool isDuplicable, int times, int index)
        => Value;

    public bool IsApplicable(bool isOriginal) => !isOriginal;

}
