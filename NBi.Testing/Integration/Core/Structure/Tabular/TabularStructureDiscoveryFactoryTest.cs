using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Structure;
using NBi.Core.Structure.Tabular;

namespace NBi.Testing.Integration.Core.Structure.Tabular
{

    [TestFixture]
    [Category("Olap")]
    public class TabularStructureDiscoveryFactoryTest
    {

        [Test]
        public void Execute_TabularDateDimensionWithHeighTeenHierarchies_ListStructureContainingSevenTeenElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Hierarchies, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives, "Internet Operation"),
                    new CaptionFilter(Target.Dimensions, "Date")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(17));
        }


        [Test]
        public void Execute_TabularCalendarHierarchyWithSixLevels_ListStructureContainingSixElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Levels, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives, "Internet Operation"),
                    new CaptionFilter(Target.Dimensions, "Date"),
                    new CaptionFilter(Target.Hierarchies, "Calendar")
                });
            
            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }


        [Test]
        public void Execute_TabularMonthLevelWithTwoProperties_ListStructureContainingNoElement()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Properties, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Adventure Works"),
                    new CaptionFilter(Target.Dimensions,"Date"),
                    new CaptionFilter(Target.Hierarchies,"Calendar"),
                    new CaptionFilter(Target.Levels,"Month")
                });
            
            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(0));
        }


        [Test]
        public void Execute_TabularMeasureGroupsForFinance_ThreeElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.MeasureGroups, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Internet Operation"),
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }


        [Test]
        public void Execute_TabularMeasuresForMeasureGroupInternetSales_SeventeenElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Measures, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Internet Operation"),
                    new CaptionFilter(Target.MeasureGroups,"Internet Sales")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(17));
        }


        [Test]
        public void Execute_TabularSchemas_FourElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Schemas, TargetType.Object,
                new CaptionFilter[] {
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Execute_TabularTablesForInternetOperation_FifteenElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Tables, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Schemas,"Internet Operation"),
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(15));
        }

        [Test]
        public void Execute_TabularColumnsForInternetSales_TwentySevenElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Columns, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Schemas,"Internet Operation"),
                    new CaptionFilter(Target.Tables,"Internet Sales"),
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(27));
        }

        [Test]
        public void Execute_OnSchemaNamedInternetOperation_ListStructureContainingTenTables()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Tables, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Schemas, "Internet Operation")});

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(15));
        }

        [Test]
        public void Execute_OnTableNamedCurrency_ListStructureContainingThreeColumns()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomdTabular());
            var factory = new TabularStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Columns, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Schemas, "Internet Operation"),
                    new CaptionFilter(Target.Tables, "Currency")
            });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(3));
        }

        //[Test]
        //[Ignore]
        //public void Execute_TabularDateDimensionLinkedToThreeMeasureGroups_ListStructureContainingThreeElements()
        //{
        //    var disco = new DiscoveryRequestFactory().BuildRelation(
        //                ConnectionStringReader.GetAdomd()
        //                , DiscoveryTarget.MeasureGroups
        //                , new List<IFilter>() { 
        //                    new CaptionFilter("Internet Operation", DiscoveryTarget.Perspectives)
        //                    , new CaptionFilter("Date", DiscoveryTarget.Dimensions)
        //                });

        //    var factory = new AdomdDiscoveryCommandFactory();
        //    var cmd = factory.BuildExact(disco);

        //    var structs = cmd.Execute();

        //    Assert.That(structs.Count(), Is.EqualTo(3));
        //}
    }


}
