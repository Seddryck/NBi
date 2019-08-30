using NBi.NUnit.Builder.Helper;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.NUnit.Builder.Helper
{
    public class ConnectionStringHelperTest
    {
        private const string CONNECTION_STRING = "server=.;database=db;integrated security=true";
        private const string CONNECTION_STRING_2 = "server=.;database=db;user=x;pwd=y";

        [Test]
        public void Execute_OnlyInlineConnectionString_InlineConnectionStringReturned()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = CONNECTION_STRING
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING));
        }

        [Test]
        public void Execute_OnlyInlineConnectionStringWithCrLfTab_InlineConnectionStringReturned()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = $"\r\n\t\t{CONNECTION_STRING}\r\n"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING));
        }

        [Test]
        public void Execute_ExistingReference_ReferenceValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    References = new List<ReferenceXml>()
                    { new ReferenceXml() { Name="Ref1", ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING_2 } } }
                },
                ConnectionString = "@Ref1"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING_2));
        }

        [Test]
        public void Execute_ExistingReferenceAndDefaultConnectionStringDefinedWithReference_ReferenceValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    References = new List<ReferenceXml>()
                    { new ReferenceXml() { Name="Ref1", ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING_2 } } },
                    Defaults = new List<DefaultXml>()
                    { new DefaultXml() { ApplyTo= SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING } } }
                },
                ConnectionString = "@Ref1"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING_2));
        }

        [Test]
        public void Execute_ExistingReferenceAndDefaultConnectionStringDefined_ReferenceValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    References = new List<ReferenceXml>()
                    { new ReferenceXml() { Name="Ref1", ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING_2 } } },
                    Defaults = new List<DefaultXml>()
                    { new DefaultXml() { ApplyTo= SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING } } }
                },
                ConnectionString = "..."
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo("..."));
        }

        [Test]
        public void Execute_NonExistingReference_ReferenceValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    References = new List<ReferenceXml>()
                    { new ReferenceXml() { Name="Ref1", ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING_2 } } }
                },
                ConnectionString = "@Ref2"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.Null.Or.Empty);
        }

        [Test]
        public void Execute_ExistingDefault_DefaultValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    Defaults = new List<DefaultXml>()
                    { new DefaultXml() { ApplyTo= SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING } } }
                },
                ConnectionString = string.Empty
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING));
        }

        [Test]
        public void Execute_ExistingDefaultOnEveryWhere_DefaultValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    Defaults = new List<DefaultXml>()
                    { new DefaultXml() { ApplyTo= SettingsXml.DefaultScope.Everywhere, ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING } } }
                },
                ConnectionString = string.Empty
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING));
        }

        [Test]
        public void Execute_ExistingDefaultOnEveryWhereWithAlsoAssert_AssertValueReturned()
        {
            var xml = new QueryXml()
            {
                Settings = new SettingsXml()
                {
                    Defaults = new List<DefaultXml>()
                    { new DefaultXml() { ApplyTo= SettingsXml.DefaultScope.Everywhere, ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING } },
                    new DefaultXml() { ApplyTo= SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = CONNECTION_STRING_2 } } }
                },
                ConnectionString = string.Empty
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.EqualTo(CONNECTION_STRING_2));
        }

        [Test]
        public void Execute_NoRoleAdded_NoRoleIncludedInConnectionStringReturned()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = CONNECTION_STRING
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.StringContaining(CONNECTION_STRING));
            Assert.That(actual, Is.Not.StringContaining("Roles="));
        }

        [Test]
        public void Execute_RoleAdded_RoleIncludedInConnectionStringReturned()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = CONNECTION_STRING,
                Roles = "Admin"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.StringContaining(CONNECTION_STRING));
            Assert.That(actual, Is.StringContaining("Roles=\"Admin\""));
        }

        [Test]
        public void Execute_TwoRolesAdded_TwoRolesIncludedInConnectionStringReturned()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = CONNECTION_STRING,
                Roles = "PowerUser;LimitedAccess"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.StringContaining(CONNECTION_STRING));
            Assert.That(actual, Is.StringMatching(".*Roles.*=.*\"PowerUser;LimitedAccess\".*"));
        }

        [Test]
        public void Execute_OneInitialRoleAndOneAdditionalRoleProvided_OneRoleAtTheEnd()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = CONNECTION_STRING + ";Roles=\"Admin\"",
                Roles = "PowerUser"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.StringMatching(".*Roles.*=.*\"PowerUser\".*"));
            Assert.That(actual, Is.Not.StringMatching("Admin"));
        }

        [Test]
        public void Execute_OneInitialRoleWithSpaceAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var xml = new QueryXml()
            {
                Settings = SettingsXml.Empty,
                ConnectionString = CONNECTION_STRING + "Roles = \"Admin Maximum\"",
                Roles = "Power User;Limited Access"
            };

            var helper = new ConnectionStringHelper();
            var actual = helper.Execute(xml, SettingsXml.DefaultScope.Assert);
            Assert.That(actual, Is.StringMatching(".*Roles.*=.*\"Power User;Limited Access\".*"));
            Assert.That(actual, Is.Not.StringMatching("Admin"));
            Assert.That(actual, Is.Not.StringMatching("Maximum"));
        }

    }
}
