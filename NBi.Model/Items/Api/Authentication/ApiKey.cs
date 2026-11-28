using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Api.Authentication;

public class ApiKey : BaseAuthentication
{
    public string Name { get; set; } = "apiKey";

    public string Value { get; set; }
}
