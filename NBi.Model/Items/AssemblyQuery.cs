using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class AssemblyQuery : Queryable
{
    public string Path { get; set; }

    public string Klass { get; set; }

    public string Method { get; set; }

    public bool Static { get; set; }

    public List<AssemblyParameter> MethodParameters { get; set; }

    public AssemblyQuery()
    {
        MethodParameters = new List<AssemblyParameter>();
    }


    public Dictionary<string, object> GetMethodParameters()
    {
        var dico = new Dictionary<string, object>();

        foreach (var param in this.MethodParameters)
        {
            dico.Add(param.Name, param.Value);
        }
        return dico;
    }

}
