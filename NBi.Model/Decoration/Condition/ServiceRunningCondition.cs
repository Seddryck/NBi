using System;
using System.Linq;

namespace NBi.Model.Decoration.Condition;

public class ServiceRunningCondition : DecorationCondition
{
    public string ServiceName { get; set; }

    public string TimeOut { get; set; }

    public ServiceRunningCondition()
    {
        TimeOut = "5000";
    }
}
