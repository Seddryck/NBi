using System.Collections.Generic;
using Moq;
using NBi.NUnit.Structure;
using NUnit.Framework;
using NBi.Core.Structure.Olap;
using NBi.Core.Structure;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class SubsetOfConstraintTest
    {
        [Test]
        public void WriteTo_FailingAssertionForListOfLevels_TextContainsFewKeyInfo()
        {
            var exp = new string[] { "Expected level 1", "Expected level 2" };
            var description = new CommandDescription(Target.Hierarchies,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Dimensions, "dimension-caption")
                                , new CaptionFilter(Target.Hierarchies, "hierarchy-caption" )
                        });


            var actuals = new string[] { "Actual level 1", "Actual level 2", "Actual level 3" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var containsConstraint = new ContainedInConstraint(exp);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Does.Contain("set").And
                                            .StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("levels").And
                                            .StringContaining("Expected level 1").And
                                            .StringContaining("Expected level 2"));
        }





    }
}
