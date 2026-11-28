using NBi.Model.Items.Api.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Hierarchical.Json;

public class JsonSource : BaseItem
{
    public FileSource File { get; set; }

    public Url Url { get; set; }

    public Rest Rest { get; set; }

    public QueryScalar QueryScalar { get; set; }

    public JsonPath JsonPath { get; set; }
}
