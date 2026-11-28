using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Api.Authentication;

public class OAuth2 : BaseAuthentication
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }
}
