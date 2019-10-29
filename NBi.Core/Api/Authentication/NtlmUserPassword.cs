using NBi.Core.Scalar.Resolver;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Authentication
{
    class NtlmUserPassword : IAuthentication
    {
        public IScalarResolver<string> Username { get; }
        public IScalarResolver<string> Password { get; }

        public NtlmUserPassword(IScalarResolver<string> username, IScalarResolver<string> password)
            => (Username, Password) = (username, password);

        public IAuthenticator GetAuthenticator() => new NtlmAuthenticator(Username.Execute(), Password.Execute());
    }

}
