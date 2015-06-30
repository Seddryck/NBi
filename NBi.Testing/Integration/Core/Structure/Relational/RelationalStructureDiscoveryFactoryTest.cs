using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NBi.Core.Structure;
using NBi.Core.Structure.Relational;
using System.Data.SqlClient;

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
            var cmd = factory.Instantiate(Target.Schemas, TargetType.Object,
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
                    new CaptionFilter(Target.Schemas, "Sales"),
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
                    new CaptionFilter(Target.Schemas,"Sales"),
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
                    new CaptionFilter(Target.Schemas,"HumanResources")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }

        public void Execute_RoutinesWithName_ListStructureContainingThisRoutine()
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Routines, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Schemas,"dbo")
                    , new CaptionFilter(Target.Routines,"ufnGetContactInformation")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(1));
        }
    }


}
