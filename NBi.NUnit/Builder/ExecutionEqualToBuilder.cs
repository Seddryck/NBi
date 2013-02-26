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
    class ExecutionEqualToBuilder : AbstractExecutionBuilder
    {
        protected EqualToXml ConstraintXml {get; set;}

        public ExecutionEqualToBuilder()
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
                    ctr = new EqualToConstraint(ConstraintXml.ResultSet.GetFile());
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
            //EqualToConstraint.PersistanceItems persi = 0;
            //if (ConstraintXml.GetCommand() != null)
            //    persi += (int)EqualToConstraint.PersistanceItems.actual;
            //if (SystemUnderTestXml is QueryXml)
            //    persi += (int)EqualToConstraint.PersistanceItems.expected;
            //if (!(persi==0 || ConstraintXml.Query==null || string.IsNullOrEmpty(ConstraintXml.Test.Name)))
            //    ctr.Persist(ConstraintXml.Persistance, persi, ConstraintXml.Test.Name);

            return ctr;
        }

        protected override IDbCommand InstantiateSystemUnderTest(ExecutionXml queryXml)
        {
            var conn = ConnectionFactory.Instance.Get(queryXml.Item.GetConnectionString());
                var cmd = conn.CreateCommand();
                cmd.CommandText = queryXml.Item.GetQuery();

                return cmd;
        }
    }
}
