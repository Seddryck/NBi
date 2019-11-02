using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Items.Api.Authentication;
using NBi.Xml.Items.Api.Rest;
using NBi.Xml.Settings;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Builder.Helper
{
    public class RestHelperTest
    {
        [Test]
        public void Execute_RestXml_CorrectInterpretation()
        {
            var xml = new RestXml()
            {
                Authentication = new AuthenticationXml { Protocol = new AnonymousXml() },
                BaseAddress = "https://api.website.com",
                Path= new RestPathXml { Value = "v1/user/{user}" },
                Headers = new List<RestHeaderXml> { new RestHeaderXml { Name = "user-agent", Value="nbi"} },
                Parameters = new List<RestParameterXml> { new RestParameterXml { Name = "order-by", Value = "FullName | text-to-lower" } },
                Segments = new List<RestSegmentXml> { new RestSegmentXml { Name = "user", Value = "@User" } },
            };

            var variables = new Dictionary<string, ITestVariable> { { "User", new GlobalVariable(new LiteralScalarResolver<string>("seddryck")) } };

            var helper = new RestHelper(new ServiceLocator(), null, SettingsXml.DefaultScope.Everywhere , variables);
            var restEngine = helper.Execute(xml);
            Assert.That(restEngine.BaseUrl.Execute(), Is.EqualTo("https://api.website.com"));
            Assert.That(restEngine.Path.Execute(), Is.EqualTo("v1/user/{user}"));
            Assert.That(restEngine.Headers.Count, Is.EqualTo(1));
            Assert.That(restEngine.Headers.First().Name.Execute(), Is.EqualTo("user-agent"));
            Assert.That(restEngine.Headers.First().Value.Execute(), Is.EqualTo("nbi"));
            Assert.That(restEngine.Parameters.Count, Is.EqualTo(1));
            Assert.That(restEngine.Parameters.First().Name.Execute(), Is.EqualTo("order-by"));
            Assert.That(restEngine.Parameters.First().Value.Execute(), Is.EqualTo("fullname"));
            Assert.That(restEngine.Segments.Count, Is.EqualTo(1));
            Assert.That(restEngine.Segments.First().Name.Execute(), Is.EqualTo("user"));
            Assert.That(restEngine.Segments.First().Value.Execute(), Is.EqualTo("seddryck"));
        }

    }
}
