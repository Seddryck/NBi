using System.Collections.Generic;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Xml
{
    [TestFixture]
    public class InstantiateConstraintsTest
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
        public void Instantiate_FirstTest_IsOfTypeQueryParser()
        {
            var t = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>() { new SyntacticallyCorrectXml() }
            };

            var cs = t.Instantiate();

            Assert.That(cs[0], Is.InstanceOf<NBi.NUnit.SyntacticallyCorrectConstraint>());
        }

        [Test]
        public void Instantiate_SecondTest_AsSeveralTypes()
        {
            var t = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>() 
                { 
                    new SyntacticallyCorrectXml(),
                    new FasterThanXml()
                },
            };


            var cs = t.Instantiate();

            Assert.That(cs[0], Is.InstanceOf<NBi.NUnit.SyntacticallyCorrectConstraint>());
            Assert.That(cs[1], Is.InstanceOf<NBi.NUnit.FasterThanConstraint>());
        }
    }
}
