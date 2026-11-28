using System;
using System.Linq;

namespace NBi.Model.Decoration.Command;

public abstract class ServiceAbstract : DecorationCommand
{
    public string ServiceName { get; set; }

    public string TimeOut { get; set; }

    public ServiceAbstract()
    {
        TimeOut = "5000";
    }
}
