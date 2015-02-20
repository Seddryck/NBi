﻿using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersEquivalentToBuilder : AbstractMembersBuilder
    {
        protected EquivalentToXml ConstraintXml { get; set; }

        public MembersEquivalentToBuilder() : base()
        {
        }

        internal MembersEquivalentToBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is EquivalentToXml))
                throw new ArgumentException("Constraint must be a 'EquivalentToXml'");

            ConstraintXml = (EquivalentToXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(EquivalentToXml ctrXml)
        {
            NBi.NUnit.Member.EquivalentToConstraint ctr;
            if (ctrXml.Query != null)
                ctr = new NBi.NUnit.Member.EquivalentToConstraint(ctrXml.Query.GetCommand());
            else if (ctrXml.Members != null)
            {
                var disco = InstantiateMembersDiscovery(ctrXml.Members);
                ctr = new NBi.NUnit.Member.EquivalentToConstraint(disco);
            }
            else
                ctr = new NBi.NUnit.Member.EquivalentToConstraint(ctrXml.GetItems());

            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

    }
}
