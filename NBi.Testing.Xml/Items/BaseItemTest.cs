using System;
using System.Linq;
using NBi.Xml.Items;
using NUnit.Framework;

namespace NBi.Testing.Xml.Unit.Items
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
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2017;Initial Catalog='Adventure Works DW 2012';";

            var connString = item.GetConnectionString();

            Assert.That(connString, Does.Not.Contain("role"));
        }

        [Test]
        public void GetConnectionString_NoInitialRoleAndOneAdditionalRoleProvided_OneRoleAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2017;Initial Catalog='Adventure Works DW 2012';";
            item.Roles = "PowerUser";

            var connString = item.GetConnectionString();

            Assert.That(connString, Does.Match(".*Roles.*=.*\"PowerUser\".*"));
        }

        [Test]
        public void GetConnectionString_NoInitialRoleAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2017;Initial Catalog='Adventure Works DW 2012';";
            item.Roles = "PowerUser;LimitedAccess";

            var connString = item.GetConnectionString();

            Assert.That(connString, Does.Match(".*Roles.*=.*\"PowerUser;LimitedAccess\".*"));
        }

        [Test]
        public void GetConnectionString_OneInitialRoleAndOneAdditionalRoleProvided_OneRoleAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2017;Initial Catalog='Adventure Works DW 2012';";
            item.ConnectionString += "Roles=\"Admin\"";
            item.Roles = "PowerUser";

            var connString = item.GetConnectionString();

            Assert.That(connString, Does.Match(".*Roles.*=.*\"PowerUser\".*"));
            Assert.That(connString, Does.Not.Match("Admin"));
        }

        [Test]
        public void GetConnectionString_OneInitialRoleAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2017;Initial Catalog='Adventure Works DW 2012';";
            item.ConnectionString += "Roles=\"Admin\"";
            item.Roles = "PowerUser;LimitedAccess";

            var connString = item.GetConnectionString();

            Assert.That(connString, Does.Match(".*Roles.*=.*\"PowerUser;LimitedAccess\".*"));
            Assert.That(connString, Does.Not.Match("Admin"));
        }

        [Test]
        public void GetConnectionString_OneInitialRoleWithSpaceAndTwoAdditionalRolesProvided_TwoRolesAtTheEnd()
        {
            var item = new BaseItemTestable();
            item.ConnectionString = @"Provider=MSOLAP.4;Data Source=(local)\SQL2017;Initial Catalog='Adventure Works DW 2012';";
            item.ConnectionString += "Roles = \"Admin Maximum\"";
            item.Roles = "Power User;Limited Access";

            var connString = item.GetConnectionString();

            Assert.That(connString, Does.Match(".*Roles.*=.*\"Power User;Limited Access\".*"));
            Assert.That(connString, Does.Not.Match("Admin"));
            Assert.That(connString, Does.Not.Match("Maximum"));
        }
    }
}
