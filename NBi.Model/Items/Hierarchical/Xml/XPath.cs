using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Hierarchical.Xml;

public class XPath
{
    public string DefaultNamespacePrefix { get; set; }
    public From From { get; set; }
    public List<Select> Selects { get; set; } = new List<Select>();
}
