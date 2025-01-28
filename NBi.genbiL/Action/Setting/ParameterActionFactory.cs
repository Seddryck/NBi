using System;
using System.Linq;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting;

public class ParameterActionFactory 
{
    public ParameterActionFactory()
    {
    }

    public ISettingAction Build(string name, bool value)
    {
        if (name.ToLower()=="parallelize-queries")
            return new ParallelizeQueriesAction(value);
        throw new ArgumentOutOfRangeException();
    }

}
