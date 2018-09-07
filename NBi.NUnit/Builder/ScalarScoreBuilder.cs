using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.NUnit.ResultSetComparison;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using System.Data;
using NBi.Core.ResultSet.Resolver;
using System.IO;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class ScalarScoreBuilder : AbstractResultSetBuilder
    {
        protected ScoreXml ConstraintXml { get; set; }

        public ScalarScoreBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ScoreXml))
                throw new ArgumentException("Constraint must be a 'ScoreXml'");

            ConstraintXml = (ScoreXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint()
        {
            return new ScoreConstraint(ConstraintXml.Threshold);
        }
    }
}
