using NBi.Core.Scalar.Resolver;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Authentication;

public class NtlmCurrentUser : IAuthentication
{
    public NtlmCurrentUser()
    { }

    public IAuthenticator GetAuthenticator() => new NtlmAuthenticator();
}
