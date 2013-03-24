using System.Collections.Generic;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Analysis.Metadata.Adomd
{
    [TestFixture]
    public class HierarchyDiscoveryCommandTest
    {
        [Test]
        public void Build_FiltersContainDisplayFolder_DisplayFolderIsNotInCommandFilter()
        {
            var discovery = new HierarchyDiscoveryCommand("connectionString");

            var filters = new List<CaptionFilter>();
            filters.Add( new CaptionFilter("my perspective", DiscoveryTarget.Perspectives));
            filters.Add(new CaptionFilter("my dimension", DiscoveryTarget.Dimensions));
            filters.Add(new CaptionFilter("my display-folder", DiscoveryTarget.DisplayFolders));
            filters.Add(new CaptionFilter("my hierarchy", DiscoveryTarget.Hierarchies));

            //Method under test
            var filterString = discovery.Build(filters);

            Assert.That(filterString, Is.Not.StringContaining("Display").And.Not.StringContaining("Folder"));

        }

        [Test]
        public void Build_FiltersContainDisplayFolder_CommandFiltersDoesNotContainDoubleAnd()
        {
            var discovery = new HierarchyDiscoveryCommand("connectionString");

            var filters = new List<CaptionFilter>();
            filters.Add(new CaptionFilter("my perspective", DiscoveryTarget.Perspectives));
            filters.Add(new CaptionFilter("my dimension", DiscoveryTarget.Dimensions));
            filters.Add(new CaptionFilter("my display-folder", DiscoveryTarget.DisplayFolders));
            filters.Add(new CaptionFilter("my hierarchy", DiscoveryTarget.Hierarchies));

            //Method under test
            var filterString = discovery.Build(filters);

            Assert.That(filterString, Is.Not.StringContaining("and  and"));
        }

        [Test]
        public void Build_FiltersContainDisplayFolder_PostCommandFilterIsNotEmpty()
        {
            var discovery = new HierarchyDiscoveryCommand("connectionString");

            var filters = new List<CaptionFilter>();
            filters.Add(new CaptionFilter("my perspective", DiscoveryTarget.Perspectives));
            filters.Add(new CaptionFilter("my dimension", DiscoveryTarget.Dimensions));
            filters.Add(new CaptionFilter("my display-folder", DiscoveryTarget.DisplayFolders));
            filters.Add(new CaptionFilter("my hierarchy", DiscoveryTarget.Hierarchies));

            //Method under test
            discovery.Build(filters);

            Assert.That(discovery.PostCommandFilters, Is.Not.Null.And.Not.Empty);
        }

    }
}
