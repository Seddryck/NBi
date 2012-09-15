using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Analysis.Discovery.FactoryValidations;

namespace NBi.Core.Analysis.Discovery
{
    public class DiscoveryFactory
    {
        public static DiscoveryCommand BuildForCube(string connectionString)
        {
            Validate( new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString)
                });
            
            var disco = new CubeDiscoveryCommand(connectionString);
            disco.Initialize();
            return disco;
        }


        public static DiscoveryCommand BuildForPerspective(string connectionString, string perspectiveName)
        {
            Validate(new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNameNotEmpty(perspectiveName)
                });

            var disco = new PerspectiveDiscoveryCommand(connectionString, perspectiveName);
            disco.Initialize();
            return disco;
        }

        public static DiscoveryCommand BuildForMeasureGroup(string connectionString, string perspectiveName, string measureGroupName)
        {
            Validate(new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNameNotEmpty(perspectiveName),
                    new MeasureGroupNameNotEmpty(measureGroupName)
                });

            var disco = new MeasureGroupDiscoveryCommand(connectionString, perspectiveName, measureGroupName);
            disco.Initialize();
            return disco;
        }

        public static DiscoveryCommand BuildForDimension(string connectionString, string perspectiveName, string path)
        {
            Validate(new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNameNotEmpty(perspectiveName),
                    new PathNotEmpty(path),
                    new PathCorrectDepth(path, 1),
                });

            var disco = new DimensionDiscoveryCommand(connectionString, perspectiveName, path);
            disco.Initialize();
            return disco;
        }

        public static DiscoveryCommand BuildForHierarchy(string connectionString, string perspectiveName, string path)
        {
            Validate(new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNameNotEmpty(perspectiveName),
                    new PathNotEmpty(path),
                    new PathCorrectDepth(path, 2),
                });

            var disco = new HierarchyDiscoveryCommand(connectionString, perspectiveName, path);
            disco.Initialize();
            return disco;
        }

        public static DiscoveryCommand BuildForLevel(string connectionString, string perspectiveName, string path)
        {
            Validate(new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNameNotEmpty(perspectiveName),
                    new PathNotEmpty(path),
                    new PathCorrectDepth(path, 3),
                });

            var disco = new LevelDiscoveryCommand(connectionString, perspectiveName, path);
            disco.Initialize();
            return disco;
        }

        public static MembersDiscoveryCommand BuildForMembers(string connectionString, string perspectiveName, string path)
        {
            return BuildForMembers(connectionString, perspectiveName, path, string.Empty);
        }

        public static MembersDiscoveryCommand BuildForMembers(string connectionString, string perspectiveName, string path, string memberCaption)
        {
            Validate(new List<Validation>() 
                {
                    new ConnectionStringNotEmpty(connectionString),
                    new PerspectiveNameNotEmpty(perspectiveName),
                    new PathNotEmpty(path),
                });
            string function = string.IsNullOrEmpty(memberCaption) ? "members" : "children";
            var disco = new MembersDiscoveryCommand(connectionString, perspectiveName, path, memberCaption, function);
            disco.Initialize();
            return disco;
        }

        internal static void Validate(List<Validation> validations)
        {
            validations.ForEach(v => v.Apply());
        }

    
    }
}
