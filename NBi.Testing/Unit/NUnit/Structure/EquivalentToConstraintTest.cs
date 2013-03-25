using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class EquivalentToConstraintTest
    {
        [Test]
        public void WriteTo_FailingAssertionForListOfLevels_TextContainsFewKeyInfo()
        {
            var exp = new string[] { "Expected level 1", "Expected level 2" };
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Hierarchies,
                        "perspective-name",
                        null, null, null,
                        "dimension-caption", "hierarchy-caption", null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var containsConstraint = new EquivalentToConstraint(exp) { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("exact").And
                                            .StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("levels").And
                                            .StringContaining("Expected level 1").And
                                            .StringContaining("Expected level 2"));
        }


        

       
    }
}
