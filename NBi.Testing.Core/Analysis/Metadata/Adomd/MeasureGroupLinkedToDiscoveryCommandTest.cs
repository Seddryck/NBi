using System.Collections.Generic;
using NBi.Core.Analysis.Request;
using NUnit.Framework;

namespace NBi.Core.Testing.Analysis.Metadata.Adomd;

//[TestFixture]
//public class MeasureGroupLinkedToDiscoveryCommandTest
//{
//    [Test]
//    public void Build_Filters_AllFiltersAreBuilt()
//    {
//        var discovery = new MeasureGroupLinkedToDiscoveryCommand("connectionString");

//        var filters = new List<CaptionFilter>();
//        filters.Add( new CaptionFilter("my perspective", DiscoveryTarget.Perspectives));
//        filters.Add(new CaptionFilter("my measure-group", DiscoveryTarget.MeasureGroups));
//        filters.Add(new CaptionFilter("my dimension", DiscoveryTarget.Dimensions));

//        //Method under test
//        var filterString = discovery.Build(filters);

//        Assert.That(filterString, Does.Contain("my perspective")
//            .And.Contain("my measure-group")
//            .And.Contain("my dimension"));

//    }
//}
