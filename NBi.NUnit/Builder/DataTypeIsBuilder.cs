using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Structure;
using NBi.Core.DataType;
using NBi.NUnit.DataType;

namespace NBi.NUnit.Builder
{
    class DataTypeIsBuilder : AbstractDataTypeBuilder
    {
        protected IsXml ConstraintXml {get; set;}

        public DataTypeIsBuilder() : base()
        {
        }

        internal DataTypeIsBuilder(DataTypeDiscoveryFactoryProvider discoveryProvider)
            : base(discoveryProvider)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is IsXml))
                throw new ArgumentException("Constraint must be a 'IsXml'");

            if (!(sutXml is DataTypeXml))
                throw new ArgumentException("System-under-test must be a 'DataTypeXml'");

            SystemUnderTestXml = (DataTypeXml)sutXml;

            ConstraintXml = (IsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml, SystemUnderTestXml);
        }

        protected NBiConstraint InstantiateConstraint(IsXml ctrXml, DataTypeXml sutXml)
        {
            var expected = sutXml.Item.Caption;

            var ctr = new IsConstraint(expected);
            
            return ctr;
        }

    }
}
