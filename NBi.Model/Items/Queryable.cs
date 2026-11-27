using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public abstract class Queryable : Executable
{
    public int Timeout { get; set; }

    public List<QueryParameter> Parameters { get; set; } = [];

    public List<QueryTemplateVariable> Variables { get; set; } = [];

    public virtual List<QueryParameter> GetParameters()
    {
        var list = Parameters;
        foreach (var param in Default!.Parameters)
            if (!Parameters.Exists(p => p.Name == param.Name))
                list.Add(param);

        var i = 0;
        while( i < list.Count)
        {
            if (list[i].IsRemoved)
                list.RemoveAt(i);
            else
                i++;
        }

        return list;
    }

    public virtual List<QueryTemplateVariable> GetTemplateVariables()
    {
        var list = Variables;
        foreach (var variable in Default!.Variables)
            if (!Variables.Exists(p => p.Name == variable.Name))
                list.Add(variable);

        return list;
    }
}
