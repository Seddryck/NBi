using System;
using System.Linq;
using NBi.Xml.Items;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class BaseItemTest
    {
        public class BaseItemTestable : BaseItem
        {}

        [Test]
        public void GetConnectionString_NoInitialRoleAndNoAdditionalRoleProvided_NoRoleAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';";

            var connString = item.GetConnectionString();

            Assert.That(connString, Is.Not.StringContaining("role"));
        }

        [Test]
        public void GetConnectionString_NoInitialRoleAndOneAdditionalRoleProvided_OneRoleAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';";
            item.Roles = "PowerUser";

            var connString = item.GetConnectionString();

            Assert.That(connString, Is.StringMatching(".*Roles.*=.*\"PowerUser\".*"));
        }

        [Test]
        public void GetConnectionString_NoInitialRoleAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';";
            item.Roles = "PowerUser;LimitedAccess";

            var connString = item.GetConnectionString();

            Assert.That(connString, Is.StringMatching(".*Roles.*=.*\"PowerUser;LimitedAccess\".*"));
        }

        [Test]
        public void GetConnectionString_OneInitialRoleAndOneAdditionalRoleProvided_OneRoleAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';";
            item.ConnectionString += "Roles=\"Admin\"";
            item.Roles = "PowerUser";

            var connString = item.GetConnectionString();

            Assert.That(connString, Is.StringMatching(".*Roles.*=.*\"PowerUser\".*"));
            Assert.That(connString, Is.Not.StringMatching("Admin"));
        }

        [Test]
        public void GetConnectionString_OneInitialRoleAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';";
            item.ConnectionString += "Roles=\"Admin\"";
            item.Roles = "PowerUser;LimitedAccess";

            var connString = item.GetConnectionString();

            Assert.That(connString, Is.StringMatching(".*Roles.*=.*\"PowerUser;LimitedAccess\".*"));
            Assert.That(connString, Is.Not.StringMatching("Admin"));
        }

        [Test]
        public void GetConnectionString_OneInitialRoleWithSpaceAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';";
            item.ConnectionString += "Roles = \"Admin Maximum\"";
            item.Roles = "Power User;Limited Access";

            var connString = item.GetConnectionString();

            Assert.That(connString, Is.StringMatching(".*Roles.*=.*\"Power User;Limited Access\".*"));
            Assert.That(connString, Is.Not.StringMatching("Admin"));
            Assert.That(connString, Is.Not.StringMatching("Maximum"));
        }
    }
}
