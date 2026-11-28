using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Hierarchical.Json;

public class JsonPath
{
    public JsonFrom From { get; set; }
    public List<JsonSelect> Selects { get; set; } = new List<JsonSelect>();
}
