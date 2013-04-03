using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Metadata;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.Services
{
    public class StructureTesting
    {
        [Flags]
        public enum Tests
        {
            Perspectives = 1,
            Dimensions = 2,
            Hierarchies = 4,
            Levels = 8,
            Properties = 16,
            Measuregroups = 128,
            Measures = 256
        }

        public ICollection<TestXml> Build(CubeMetadata cube, Tests toBuild)
        {
            var tests = new List<TestXml>();

            foreach (var perspective in cube.Perspectives)
            {
                if ((toBuild & Tests.Perspectives) == Tests.Perspectives)
                    tests.Add(Build(perspective.Value));
                foreach (var dimension in perspective.Value.Dimensions)
                {
                    if ((toBuild & Tests.Dimensions) == Tests.Dimensions)
                        tests.Add(Build(perspective.Value, dimension.Value));
                }
            }

            return tests;
        }

        public TestXml Build(Perspective item)
        {
            var test = new TestXml();
            
            test.Name = string.Format("Perspective '{0}' exists", item.Caption);

            var system = new StructureXml()
                {
                    Item = new PerspectiveXml()
                    {
                        Caption = item.Caption
                    }
                };

            var assert = new ExistsXml();

            test.Systems.Add(system);
            test.Constraints.Add(assert);

            return test;
        }

        public TestXml Build(Perspective perspective, Dimension item)
        {
            var test = new TestXml();

            test.Name = string.Format("Dimension '{0}' exists (through perspective '{1}')", item.Caption, perspective.Caption);

            var system = new StructureXml()
            {
                Item = new DimensionXml()
                {
                    Perspective = perspective.Caption,
                    Caption = item.Caption
                }
            };

            var assert = new ExistsXml();

            test.Systems.Add(system);
            test.Constraints.Add(assert);

            return test;
        }


    }
}
