using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Member
{
    [TestFixture]
    public class MembersCommandTest
    {
        [Test]
        public void List_Level_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var cmd = new MembersCommand(connStr, "Members", null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(72));
        }

        [Test]
        public void List_Hierarchy_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var cmd = new MembersCommand(connStr, "Members", null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Month Of Year", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(13));
        }

        [Test]
        public void List_LevelWithExclusionOfJanuary2005_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "January 2005" };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(71));
        }

        [Test]
        public void List_LevelWithExclusionOfJanuary2005AndNovember2005_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "January 2005", "November 2005" };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(70));
        }

        [Test]
        public void List_LevelWithExclusionOfJanuary2005AndNonExistingMember_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "January 2005", "Non existing month 2005" };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(71));
        }

        [Test]
        public void List_HierarchyWithExclusionOfAll_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "All" };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Month Of Year", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(12));
        }

        [Test]
        public void List_HierarchyWithExclusionOfPatternEndingByBer_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "All" };
            var excludedPatterns = new List<PatternValue>() { new PatternValue() {Pattern=Pattern.EndWith, Text="ber" }};
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, excludedPatterns);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Month Of Year", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(8));
        }

        [Test]
        public void List_HierarchyWithExclusionOfPatternStartingByJu_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "All" };
            var excludedPatterns = new List<PatternValue>() { new PatternValue() { Pattern = Pattern.StartWith, Text = "Ju" } };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, excludedPatterns);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Month Of Year", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            Assert.That(result.Count, Is.EqualTo(10));
        }

        [Test]
        public void List_HierarchyWithExclusionOfPatternContainEm_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "All" };
            var excludedPatterns = new List<PatternValue>() { new PatternValue() { Pattern = Pattern.Contain, Text = "em" } };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, excludedPatterns);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Month Of Year", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            //SeptEMber, NovEMber, DecEMber
            Assert.That(result.Count, Is.EqualTo(9));
        }

        [Test]
        public void List_LevelWithExclusionOfPatternContainEm_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var excludedMembers = new List<string>() { "All" };
            var excludedPatterns = new List<PatternValue>() { new PatternValue() { Pattern = Pattern.Contain, Text = "em" } };
            var cmd = new MembersCommand(connStr, "Members", null, excludedMembers, excludedPatterns);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels),
                };

            var result = cmd.List(filters);
            //6years and exlude SeptEMber, NovEMber, DecEMber (+All)
            Assert.That(result.Count, Is.EqualTo(6*9));
        }

        [Test]
        public void List_HierarchyWithMemberChildren_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var member = "Q3 CY 2006";
            var cmd = new MembersCommand(connStr, "Children", member, null, null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            //REturns the 3 months of the Q3
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void List_HierarchyWithMemberChildrenAndExclusion_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var member = "Q3 CY 2006";
            var excludedMembers = new List<string>() { "All" };
            var excludedPatterns = new List<PatternValue>() { new PatternValue() { Pattern = Pattern.Contain, Text = "em" } };
            var cmd = new MembersCommand(connStr, "Children", member, excludedMembers, excludedPatterns);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                };

            var result = cmd.List(filters);
            //REturns the 3 months of the Q3 and remove SeptEMber
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void List_LevelWithMemberChildren_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var member = "January 2005";
            var cmd = new MembersCommand(connStr, "Children", member, null, null);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                };

            var result = cmd.List(filters);
            //Returns the 31 days of the month
            Assert.That(result.Count, Is.EqualTo(31));
        }

        [Test]
        public void List_LevelsWithMemberChildrenAndExclusion_ListOfMembers()
        {
            var connStr = ConnectionStringReader.GetAdomd();
            var member = "January 2005";
            var excludedPatterns = new List<PatternValue>() { new PatternValue() { Pattern = Pattern.Contain, Text = "3" } };
            var cmd = new MembersCommand(connStr, "Children", member, null, excludedPatterns);
            var filters = new List<CaptionFilter>(){ 
                    new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                    new CaptionFilter("Date", DiscoveryTarget.Dimensions),
                    new CaptionFilter("Calendar", DiscoveryTarget.Hierarchies),
                    new CaptionFilter("Month", DiscoveryTarget.Levels)
                };

            var result = cmd.List(filters);
            //Returns the 31 days of the month minus 3, 13, 23, 30, 31
            Assert.That(result.Count, Is.EqualTo(31-5));
        }
    }
}
