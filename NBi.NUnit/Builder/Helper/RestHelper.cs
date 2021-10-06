using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Api.Rest;
using NBi.Xml.Items.Api.Rest;
using NBi.Core.Api.Authentication;
using NBi.Xml.Items.Api.Authentication;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder.Helper
{
    public class RestHelper
    {
        private ServiceLocator ServiceLocator { get; }
        private IDictionary<string, IVariable> Variables { get; }
        private SettingsXml Settings { get; } = SettingsXml.Empty;
        private SettingsXml.DefaultScope Scope { get; } = SettingsXml.DefaultScope.Everywhere;

        public RestHelper(ServiceLocator serviceLocator, SettingsXml settings, SettingsXml.DefaultScope scope, IDictionary<string, IVariable> variables)
            => (ServiceLocator, Settings, Scope, Variables) = (serviceLocator, settings ?? SettingsXml.Empty, scope, variables ?? new Dictionary<string, IVariable>());

        public RestEngine Execute(object rest)
        {
            switch (rest)
            {
                case RestXml x: return BuildRestEngine(x);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private RestEngine BuildRestEngine(RestXml restXml)
        {
            var helper = new ScalarHelper(ServiceLocator, Settings, Scope, new Context(Variables));

            var authentication = BuildRestAuthentication(restXml.Authentication);
            var resolverUrl = helper.InstantiateResolver<string>(restXml.BaseAddress);
            var resolverPath = helper.InstantiateResolver<string>(restXml.Path.Value);

            var parameters = restXml.Parameters.Select(x => new ParameterRest(
                helper.InstantiateResolver<string>(x.Name)
                , helper.InstantiateResolver<string>(x.Value)
                ));

            var segments = restXml.Segments.Select(x => new SegmentRest(
                helper.InstantiateResolver<string>(x.Name)
                , helper.InstantiateResolver<string>(x.Value)
                ));

            var headers = restXml.Headers.Select(x => new HeaderRest(
                helper.InstantiateResolver<string>(x.Name)
                , helper.InstantiateResolver<string>(x.Value)
                ));

            return new RestEngine(authentication, resolverUrl, resolverPath, parameters, segments, headers);
        }

        private IAuthentication BuildRestAuthentication(AuthenticationXml authentication)
        {
            var helper = new ScalarHelper(ServiceLocator, Settings, Scope, new Context(Variables));
            switch (authentication.Protocol)
            {
                case AnonymousXml _: return new Anonymous();
                case ApiKeyXml x: return new ApiKey(helper.InstantiateResolver<string>(x.Name), helper.InstantiateResolver<string>(x.Value));
                case HttpBasicXml x: return new HttpBasic(helper.InstantiateResolver<string>(x.Username), helper.InstantiateResolver<string>(x.Password));
                case NtmlCurrentUserXml _: return new NtlmCurrentUser();
                case NtmlUserPasswordXml x: return new NtlmUserPassword(helper.InstantiateResolver<string>(x.Username), helper.InstantiateResolver<string>(x.Password));
                case OAuth2Xml x: return new OAuth2(helper.InstantiateResolver<string>(x.AccessToken), helper.InstantiateResolver<string>(x.TokenType));
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
