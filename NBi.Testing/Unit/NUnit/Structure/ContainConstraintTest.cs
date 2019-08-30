using System.Collections.Generic;
using Moq;
using NBi.Core.Structure;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ContainConstraintTest
    {
        [Test]
        public void Matches_GivenCommand_CommandExecuteCalledOnce()
        {
            var exp = "Expected hierarchy";
            var description = new CommandDescription(Target.Hierarchies,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Dimensions, "dimension-caption")
                        });

            var actuals = new string[] { "Actual hierarchy 1" };

            var commandMock = new Mock<IStructureDiscoveryCommand>();
            commandMock.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandMock.Setup(cmd => cmd.Description).Returns(description);


            var containsConstraint = new ContainConstraint(exp) {};

            //Method under test
            containsConstraint.Matches(commandMock.Object);

            //Test conclusion            
            commandMock.Verify(cmd => cmd.Execute(), Times.Once());
        }


        [Test]
        public void WriteTo_FailingAssertionForOneDimension_TextContainsFewKeyInfo()
        {
            var exp = "Expected hierarchy";
            var description = new CommandDescription(Target.Hierarchies,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Dimensions, "dimension-caption")
                        });

            var actuals = new string[] { "Actual hierarchy 1" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);


            var containsConstraint = new ContainConstraint(exp) { };

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
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("Expected hierarchy"));
        }

        [Test]
        public void WriteTo_FailingAssertionForOneMeasureGroup_TextContainsFewKeyInfo()
        {
            var exp = "Expected measure";
            var description = new CommandDescription(Target.Hierarchies,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.MeasureGroups, "measure-group-caption")
                        });


            var actuals = new string[] { "Actual hierarchy 1" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var containsConstraint = new ContainConstraint(exp) { };

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
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("measure-group-caption").And
                                            .StringContaining("Expected measure"));
        }


        [Test]
        public void WriteTo_FailingAssertionForMultipleHierarchies_TextContainsFewKeyInfo()
        {
            var exp = new string[] {"Expected hierarchy 1", "Expected hierarchy 2"};

            var description = new CommandDescription(Target.Hierarchies,
                       new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Dimensions, "dimension-caption")
                        });

            var actuals = new string[] { "Actual hierarchy 1", "Actual hierarchy 2" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var containsConstraint = new ContainConstraint(exp) { };

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
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchies").And
                                            .StringContaining("Expected hierarchy 1").And
                                            .StringContaining("Expected hierarchy 2"));
        }

        [Test]
        public void WriteTo_FailingAssertionForOnePerspective_TextContainsFewKeyInfo()
        {
            var exp = "Expected perspective";
            var description = new CommandDescription(Target.Perspectives,
                        new CaptionFilter[]{});


            var actuals = new string[] { "Actual perspective 1", "Actual perspective 2" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var containsConstraint = new ContainConstraint(exp) { };

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
            Assert.That(assertionText, Is.StringContaining("find a perspective named 'Expected perspective'.").And
                                            .StringContaining("Actual perspective 1").And
                                            .StringContaining("Actual perspective 2").And
                                            .Not.StringContaining("contain"));
        }

        [Test]
        public void WriteTo_FailingAssertionForTwoPerspectives_TextContainsFewKeyInfo()
        {
            var exp = new string[] { "Expected perspective 1", "Expected perspective 2" } ;
            var description = new CommandDescription(Target.Perspectives,
                        new CaptionFilter[] { });


            var actuals = new string[] { "Actual perspective 1", "Actual perspective 2" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var containsConstraint = new ContainConstraint(exp) { };

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
            Assert.That(assertionText, Is.StringContaining("find the perspectives named").And
                                            .StringContaining("Expected perspective 1").And
                                            .StringContaining("Expected perspective 2").And
                                            .StringContaining(".").And
                                            .StringContaining("Actual perspective 1").And
                                            .StringContaining("Actual perspective 2").And
                                            .Not.StringContaining("contain"));
        }

    }
}
