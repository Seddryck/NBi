using NBi.Extensibility.Resolving;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Authentication
{
    public class NtlmUserPassword : IAuthentication
    {
        public IScalarResolver<string> Username { get; }
        public IScalarResolver<string> Password { get; }

        public NtlmUserPassword(IScalarResolver<string> username, IScalarResolver<string> password)
            => (Username, Password) = (username, password);

        public IAuthenticator GetAuthenticator() 
            => new NtlmAuthenticator(Username.Execute() ?? string.Empty, Password.Execute() ?? string.Empty);
    }

}
