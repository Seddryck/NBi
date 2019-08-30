using System;
using System.Collections.Generic;
using Moq;
using NBi.Core.Structure;
using NBi.NUnit.Structure;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ExistsConstraintTest
    {
        [Test]
        public void Matches_GivenCommand_ExecuteCalledOnce()
        {
            var description = new CommandDescription(Target.Dimensions,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });

            var actuals = new string[] { "Actual dimension 1", "Actual dimension 2", "Actual dimension 3" };

            var commandMock = new Mock<IStructureDiscoveryCommand>();
            commandMock.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandMock.Setup(cmd => cmd.Description).Returns(description);

            var existConstraint = new ExistsConstraint("expected-dimension-caption");

            //Method under test
            existConstraint.Matches(commandMock.Object);

            //Test conclusion            
            commandMock.Verify(cmd => cmd.Execute(), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForDimension_TextContainsCaptionOfExpectedDimensionAndNameOfPerspective()
        {
            var description = new CommandDescription(Target.Dimensions,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });

            var actuals = new string[] { "Actual dimension 1", "Actual dimension 2", "Actual dimension 3" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-dimension-caption");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("expected-dimension-caption"));
        }

        [Test]
        public void WriteTo_FailingAssertionForHierarchy_TextContainsCaptionOfExpectedHierarchyAndCaptionOfFilters()
        {
            var description = new CommandDescription(Target.Hierarchies,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Dimensions, "dimension-caption")
                        });

            var actuals = new string[] { "Actual hierarchy 1", "Actual hierarchy 2", "Actual hierarchy 3" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-hierarchy-caption");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion     
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("expected-hierarchy-caption"));
        }

        [Test]
        public void WriteTo_FailingAssertionForMeasureGroup_TextContainsNameOfExpectedMeasureGroupAndNameOfPerspectiveFiltering()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] {};

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-measure-group-caption");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("expected-measure-group-caption"));
        }

        [Test]
        public void WriteTo_FailingAssertionForPerspectiveWithNot_TextContainsFewKeyInfo()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] { "expected-measure-group-caption", "other expected-measure-group-caption" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-measure-group-caption");
            var notExistsConstraint = new NotConstraint(existsConstraint);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, notExistsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion      
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining("not find"));
        }

        [Test]
        public void WriteTo_FailingAssertionForPerspectiveWithInvestigationReturningOtherFields_TextContainsFewKeyInfo()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] { "unexpected-measure-group-1", "unexpected-measure-group-2" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-measure-group-caption");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining(actuals[0]).And
                                            .StringContaining(actuals[1]));
        }

        [Test]
        public void WriteTo_FailingAssertionForPerspectiveWithInvestigationReturningNoField_TextContainsFewKeyInfo()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] {};

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-measure-group-caption");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining("nothing found"));
        }

        [Test]
        public void WriteTo_FailingAssertionForDimensionWithMinorMistake_TextContainsTheSuggestionOfValue()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] { "expected-dimension-catpion" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("expected-dimension-caption");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Is.StringContaining("The value 'expected-dimension-catpion' is close to your expectation."));
        }

        [Test]
        public void Matches_Default_Success()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] { "a", "b", "c" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("a");

            //Method under test
            Assert.That(commandStub.Object, existsConstraint);
        }

        [Test]
        public void Matches_WithIgnoreCase_Success()
        {
            var description = new CommandDescription(Target.MeasureGroups,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                        });


            var actuals = new string[] { "a", "b", "c" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var existsConstraint = new ExistsConstraint("A");
            existsConstraint = existsConstraint.IgnoreCase;

            //Method under test
            Assert.That(commandStub.Object, existsConstraint);
        }


    }
}
