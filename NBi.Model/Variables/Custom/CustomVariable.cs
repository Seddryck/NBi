using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Variables.Custom;

public class CustomVariable
{
    public string AssemblyPath { get; set; }

    public string TypeName { get; set; }

    public List<CustomParameter> Parameters { get; set; } = new List<CustomParameter>();
}
