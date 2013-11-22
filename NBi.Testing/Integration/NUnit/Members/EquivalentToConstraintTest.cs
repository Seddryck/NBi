#region Using directives
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Member;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit.Members
{
	[TestFixture]
	public class EquivalentToConstraintTest
	{

		#region SetUp & TearDown
		//Called only at instance creation
		[TestFixtureSetUp]
		public void SetupMethods()
		{

		}

		//Called only at instance destruction
		[TestFixtureTearDown]
		public void TearDownMethods()
		{
		}

		//Called before each test
		[SetUp]
		public void SetupTest()
		{
		}

		//Called after each test
		[TearDown]
		public void TearDownTest()
		{
		}
		#endregion


		[Test, Category("Sql"), Category("Mdx")]
		public void Matches_SqlQueryAndMembers_Succesful()
		{
			var command = new SqlCommand();
			command.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
			command.CommandText = "select " +
				"'Executive General and Administration' union select " +
				"'Inventory Management' union select " +
                "'Manufacturing' union select " +
                "'Research and Development' union select " +
                "'Quality Assurance' union select " +
				"'Sales and Marketing' ";

			var discovery = new DiscoveryRequestFactory().Build(
						ConnectionStringReader.GetAdomd()
						, "Corporate"
						, "Adventure Works"
						, "Department"
						, "Departments"
						, null);

			var ctr = new EquivalentToConstraint(command);

			Assert.That(discovery, ctr);
		}
	}
}
