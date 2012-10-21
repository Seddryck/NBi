using System;
using NBi.Core.Analysis.Metadata;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Xml.Systems.Structure;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class ConstraintFactory
    {
        public static Constraint Instantiate(AbstractConstraintXml ctrXml, IAbstractSystemUnderTestXml sutXml)
        {
            Constraint ctr = null;

            if (ctrXml.GetType() == typeof(EqualToXml)) ctr = Instantiate((EqualToXml)ctrXml, sutXml);
            if (ctrXml.GetType() == typeof(FasterThanXml)) ctr = Instantiate((FasterThanXml)ctrXml);
            if (ctrXml.GetType() == typeof(SyntacticallyCorrectXml)) ctr = Instantiate((SyntacticallyCorrectXml)ctrXml);
            if (ctrXml.GetType() == typeof(CountXml)) ctr = Instantiate((CountXml)ctrXml);
            if (ctrXml.GetType() == typeof(ContainsXml)) ctr = Instantiate((ContainsXml)ctrXml, sutXml);
            if (ctrXml.GetType() == typeof(OrderedXml)) ctr = Instantiate((OrderedXml)ctrXml);

            //If not handled by a constructore
            if (ctr==null)
                throw new ArgumentException(string.Format("{0} is not an expected type for a constraint.",ctrXml.GetType().Name));

            //Apply negation if needed
            if (ctrXml.Not)
                ctr = new NotConstraint(ctr);

            return ctr;
        }

        protected static EqualToConstraint Instantiate(EqualToXml ctrXml, IAbstractSystemUnderTestXml sutXml)
        {
            EqualToConstraint ctr = null;
            
            if (ctrXml.GetCommand() != null)
            {
                ctr = new EqualToConstraint(ctrXml.GetCommand());
            }
            else if (ctrXml.ResultSet != null)
            {
                if (!string.IsNullOrEmpty(ctrXml.ResultSet.File))
                {
                    Console.WriteLine("Debug: ResultSet.File defined in external file!");
                    ctr = new EqualToConstraint(ctrXml.ResultSet.File);
                }
                else if (ctrXml.ResultSet.Rows!=null)
                {
                    Console.WriteLine("Debug: ResultSet defined in embedded resultSet!");
                    ctr = new EqualToConstraint(ctrXml.ResultSet.Rows);
                }
            }
            
            if (ctr==null)
                throw new ArgumentException();

            //Manage settings for comparaison
            ResultSetComparisonSettings settings = new ResultSetComparisonSettings(
                ctrXml.KeysDef,
                ctrXml.ValuesDef,
                ctrXml.ColumnsDef
                );

            ctr.Using(settings);

            //Manage persistance
            EqualToConstraint.PersistanceItems persi = 0;
            if (ctrXml.GetCommand() != null)
                persi += (int)EqualToConstraint.PersistanceItems.actual;
            if (sutXml is QueryXml)
                persi += (int)EqualToConstraint.PersistanceItems.expected;
            if (!(persi==0 || ctrXml.Query==null || string.IsNullOrEmpty(ctrXml.Query.Name)))
                ctr.Persist(ctrXml.Persistance, persi, ctrXml.Query.Name);

            return ctr;

            
        }

        protected static FasterThanConstraint Instantiate(FasterThanXml xml)
        {
            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(xml.MaxTimeMilliSeconds);
            if (xml.CleanCache)
                ctr = ctr.CleanCache();
            return ctr;
        }

        protected static SyntacticallyCorrectConstraint Instantiate(SyntacticallyCorrectXml xml)
        {
            var ctr = new SyntacticallyCorrectConstraint();
            return ctr;
        }

        protected static NBi.NUnit.Member.CountConstraint Instantiate(CountXml xml)
        {
            var ctr = new NBi.NUnit.Member.CountConstraint();
            if (xml.Specification.IsExactlySpecified)
                ctr = ctr.Exactly(xml.Exactly);

            if (xml.Specification.IsMoreThanSpecified)
                ctr = ctr.MoreThan(xml.MoreThan);

            if (xml.Specification.IsLessThanSpecified)
                ctr = ctr.LessThan(xml.LessThan);
            return ctr;
        }

        protected static NBi.NUnit.Member.OrderedConstraint Instantiate(OrderedXml xml)
        {
            var ctr = new NBi.NUnit.Member.OrderedConstraint();
            if (xml.Descending)
                ctr = ctr.Descending;

            switch (xml.Rule)
            {
                case OrderedXml.Order.Alphabetical:
                    ctr = ctr.Alphabetical;
                    break;
                case OrderedXml.Order.Chronological:
                    ctr = ctr.Chronological;
                    break;
                case OrderedXml.Order.Numerical:
                    ctr = ctr.Numerical;
                    break;
                case OrderedXml.Order.Specific:
                    ctr = ctr.Specific(xml.Definition);
                    break;
                default:
                    break;
            }

            return ctr;
        }

        protected static Constraint Instantiate(ContainsXml ctrXml, IAbstractSystemUnderTestXml sutXml)
        {
            Constraint ctr = null;
            if (sutXml.IsStructure())
                ctr = InstantiateForStructure(ctrXml);
            else if (sutXml.IsMembers())
                ctr = InstantiateForMember(ctrXml);

            if (ctr==null)
                throw new ArgumentException(string.Format("'{0}' is not an expected type for a system when instantiating a '{1}' constraint.", sutXml.GetType().Name, ctrXml.GetType().Name));

            return ctr;
        }

        private static Constraint InstantiateForStructure(ContainsXml xml)
        {
            NBi.NUnit.Structure.ContainsConstraint ctr=null;

            if (xml.Specification.IsDisplayFolderSpecified)
            {
                ctr = new NBi.NUnit.Structure.ContainsConstraint(
                    new FieldWithDisplayFolder()
                    {
                        Caption = xml.Caption,
                        DisplayFolder = xml.DisplayFolder
                    });
            }
            else
                ctr = new NBi.NUnit.Structure.ContainsConstraint(xml.Caption);

            //Ignore-case if requested
            if (xml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }

        private static Constraint InstantiateForMember(ContainsXml xml)
        {
            var ctr = new NBi.NUnit.Member.ContainsConstraint(xml.Caption);

            //Ignore-case if requested
            if (xml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }
    }
}
