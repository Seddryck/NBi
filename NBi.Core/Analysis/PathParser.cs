//using System;
//using System.Linq;
//using NBi.Core.Analysis.Discovery;

//namespace NBi.Core.Analysis
//{
//    public class PathParser
//    {
//        public PathFilter Filter { get; protected set; }
//        public PathPosition Position { get; protected set; }

//        public static PathParser Build(DiscoveryCommand command)
//        {
//            var pathParser = new PathParser();
//            pathParser.Filter = pathParser.GetFilter(command);
//            if (!string.IsNullOrEmpty(command.Path) && command.Path != "[Measures]")
//                pathParser.Position = pathParser.GetPosition(command);
//            return pathParser;
//        }


//        protected PathFilter GetFilter(DiscoveryCommand command)
//        {
//            var filter = new PathFilter();
//            filter.Perspective = command.Perspective;

//            if (command.Target == DiscoverTarget.Perspectives)
//            {
//                filter.Type = FilterType.Cube;           
//            }
//            else if (command.Target == DiscoverTarget.Measures || command.Target == DiscoverTarget.MeasureGroups)
//            {
//                filter.Type = FilterType.Measure;
//                filter.MeasureGroupName = command.MeasureGroup;
//            }
//            else
//            {
//                filter.Type = FilterType.Dimension;

//                string[] parts=null;
//                if (!string.IsNullOrEmpty(command.Path))
//                    parts = command.Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

//                switch (command.Target)
//                {
//                    case DiscoverTarget.Hierarchies:
//                        if (parts.Length != 1)
//                            throw new Exception(string.Format("Path '{0}' is not valid for a target 'hierarchies'", command.Path));
//                        filter.DimensionUniqueName = parts[0];
//                        break;
//                    case DiscoverTarget.Levels:
//                        if (parts.Length != 2)
//                            throw new Exception(string.Format("Path '{0}' is not valid for a target 'hierarchies'", command.Path));
//                        filter.DimensionUniqueName = parts[0];
//                        filter.HierarchyUniqueName = parts[0] + "." + parts[1];
//                        break;
//                    default:
//                        break;
//                }
//            }
//            return filter;
//        }

//        protected PathPosition GetPosition(DiscoveryCommand command)
//        {
//            int partsCount = 0;

//            if (string.IsNullOrEmpty(command.Path))
//                partsCount = 0;
//            else
//                partsCount = command.Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Count();
//            return new PathPosition(partsCount);
//        }

//        public enum FilterType
//        {
//            Cube = 0,
//            Measure = 1,
//            Dimension = 2
//        };

//        public class PathFilter
//        {
            
         
//            public FilterType Type { get; internal set; }
//            public string Perspective { get; set; }
//            public string DimensionUniqueName { get; set; }
//            public string HierarchyUniqueName { get; set; }
//            public string LevelUniqueName { get; set; }
//            public string MeasureGroupName { get; set; }

//            public static PathFilter EmptyFilter()
//            {
//                return new PathFilter();
//            }
//        }

//        public class PathPosition
//        {
//            private readonly int currentPos;
//            private readonly string[] displays = new string[] { "cube", "dimension", "hierarchy", "level", "property" };
//            public PathPosition(int partsCount)
//            {
//                currentPos = partsCount;
//            }

//            public string Current { get { return displays[currentPos]; } }
//            public string Next { get { return displays[currentPos+1]; } }
//        }
//    }
//}
