using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NBi.Core.DataType;
using NBi.NUnit.DataType;

namespace NBi.Testing.Unit.NUnit.DataType
{
    [TestFixture]
    public class IsConstraintTest
    {
        [Test]
        public void Matches_GivenCommand_ExecuteCalledOnce()
        {
            var actual = new DataTypeInfo();

            var commandMock = new Mock<IDataTypeDiscoveryCommand>();
            commandMock.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("varchar");

            //Method under test
            isConstraint.Matches(commandMock.Object);

            //Test conclusion            
            commandMock.Verify(cmd => cmd.Execute(), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertion_TextContainsColumnInfo()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });

            var actual = new DataTypeInfo() { Name = "bit" };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("int");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, isConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Does.Contain("ccc-name").And
                                            .StringContaining("table-name").And
                                            .StringContaining("perspective-name"));
        }

        [Test]
        public void WriteTo_FailingAssertionForSimpleType_TextContainsName()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });

            var actual = new DataTypeInfo() { Name = "bit" };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("int");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, isConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Does.Contain("bit").And
                                            .StringContaining("int")
                                            );
        }

        [Test]
        public void WriteTo_FailingAssertionForComplexTypeVersusSimpleType_TextContainsTwoTypeNamesButNotLength()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });

            var actual = new TextInfo() { Name = "varchar", Length = 10 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("nvarchar");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, isConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Does.Contain("varchar").And
                                            .StringContaining("nvarchar").And
                                            .Not.StringContaining("10")
                                            );
        }

        [Test]
        public void WriteTo_FailingAssertionForNumericTypeVersusSimpleType_TextContainsTwoTypeNamesButNotLength()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });

            var actual = new NumericInfo() { Name = "decimal", Scale = 10, Precision = 3 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("varchar");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, isConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Does.Contain("varchar").And
                                            .StringContaining("decimal").And
                                            .Not.StringContaining("10").And
                                            .Not.StringContaining("3")
                                            );
        }

        [Test]
        public void WriteTo_FailingAssertionForComplexType_TextContainsTwoFullTypeNames()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });

            var actual = new TextInfo() { Name = "varchar", Length = 10 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("nvarchar(20)");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, isConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Does.Contain("varchar(10)").And
                                            .StringContaining("nvarchar(20)")
                                            );
        }

        [Test]
        public void WriteTo_FailingAssertionForNumericType_TextContainsTwoFullTypeNames()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });

            var actual = new NumericInfo() { Name = "decimal", Precision=10, Scale = 3 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("decimal(11,2)");

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, isConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion   
            Console.WriteLine(assertionText);
            Assert.That(assertionText, Does.Contain("decimal(11,2)").And
                                            .StringContaining("decimal(10,3)")
                                            );
        }

        [Test]
        public void Matches_Bit_Success()
        {
            var actual = new DataTypeInfo() { Name = "bit" };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("bit");

            //Method under test
            Assert.That(commandStub.Object, isConstraint);
        }

        [Test]
        public void Matches_Varchar_Success()
        {
            var actual = new TextInfo() { Name = "varchar", Length = 10 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("varchar");

            //Method under test
            Assert.That(commandStub.Object, isConstraint);
        }

        [Test]
        public void Matches_Varchar10_Success()
        {
            var actual = new TextInfo() { Name = "varchar", Length=10 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("varchar(10)");

            //Method under test
            Assert.That(commandStub.Object, isConstraint);
        }

        [Test]
        public void Matches_Int_Success()
        {
            var actual = new NumericInfo() { Name = "int" };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("int");

            //Method under test
            Assert.That(commandStub.Object, isConstraint);
        }

        [Test]
        public void Matches_Decimal_Success()
        {
            var actual = new NumericInfo() { Name = "decimal", Scale = 10, Precision = 3 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("decimal");

            //Method under test
            Assert.That(commandStub.Object, isConstraint);
        }

        [Test]
        public void Matches_Decimal10Coma3_Success()
        {
            var actual = new NumericInfo() { Name = "decimal", Precision=10 , Scale = 3 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);

            var isConstraint = new IsConstraint("decimal(10,3)");

            //Method under test
            Assert.That(commandStub.Object, isConstraint);
        }

        [Test]
        public void Matches_BitWithInt_Failure()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });
            var actual = new DataTypeInfo() { Name = "bit" };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("int");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(commandStub.Object, isConstraint); });
        }

        public void Matches_Varchar10WithVarchar20_Failure()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });
            var actual = new TextInfo() { Name = "varchar", Length=10 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("varchar(20)");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(commandStub.Object, isConstraint); });
        }

        public void Matches_Decimal10Coma3WithDecimal10Coma2_Failure()
        {
            var description = new CommandDescription(Target.Columns,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Tables, "table-name")
                                , new CaptionFilter(Target.Columns, "ccc-name")
                        });
            var actual = new NumericInfo() { Name = "decimal", Scale = 10, Precision=3 };

            var commandStub = new Mock<IDataTypeDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actual);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var isConstraint = new IsConstraint("decimal(10,2)");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(commandStub.Object, isConstraint); });
        }


    }
}
