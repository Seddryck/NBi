using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using NBi.NUnit.ResultSetComparison;
using System.Data;

namespace NBi.NUnit.Builder
{
    class ResultSetSupersetOfBuilder : ResultSetEqualToBuilder
    {
        protected override ComparisonKind ComparisonKind
        {
            get { return ComparisonKind.SupersetOf; }
        }

        public ResultSetSupersetOfBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SupersetOfXml))
                throw new ArgumentException("Constraint must be a 'SupersetOfXml'");

            ConstraintXml = (SupersetOfXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(string path)
        {
            return new SupersetOfConstraint(path);
        }
        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IDbCommand cmd)
        {
            return new SupersetOfConstraint(cmd);
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IContent content)
        {
            return new SupersetOfConstraint(content);
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(XPathEngine engine)
        {
            return new SupersetOfConstraint(engine);
        }



    }
}
