using NBi.Xml.Constraints;
using NBi.Xml.Constraints.EqualTo;
using NBi.Xml.Systems;
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
            var ctr = NBiNu.ConstraintFactory.Instantiate(new SyntacticallyCorrectXml(), null);

            Assert.That(ctr, Is.InstanceOf<NBiNu.SyntacticallyCorrectConstraint>());
        }

        [Test]
        public void Instantiate_FasterThanXml_IsOfTypeFasterThanConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new FasterThanXml(), null);

            Assert.That(ctr, Is.InstanceOf<NBiNu.FasterThanConstraint>());
        }

        [Test]
        public void Instantiate_EqualToXml_IsOfTypeEqualToConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new EqualToXml() { ResultSetFile = "resultset.csv" }, null);

            Assert.That(ctr, Is.InstanceOf<NBiNu.EqualToConstraint>());
        }

        [Test]
        public void Instantiate_EqualToXmlWithInlineQuery_IsOfTypeEqualToConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new EqualToXml() 
                { InlineQuery = "SELECT * FROM Product;", 
                    ConnectionString = "Data Source=.;Initial Catalog='NBi.Testing';Integrated Security=SSPI;"
                }, null);

            Assert.That(ctr, Is.InstanceOf<NBiNu.EqualToConstraint>());
        }

        [Test]
        public void Instantiate_EqualToXmlWithResultSet_IsOfTypeEqualToConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new EqualToXml()
            {
                ResultSet = new ResultSetXml(),
            }, null);

            Assert.That(ctr, Is.InstanceOf<NBiNu.EqualToConstraint>());
        }

        [Test]
        public void Instantiate_CountXml_IsOfTypeCountConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new CountXml() { Exactly = 10 }, null);

            Assert.That(ctr, Is.InstanceOf<NBiNu.Member.CountConstraint>());
        }

        [Test]
        public void Instantiate_ContainsXml_IsOfTypeElementContainsConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new ContainsXml() { Caption = "xYz" }, typeof(StructureXml));

            Assert.That(ctr, Is.InstanceOf<NBiNu.Structure.ContainsConstraint>());
        }

        [Test]
        public void Instantiate_ContainsXml_IsOfTypeMemberContainsConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new ContainsXml() { Caption = "xYz" }, typeof(MembersXml));

            Assert.That(ctr, Is.InstanceOf<NBiNu.Member.ContainsConstraint>());
        }

        [Test]
        public void Instantiate_OrderedXml_IsOfTypeMemberOrderedConstraint()
        {
            var ctr = NBiNu.ConstraintFactory.Instantiate(new OrderedXml(), typeof(MembersXml));

            Assert.That(ctr, Is.InstanceOf<NBiNu.Member.OrderedConstraint>());
        }

        [Test]
        public void Instantiate_ContainsXmlWithNot_IsOfTypeNotConstraint()
        {
            var ctrXml = new ContainsXml() {Not=true, Caption="caption"};
            
            var ctr = NBiNu.ConstraintFactory.Instantiate(ctrXml, typeof(MembersXml));

            Assert.That(ctr, Is.InstanceOf<global::NUnit.Framework.Constraints.NotConstraint>());
        }

    }
}
