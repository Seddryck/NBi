using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Structure;
using NBi.Core.Structure.Olap;

namespace NBi.Testing.Integration.Core.Structure.Olap
{

    [TestFixture]
    [Category("Olap")]
    public class OlapServiceStructureDiscoveryFactoryTest
    {
        [Test]
        public void Execute_Perspective_ListStructureContainingCorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Perspectives, TargetType.Object,
                new CaptionFilter[] {
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        [Test]
        public void Execute_AdventureWorksDimensions_ListStructureContainingNotMeasure()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Dimensions, TargetType.Object,
                new CaptionFilter[] { 
                    new CaptionFilter(Target.Perspectives, "Adventure Works")
                });

            var structs = cmd.Execute();

            Assert.That(structs, Is.Not.Contains("Measures"));
        }

        [Test]
        public void Execute_DateDimensionWithHeighTeenHierarchies_ListStructureContainingCorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Hierarchies, TargetType.Object,
                new CaptionFilter[] { 
                    new CaptionFilter(Target.Perspectives, "Adventure Works"),
                    new CaptionFilter(Target.Dimensions, "Date")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(18));
        }

        [Test]
        public void Execute_CalendarHierarchyWithSixLevels_ListStructureContainingCorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Levels, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Adventure Works"),
                    new CaptionFilter(Target.Dimensions,"Date"),
                    new CaptionFilter(Target.Hierarchies,"Calendar")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(6));
        }

        [Test]
        public void Execute_MonthLevelWithTwoProperties_ListStructureContainingCorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Properties, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Adventure Works"),
                    new CaptionFilter(Target.Dimensions,"Date"),
                    new CaptionFilter(Target.Hierarchies,"Calendar"),
                    new CaptionFilter(Target.Levels,"Month")
                });
            
            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Execute_MeasureGroupsForCubeFinance_CorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.MeasureGroups, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Finance"),
                });
            
            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Execute_MeasuresForMeasureGroupInternetSales_CorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Measures, TargetType.Object,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Adventure Works"),
                    new CaptionFilter(Target.MeasureGroups,"Internet Sales")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(14));
        }

        [Test]
        public void Execute_SetsWithPerspective_ListStructureContainingCorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Sets, TargetType.Object,
                new CaptionFilter[] { 
                    new CaptionFilter(Target.Perspectives, "Channel Sales")
                });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Execute_DateDimensionLinkedTo_ListStructureContainingTenElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.MeasureGroups, TargetType.Relation,
                        new CaptionFilter[] { 
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.Dimensions, "Date")
                        });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(10));
        }

        [Test]
        public void Execute_MeasureGroupSalesOrdersLinkedTo_ListStructureContainingCorrectCountOfElements()
        {
            var conn = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Dimensions, TargetType.Relation,
                        new CaptionFilter[] { 
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.MeasureGroups, "Sales Orders")
                        });

            var structs = cmd.Execute();

            Assert.That(structs.Count(), Is.EqualTo(9));
        }


    }


}
