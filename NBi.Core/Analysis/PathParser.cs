using System;
using System.Linq;
using NBi.Core.Analysis.Metadata;

namespace NBi.Core.Analysis
{
    public class PathParser
    {
        public PathFilter Filter { get; protected set; }
        public PathPosition Position { get; protected set; }

        public static PathParser Build(DiscoverCommand command)
        {
            var pathParser = new PathParser();
            pathParser.Filter = pathParser.GetFilter(command);
            pathParser.Position = pathParser.GetPosition(command);
            return pathParser;
        }


        protected PathFilter GetFilter(DiscoverCommand command)
        {
            if (string.IsNullOrEmpty(command.Perspective))
                throw new ArgumentNullException();

            var filter = new PathFilter();
            filter.Perspective = command.Perspective;
            var parts = command.Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (command.IsMeasureBased)
            {
                filter.Type = FilterType.Measure;
                
                if (parts.Length == 2)
                    filter.MeasureGroupName = parts[1].Replace("[", "").Replace("]", "");
                else
                    throw new ArgumentException("Path for a measure must have two parts!");
            }
            else
            {
                filter.Type = FilterType.Dimension;

                if (parts.Length > 0)
                    filter.DimensionUniqueName = parts[0];

                if (parts.Length > 1)
                    filter.HierarchyUniqueName = string.Format("{0}.{1}", parts);

                if (parts.Length > 2)
                    filter.LevelUniqueName = string.Format("{0}.{1}.{2}", parts);
            }
            return filter;
        }

        protected PathPosition GetPosition(DiscoverCommand command)
        {
            var partsCount = command.Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Count();
            return new PathPosition(partsCount);
        }

        public enum FilterType
        {
            Measure = 1,
            Dimension = 2
        };

        public class PathFilter
        {
            
         
            public FilterType Type { get; internal set; }
            public string Perspective { get; set; }
            public string DimensionUniqueName { get; set; }
            public string HierarchyUniqueName { get; set; }
            public string LevelUniqueName { get; set; }
            public string MeasureGroupName { get; set; }

            public static PathFilter EmptyFilter()
            {
                return new PathFilter();
            }
        }

        public class PathPosition
        {
            private readonly int currentPos;
            private string[] displays = new string[] {"dimension", "hierarchy", "level", "property"};
            public PathPosition(int partsCount)
	        {
                currentPos = partsCount-1;
	        }

            public string Current { get { return displays[currentPos]; } }
            public string Next { get { return displays[currentPos+1]; } }
        }
    }
}
