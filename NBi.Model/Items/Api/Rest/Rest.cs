using NBi.Model.Items.Api.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Api.Rest;

public class Rest
{
    public string BaseAddress { get; set; }

    public Authentication Authentication { get; set; } = new Authentication { Protocol = new Anonymous() };

    public List<RestHeader> Headers { get; set; } = new List<RestHeader>();

    public RestPath Path { get; set; }

    public List<RestSegment> Segments { get; set; } = new List<RestSegment>();

    public List<RestParameter> Parameters { get; set; } = new List<RestParameter>();
}
