using NBi.Core.Scalar.Resolver;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Resolving;

namespace NBi.Core.Api.Authentication
{
    public class ApiKey : IAuthentication
    {
        public IScalarResolver<string> Name { get; }
        public IScalarResolver<string> Value { get; }

        public ApiKey(IScalarResolver<string> name, IScalarResolver<string> value)
            => (Name, Value) = (name, value);

        public ApiKey(IScalarResolver<string> value)
            : this(new LiteralScalarResolver<string>("apiKey"), value) { }

        public IAuthenticator GetAuthenticator() 
            => new ApiKeyAuthenticator(Name.Execute() ?? string.Empty, Value.Execute() ?? string.Empty);

        public class ApiKeyAuthenticator : IAuthenticator
        {
            public string Name { get; }
            public string Value { get; }

            public ApiKeyAuthenticator(string name, string value)
                => (Name, Value) = (name, value);

            public void Authenticate(IRestClient client, IRestRequest request)
            {
                request.AddHeader(Name, Value);
            }
        }
    }
}
