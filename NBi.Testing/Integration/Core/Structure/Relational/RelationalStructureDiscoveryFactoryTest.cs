using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NBi.Core.Structure;
using NBi.Core.Structure.Relational;
using System.Data.SqlClient;
using NBi.Core.Structure.Relational.PostFilters;

namespace NBi.Testing.Integration.Core.Structure.Relational
{

    [TestFixture]
    [Category("Sql")]
    public class RelationalServiceStructureDiscoveryFactoryTest
    {
        [Test]
        public void Execute_Schema_ListStructureContainingSixElements()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Perspectives, TargetType.Object,
                new CaptionFilter[] {
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6 + 2));//6 standards + Northwind + Olympics
        }

        [Test]
        public void Execute_Table_ListStructureContainingFifteenElements()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Tables, TargetType.Object,
                new CaptionFilter[] { 
                    new CaptionFilter(Target.Perspectives, "Sales"),
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(19 + 7));//Tables + Views
        }

        [Test]
        public void Execute_Column_ListStructureContainingSevenElements()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Columns, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Sales"),
                    new CaptionFilter(Target.Tables,"Customer"),
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(7));
        }


        //[Test]
        //public void Execute_DateDimensionLinkedToElevenMeasureGroups_ListStructureContainingTenElements()
        //{
        //    var disco = new DiscoveryRequestFactory().BuildRelation(
        //                ConnectionStringReader.GetAdomd()
        //                , DiscoveryTarget.MeasureGroups
        //                , new List<IFilter>() { 
        //                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
        //                    , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
        //                });

        //    var factory = new AdomdDiscoveryCommandFactory();
        //    var cmd = factory.BuildExact(disco);

        //    var structs = cmd.Execute();

        //    Assert.That(structs.Count(), Is.EqualTo(10));
        //}

        [Test]
        public void Execute_Routines_ListStructureContainingThreeElements()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Routines, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"HumanResources")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_RoutinesWithName_ListStructureContainingThisRoutine()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Routines, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"dbo")
                    , new CaptionFilter(Target.Routines,"ufnGetContactInformation")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_Parameters_ListStructureContainingFiveElements()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Parameters, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"HumanResources")
                    , new CaptionFilter(Target.Routines,"uspUpdateEmployeePersonalInfo")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(5));
        }

        [Test]
        public void Execute_ParameterWithName_ListStructureContainingThisParameter()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Parameters, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"HumanResources")
                    , new CaptionFilter(Target.Routines,"uspUpdateEmployeePersonalInfo")
                    , new CaptionFilter(Target.Parameters,"BirthDate")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(1));
            Assert.That(structs.ElementAt(0), Is.EqualTo("BirthDate"));
        }

        [Test]
        [TestCase(true, 0)]
        [TestCase(false, 1)]
        public void Execute_ParameterWithNameAndResult_ListStructureContainingThisParameter(bool isResult, int count)
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Parameters, TargetType.Object,
                new IFilter[] {
                    new CaptionFilter(Target.Perspectives,"HumanResources")
                    , new CaptionFilter(Target.Routines,"uspUpdateEmployeePersonalInfo")
                    , new CaptionFilter(Target.Parameters,"BirthDate")
                    , new IsResultFilter(isResult)
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(count));
        }
    }


}
