using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request.FactoryValidations;

namespace NBi.Core.Analysis.Request
{
    public class DiscoveryRequestFactory
    {
        internal virtual void Validate(List<Validation> validations)
        {
            validations.ForEach(v => v.Apply());
        }

        public virtual MetadataDiscoveryRequest Build(string connectionString, DiscoveryTarget target, string perspective, string measuregroup, string displayFolder, string measure, string dimension, string hierarchy, string level)
        {
            //Validations
            Validate( 
                new List<Validation>()
                {
                    new ConnectionStringNotEmpty(connectionString),
                    !target.Equals(DiscoveryTarget.Perspectives) ? (Validation)new PerspectiveNotNull(perspective) : new NoValidation(),
                    new MeasureGroupWithoutDimension(measuregroup, dimension),
                    new MeasureWithoutDimension(measure, dimension),
                    !string.IsNullOrEmpty(displayFolder) ? (Validation)new MeasureNotNull(dimension) : new NoValidation(),
                    !string.IsNullOrEmpty(hierarchy) ? (Validation)new DimensionNotNull(dimension) : new NoValidation(),
                    !string.IsNullOrEmpty(level) ? (Validation)new HierarchyNotNull(hierarchy) : new NoValidation()
                }
            );
            
            //If validation of parameters is successfull then we build the object
            var disco = new MetadataDiscoveryRequest();
            disco.ConnectionString = connectionString;
            disco.Target = target;
            if (!string.IsNullOrEmpty(perspective))     disco.SpecifyFilter(new CaptionFilter(perspective, DiscoveryTarget.Perspectives));
            if (!string.IsNullOrEmpty(measuregroup))    disco.SpecifyFilter(new CaptionFilter(measuregroup, DiscoveryTarget.MeasureGroups));
            if (!string.IsNullOrEmpty(displayFolder))   disco.SpecifyFilter(new CaptionFilter(displayFolder, DiscoveryTarget.DisplayFolders));
            if (!string.IsNullOrEmpty(measure))         disco.SpecifyFilter(new CaptionFilter(measure, DiscoveryTarget.Measures));
            if (!string.IsNullOrEmpty(dimension))       disco.SpecifyFilter(new CaptionFilter(dimension, DiscoveryTarget.Dimensions));
            if (!string.IsNullOrEmpty(hierarchy))       disco.SpecifyFilter(new CaptionFilter(hierarchy, DiscoveryTarget.Hierarchies));
            if (!string.IsNullOrEmpty(level))           disco.SpecifyFilter(new CaptionFilter(level, DiscoveryTarget.Levels));

            return disco;
        }

        public virtual MembersDiscoveryRequest Build(string connectionString, string memberCaption, string perspective, string dimension, string hierarchy, string level)
        {
            //Validations
            Validate(
                new List<Validation>()
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNotNull(perspective),
                    new DimensionNotNull(dimension),
                    !string.IsNullOrEmpty(level) ? (Validation)new HierarchyNotNull(hierarchy) : new NoValidation()
                }
            );

            //If validation of parameters is successfull then we build the object
            var disco = new MembersDiscoveryRequest();
            disco.ConnectionString = connectionString;
            if (!string.IsNullOrEmpty(perspective))     disco.SpecifyFilter(new CaptionFilter(perspective, DiscoveryTarget.Perspectives));
            if (!string.IsNullOrEmpty(dimension))       disco.SpecifyFilter(new CaptionFilter(dimension, DiscoveryTarget.Dimensions));
            if (!string.IsNullOrEmpty(hierarchy))       disco.SpecifyFilter(new CaptionFilter(hierarchy, DiscoveryTarget.Hierarchies));
            if (!string.IsNullOrEmpty(level))           disco.SpecifyFilter(new CaptionFilter(level, DiscoveryTarget.Levels));
            disco.Function = string.IsNullOrEmpty(memberCaption) ? "members" : "children";
            disco.MemberCaption = memberCaption;
            
            return disco;
        }
   
    }
}
