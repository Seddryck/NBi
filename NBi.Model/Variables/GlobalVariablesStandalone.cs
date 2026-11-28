using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Variables;

public class GlobalVariablesStandalone
{
    public List<GlobalVariable> Variables { get; set; }

    public GlobalVariablesStandalone()
    {
        Variables = new List<GlobalVariable>();
    }
}
