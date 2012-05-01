using System.Collections.Generic;
using NBi.Xml;
using NBiNu = NBi.NUnit;
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
        public void Instantiate_SyntacticallyCorrectXml_IsOfTypeSyntacticallyCorrectConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new SyntacticallyCorrectXml());

            Assert.That(ctr, Is.InstanceOf<NBiNu.SyntacticallyCorrectConstraint>());
        }

        [Test]
        public void Instantiate_FasterThanXml_IsOfTypeFasterThanConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new FasterThanXml());

            Assert.That(ctr, Is.InstanceOf<NBiNu.FasterThanConstraint>());
        }

        [Test]
        public void Instantiate_EqualToXml_IsOfTypeEqualToConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new EqualToXml());

            Assert.That(ctr, Is.InstanceOf<NBiNu.EqualToConstraint>());
        }
    }
}
