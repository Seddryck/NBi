using System;
using NBi.Core.Analysis.Metadata;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class ConstraintFactory
    {
        public static Constraint Instantiate(AbstractConstraintXml xml, Type systemType)
        {
            Constraint ctr = null;
            
            if (xml.GetType() == typeof(EqualToXml)) ctr = Instantiate((EqualToXml)xml);
            if (xml.GetType() == typeof(FasterThanXml)) ctr = Instantiate((FasterThanXml)xml);
            if (xml.GetType() == typeof(SyntacticallyCorrectXml)) ctr = Instantiate((SyntacticallyCorrectXml)xml);
            if (xml.GetType() == typeof(CountXml)) ctr = Instantiate((CountXml)xml);
            if (xml.GetType() == typeof(ContainsXml)) ctr = Instantiate((ContainsXml)xml, systemType);
            if (xml.GetType() == typeof(OrderedXml)) ctr = Instantiate((OrderedXml)xml);

            //If not handled by a constructore
            if (ctr==null)
                throw new ArgumentException(string.Format("{0} is not an expected type for a constraint.",xml.GetType().Name));

            //Apply negation if needed
            if (xml.Not)
                ctr = new NotConstraint(ctr);

            return ctr;
        }
        
        protected static EqualToConstraint Instantiate(EqualToXml xml, Type systemType)
        {
            EqualToConstraint ctr = null;
            
            if (xml.GetCommand() != null)
            {
                ctr = new EqualToConstraint(xml.GetCommand());
            }
            else if (xml.ResultSet != null)
            {
                if (!string.IsNullOrEmpty(xml.ResultSet.File))
                {
                    Console.WriteLine("Debug: ResultSet.File defined in external file!");
                    ctr = new EqualToConstraint(xml.ResultSet.File);
                }
                else if (xml.ResultSet.Rows!=null)
                {
                    Console.WriteLine("Debug: ResultSet defined in embedded resultSet!");
                    ctr = new EqualToConstraint(xml.ResultSet.Rows);
                }
            }
            
            if (ctr==null)
                throw new ArgumentException();

            //Manage settings for comparaison
            ResultSetComparisonSettings settings = new ResultSetComparisonSettings(
                xml.KeysDef,
                xml.ValuesDef,
                xml.ColumnsDef
                );

            ctr.Using(settings);

            //Manage persistance
            EqualToConstraint.PersistanceItems persi = 0;
            if (xml.GetCommand() != null)
                persi += (int)EqualToConstraint.PersistanceItems.actual;
            if (systemType == typeof(QueryXml))
                persi += (int)EqualToConstraint.PersistanceItems.expected;
            if (!(persi==0 || xml.Query==null || string.IsNullOrEmpty(xml.Query.Name)))
                ctr.Persist(xml.Persistance, persi, xml.Query.Name);

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

        protected static Constraint Instantiate(ContainsXml xml, Type systemType)
        {
            Constraint ctr = null;
            if (systemType == typeof(StructureXml))
                ctr = InstantiateForStructure(xml);
            if (systemType == typeof(MembersXml))
                ctr = InstantiateForMember(xml);

            if (ctr==null)
                throw new ArgumentException(string.Format("'{0}' is not an expected type for a system when instantiating a '{1}' constraint.", systemType.Name, xml.GetType().Name));

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
