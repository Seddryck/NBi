using NBi.Xml.Constraints;
using NUnit.Framework;
using NBiNu = NBi.NUnit;

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
            var ctr = NBiNu.ConstraintFactory.Instantiate(new EqualToXml() { ResultSetFile="resultset.csv" } );

            Assert.That(ctr, Is.InstanceOf<NBiNu.EqualToConstraint>());
        }

        [Test]
        public void Instantiate_EqualToXmlWithResultSet_IsOfTypeEqualToConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new EqualToXml() 
                { InlineQuery = "SELECT * FROM Product;", 
                    ConnectionString = "Data Source=.;Initial Catalog='NBi.Testing';Integrated Security=SSPI;" 
                });

            Assert.That(ctr, Is.InstanceOf<NBiNu.EqualToConstraint>());
        }

        [Test]
        public void Instantiate_CountXml_IsOfTypeEqualToConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new CountXml() {Exactly=10});

            Assert.That(ctr, Is.InstanceOf<NBiNu.CountConstraint>());
        }

    }
}
