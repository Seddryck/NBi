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
            if (xml.GetType() == typeof(EqualToXml)) return Instantiate((EqualToXml)xml);
            if (xml.GetType() == typeof(FasterThanXml)) return Instantiate((FasterThanXml)xml);
            if (xml.GetType() == typeof(SyntacticallyCorrectXml)) return Instantiate((SyntacticallyCorrectXml)xml);
            if (xml.GetType() == typeof(CountXml)) return Instantiate((CountXml)xml);
            if (xml.GetType() == typeof(ContainsXml)) return Instantiate((ContainsXml)xml, systemType);
            if (xml.GetType() == typeof(OrderedXml)) return Instantiate((OrderedXml)xml);

            throw new ArgumentException(string.Format("{0} is not an expected type for a constraint.",xml.GetType().Name));
        }
        
        protected static EqualToConstraint Instantiate(EqualToXml xml)
        {
            EqualToConstraint ctr = null;
            
            if (!string.IsNullOrEmpty(xml.ResultSetFile))
            {
                ctr = new EqualToConstraint(xml.ResultSetFile);
            }
            else if (xml.GetCommand() != null)
            {
                ctr = new EqualToConstraint(xml.GetCommand());
            }
            else if (xml.ResultSet != null)
            {
                Console.WriteLine("Debug: ResultSet defined!");
                if (!string.IsNullOrEmpty(xml.ResultSet.File))
                {
                    Console.WriteLine("Debug: ResultSet.File defined!");
                    ctr = new EqualToConstraint(xml.ResultSet.File);
                }
                else if (xml.ResultSet.Rows!=null)
                {
                    Console.WriteLine("Debug: ResultSet.Rows defined!");
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

            if (systemType == typeof(StructureXml))
                return InstantiateForStructure(xml);
            if (systemType == typeof(MembersXml))
                return InstantiateForMember(xml);

            throw new ArgumentException(string.Format("'{0}' is not an expected type for a system when instantiating a '{1}' constraint.", systemType.Name, xml.GetType().Name));
        }

        private static NBi.NUnit.Structure.ContainsConstraint InstantiateForStructure(ContainsXml xml)
        {
            NBi.NUnit.Structure.ContainsConstraint ctr=null;

            if (xml.Specification.IsDisplayFolderSpecified)
                ctr = new NBi.NUnit.Structure.ContainsConstraint(new FieldWithDisplayFolder() { Caption = xml.Caption, DisplayFolder = xml.DisplayFolder });
            else
                ctr = new NBi.NUnit.Structure.ContainsConstraint(xml.Caption);

            if (xml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }

        private static NBi.NUnit.Member.ContainsConstraint InstantiateForMember(ContainsXml xml)
        {
            var ctr = new NBi.NUnit.Member.ContainsConstraint(xml.Caption);

            if (xml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }
    }
}
