using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Authentication;

public interface IAuthentication
{
    IAuthenticator? GetAuthenticator();
}
