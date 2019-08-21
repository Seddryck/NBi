using System;
using System.Linq;
using System.Collections.Generic;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Analysis.Member;
using NBi.NUnit.Builder.Helper;

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
            try 
	        {	        
		        return InstantiateMembersDiscovery(sutXml);
	        }
	        catch (ArgumentOutOfRangeException)
	        {
		
		        throw new ArgumentOutOfRangeException("sutXml", sutXml, "The system-under-test for members must be a hierarchy or a level or a set");
	        }
        }
  
        protected MembersDiscoveryRequest InstantiateMembersDiscovery(MembersXml membersXml)
        {
            string perspective = null, dimension = null, hierarchy = null, level = null, set=null;
            MembersDiscoveryRequest disco = null;

            if (membersXml.Item == null)
                throw new ArgumentNullException();

            if (!(membersXml.Item is HierarchyXml || membersXml.Item is LevelXml || membersXml.Item is SetXml))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (membersXml.Item is HierarchyXml)
            {
                perspective = ((HierarchyXml)membersXml.Item).Perspective;
                dimension = ((HierarchyXml)membersXml.Item).Dimension;
                hierarchy = membersXml.Item.Caption;
            }
            if (membersXml.Item is LevelXml)
            {
                perspective = ((LevelXml)membersXml.Item).Perspective;
                dimension = ((LevelXml)membersXml.Item).Dimension;
                hierarchy = ((LevelXml)membersXml.Item).Hierarchy;
                level = membersXml.Item.Caption;
            }
            if (membersXml.Item is HierarchyXml || membersXml.Item is LevelXml)
            {
                var connectionString = new ConnectionStringHelper().Execute(membersXml.Item, Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest);

                disco = discoveryFactory.Build(
                    connectionString,
                    membersXml.ChildrenOf,
                    membersXml.Exclude.Items,
                    BuildPatterns(membersXml.Exclude.Patterns),
                    perspective,
                    dimension,
                    hierarchy,
                    level);
            }
                
            if (membersXml.Item is SetXml)
            {
                perspective = ((SetXml)membersXml.Item).Perspective;
                set = membersXml.Item.Caption;
                var connectionString = new ConnectionStringHelper().Execute(membersXml.Item, Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest);

                disco = discoveryFactory.Build(
                    connectionString,
                    membersXml.Exclude.Items,
                    BuildPatterns(membersXml.Exclude.Patterns),
                    perspective,
                    set);
            }

            if (disco == null)
                throw new ArgumentException();

            return disco;
        }

		private IEnumerable<PatternValue> BuildPatterns(IEnumerable<PatternXml> patterns)
		{
			var res = new List<PatternValue>();
			if (patterns != null)
			{ 
				foreach (var p in patterns)
				{
					var pv = new PatternValue();
					pv.Pattern = p.Pattern;
					pv.Text = p.Value;
					res.Add(pv);
				}
			}
			return res;
		}
	}
}
