using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Authenticators;

namespace NBi.Core.Api.Authentication;

public class Anonymous : IAuthentication
{
    public IAuthenticator? GetAuthenticator() => null;
}
