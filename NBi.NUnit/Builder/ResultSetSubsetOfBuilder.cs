using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
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
    class ResultSetSubsetOfBuilder : ResultSetEqualToBuilder
    {
        public ResultSetSubsetOfBuilder()
        {

        }
        protected override ComparisonKind ComparisonKind
        {
            get { return ComparisonKind.SubsetOf; }
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SubsetOfXml))
                throw new ArgumentException("Constraint must be a 'SubsetOfXml'");

            ConstraintXml = (SubsetOfXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(string path)
        {
            return new SubsetOfConstraint(path);
        }
        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IDbCommand cmd)
        {
            return new SubsetOfConstraint(cmd);
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IContent content)
        {
            return new SubsetOfConstraint(content);
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(XPathEngine engine)
        {
            return new SubsetOfConstraint(engine);
        }



    }
}
