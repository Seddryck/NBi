using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Api.Authentication;

public class HttpBasic : BaseAuthentication
{
    public string Username { get; set; }

    public string Password { get; set; }
}
