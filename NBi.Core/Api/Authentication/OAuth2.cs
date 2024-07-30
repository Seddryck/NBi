using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Authentication
{
    public class OAuth2 : IAuthentication
    {
        public IScalarResolver<string> AccessToken { get; }
        public IScalarResolver<string> TokenType { get; }

        public OAuth2(IScalarResolver<string> accessToken, IScalarResolver<string> tokenType)
            => (AccessToken, TokenType) = (accessToken, tokenType);

        public OAuth2(IScalarResolver<string> accessToken)
            : this(accessToken, new LiteralScalarResolver<string>("OAuth")) { }

        public IAuthenticator GetAuthenticator() 
            => new OAuth2AuthorizationRequestHeaderAuthenticator(
                    AccessToken.Execute() ?? throw new NullReferenceException()
                    , TokenType.Execute() ?? throw new NullReferenceException()
                );
    }
}
