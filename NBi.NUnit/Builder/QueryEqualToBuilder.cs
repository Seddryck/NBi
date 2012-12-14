using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class QueryEqualToBuilder : AbstractQueryBuilder
    {
        protected EqualToXml ConstraintXml {get; set;}

        public QueryEqualToBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is EqualToXml))
                throw new ArgumentException("Constraint must be a 'EqualToXml'");

            ConstraintXml = (EqualToXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint()
        {
            EqualToConstraint ctr = null;
            
            if (ConstraintXml.GetCommand() != null)
            {
                ctr = new EqualToConstraint(ConstraintXml.GetCommand());
            }
            else if (ConstraintXml.ResultSet != null)
            {
                if (!string.IsNullOrEmpty(ConstraintXml.ResultSet.File))
                {
                    Console.WriteLine("Debug: ResultSet.File defined in external file!");
                    ctr = new EqualToConstraint(ConstraintXml.ResultSet.File);
                }
                else if (ConstraintXml.ResultSet.Rows!=null)
                {
                    Console.WriteLine("Debug: ResultSet defined in embedded resultSet!");
                    ctr = new EqualToConstraint(ConstraintXml.ResultSet.Rows);
                }
            }
            
            if (ctr==null)
                throw new ArgumentException();

            //Manage settings for comparaison
            ResultSetComparisonSettings settings = new ResultSetComparisonSettings(
                ConstraintXml.KeysDef,
                ConstraintXml.ValuesDef,
                ConstraintXml.ColumnsDef
                );

            ctr.Using(settings);

            //Manage persistance
            EqualToConstraint.PersistanceItems persi = 0;
            if (ConstraintXml.GetCommand() != null)
                persi += (int)EqualToConstraint.PersistanceItems.actual;
            if (SystemUnderTestXml is QueryXml)
                persi += (int)EqualToConstraint.PersistanceItems.expected;
            if (!(persi==0 || ConstraintXml.Query==null || string.IsNullOrEmpty(ConstraintXml.Query.Name)))
                ctr.Persist(ConstraintXml.Persistance, persi, ConstraintXml.Query.Name);

            return ctr;
        }

        protected override IDbCommand InstantiateSystemUnderTest(QueryXml queryXml)
        {
                var conn = ConnectionFactory.Get(queryXml.GetConnectionString());
                var cmd = conn.CreateCommand();
                cmd.CommandText = queryXml.GetQuery();

                return cmd;
        }
    }
}
