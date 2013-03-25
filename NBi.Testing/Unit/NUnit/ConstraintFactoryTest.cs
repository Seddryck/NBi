using System;
using System.Data.SqlClient;
using Moq;
using NBi.NUnit;
using NBi.NUnit.Builder;
using NBi.NUnit.Member;
using NBi.NUnit.Query;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit
{
    [TestFixture]
    public class ConstraintFactoryTest
    {
        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void IsHandling_QuerySyntacticallyCorrect_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }
        
        [Test]
        public void Instantiate_QuerySyntacticallyCorrect_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new SyntacticallyCorrectXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new SyntacticallyCorrectConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(SyntacticallyCorrectXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();

        }

        [Test]
        public void IsHandling_MembersSyntacticallyCorrect_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersSyntacticallyCorrect_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureSyntacticallyCorrect_False()
        {
            var sutXml = new StructureXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureSyntacticallyCorrect_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }


        [Test]
        public void IsHandling_QueryFasterThan_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_QueryFasterThan_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new FasterThanXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new FasterThanConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(FasterThanXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();

        }

        [Test]
        public void IsHandling_MembersFasterThan_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersFasterThan_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureFasterThan_False()
        {
            var sutXml = new StructureXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureFasterThan_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryEqualTo_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_QueryEqualTo_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new EqualToXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new EqualToConstraint("value"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(EqualToXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();

        }

        [Test]
        public void IsHandling_MembersEqualTo_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersEqualTo_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureEqualTo_False()
        {
            var sutXml = new StructureXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureEqualTo_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryContains_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ContainsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryContains_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ContainsXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
       }

        [Test]
        public void IsHandling_MembersContains_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ContainsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersContains_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ContainsXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new NBi.NUnit.Member.ContainsConstraint("expected"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(ContainsXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_StructureContains_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ContainsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_StructureContains_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ContainsXml();
            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new NBi.NUnit.Structure.CollectionItemConstraint("expected"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(StructureXml), typeof(ContainsXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_QueryCount_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryCount_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_MembersCount_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersCount_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new CountXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new CountConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(CountXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_StructureCount_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureCount_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryOrdered_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryOrdered_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_MembersOrdered_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersOrdered_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new OrderedXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new OrderedConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(OrderedXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_StructureOrdered_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureOrdered_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }


        [Test]
        public void IsHandling_QueryExists_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryExists_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_MembersExists_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersExists_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ExistsXml();

            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureExists_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_StructureExists_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ExistsXml();
            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new ExistsConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(StructureXml), typeof(ExistsXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }
    }
}
