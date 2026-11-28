using NBi.Model.Items.Api.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Hierarchical.Xml;

public class XmlSource : BaseItem
{
    public bool IgnoreNamespace { get; set; } = false;

    public FileSource File { get; set; }

    public Url Url { get; set; }

    public Rest Rest { get; set; }

    public XPath XPath { get; set; }
}
