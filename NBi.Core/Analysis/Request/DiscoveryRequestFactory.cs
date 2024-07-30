using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request.FactoryValidations;
using NBi.Core.Analysis.Member;

namespace NBi.Core.Analysis.Request
{
    public class DiscoveryRequestFactory
    {

        internal virtual void Validate(List<Validation> validations)
        {
            validations.ForEach(v => v.Apply());
        }

        public virtual MetadataDiscoveryRequest BuildDirect(string connectionString, DiscoveryTarget target, IEnumerable<IFilter> filters)
        {
            //Validations
            Validate( 
                [
                    new ConnectionStringNotEmpty(connectionString),
                    !target.Equals(DiscoveryTarget.Perspectives) ? (Validation)new PerspectiveNotNull(filters) : new NoValidation(),
                    new MeasureGroupWithoutDimension(filters),
                    new MeasureWithoutDimension(filters),
                    new MeasureNotNull(filters),
                    new DimensionNotNullIfHierarchy(target, filters),
                    new HierarchyNotNullIfLevel(target, filters),
                    new LevelNotNullIfProperty(target, filters),
                    new TableNotNullIfColumn(target, filters)
                ]
            );
            
            //If validation of parameters is successfull then we build the object
            var disco = new MetadataDiscoveryRequest(connectionString, target, filters);
            return disco;
        }

        public virtual MembersDiscoveryRequest Build(string connectionString, IEnumerable<string> excludedMembers, IEnumerable<PatternValue> excludedPatterns, string perspective, string set)
        {
            Validate(
                [
                    new ConnectionStringNotEmpty(connectionString)
                ]
            );

            //If validation of parameters is successfull then we build the object
            var disco = new MembersDiscoveryRequest(connectionString, string.Empty, string.Empty, excludedMembers, excludedPatterns);
            if (!string.IsNullOrEmpty(perspective)) 
                disco.SpecifyFilter(new CaptionFilter(perspective, DiscoveryTarget.Perspectives));
            if (!string.IsNullOrEmpty(set))
                disco.SpecifyFilter(new CaptionFilter(set, DiscoveryTarget.Sets));

            return disco;
        }

        public virtual MembersDiscoveryRequest Build(string connectionString, string memberCaption, string perspective, string dimension, string hierarchy, string level)
        {
            return Build(connectionString, memberCaption, [], [], perspective, dimension, hierarchy, level);
        }

        public virtual MembersDiscoveryRequest Build(string connectionString, string memberCaption
            , IEnumerable<string> excludedMembers
            , string perspective, string dimension, string hierarchy, string level)
        {
            return Build(connectionString, memberCaption, [], [], perspective, dimension, hierarchy, level);
        }

        public virtual MembersDiscoveryRequest Build(string connectionString, string memberCaption
            , IEnumerable<string> excludedMembers, IEnumerable<PatternValue> excludedPatterns, 
            string perspective, string dimension, string hierarchy, string level)
        {
            //Validations
            Validate(
                [
                    new ConnectionStringNotEmpty(connectionString),
                    //new PerspectiveNotNull(perspective),
                    //new DimensionNotNullIfHierarchy(dimension),
                    //!string.IsNullOrEmpty(level) ? (Validation)new HierarchyNotNullIfLevel(hierarchy) : new NoValidation()
                ]
            );

            //If validation of parameters is successfull then we build the object
            var disco = new MembersDiscoveryRequest(connectionString, string.IsNullOrEmpty(memberCaption) ? "members" : "children", memberCaption, excludedMembers, excludedPatterns);
            if (!string.IsNullOrEmpty(perspective)) disco.SpecifyFilter(new CaptionFilter(perspective, DiscoveryTarget.Perspectives));
            if (!string.IsNullOrEmpty(dimension)) disco.SpecifyFilter(new CaptionFilter(dimension, DiscoveryTarget.Dimensions));
            if (!string.IsNullOrEmpty(hierarchy)) disco.SpecifyFilter(new CaptionFilter(hierarchy, DiscoveryTarget.Hierarchies));
            if (!string.IsNullOrEmpty(level)) disco.SpecifyFilter(new CaptionFilter(level, DiscoveryTarget.Levels));

            return disco;
        }

        public virtual MetadataDiscoveryRequest BuildRelation(string connectionString, DiscoveryTarget target, IEnumerable<IFilter> filters)
        {
            //Validations
            Validate(
                [
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNotNull(filters),
                    new AtLeastOneNotNull(filters, DiscoveryTarget.Dimensions, DiscoveryTarget.MeasureGroups)
                ]
            );

            //If validation of parameters is successfull then we build the object
            var disco = new MetadataLinkedToDiscoveryRequest(connectionString, target, filters);
            return disco;
        }
   
    }
}
