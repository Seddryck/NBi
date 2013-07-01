using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    abstract class AbstractMembersBuilder : AbstractTestCaseBuilder
    {
        protected readonly DiscoveryRequestFactory discoveryFactory;

        public AbstractMembersBuilder()
        {
            discoveryFactory = new DiscoveryRequestFactory();
        }

        public AbstractMembersBuilder(DiscoveryRequestFactory factory)
        {
            discoveryFactory = factory;
        }
        
        protected MembersXml SystemUnderTestXml { get; set; }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is MembersXml))
                throw new ArgumentException("Constraint must be a 'MembesrXml'");

            SystemUnderTestXml = (MembersXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);
        }

        protected object InstantiateSystemUnderTest(MembersXml sutXml)
        {
            string perspective = null, dimension = null, hierarchy = null, level = null;

            if (sutXml.Item == null)
                throw new ArgumentNullException();

            if (!(sutXml.Item is HierarchyXml || sutXml.Item is LevelXml))
            {
                throw new ArgumentOutOfRangeException("sutXml", sutXml, "The system-under-test for members must be a hierarchy or a level");
            }

            if (sutXml.Item is HierarchyXml)
            {
                perspective = ((HierarchyXml)sutXml.Item).Perspective;
                dimension = ((HierarchyXml)sutXml.Item).Dimension;
                hierarchy = sutXml.Item.Caption;
            }
            if (sutXml.Item is LevelXml)
            {
                perspective = ((LevelXml)sutXml.Item).Perspective;
                dimension = ((LevelXml)sutXml.Item).Dimension;
                hierarchy = ((LevelXml)sutXml.Item).Hierarchy;
                level = sutXml.Item.Caption;
            }
           
            var disco = discoveryFactory.Build
                (
                    sutXml.Item.GetConnectionString(),
                    sutXml.ChildrenOf,
                    sutXml.Exclude.Items,
                    perspective,
                    dimension,
                    hierarchy,
                    level
                );

            return disco;
        }


    }
}
