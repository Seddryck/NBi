using System;
using System.Linq;

namespace NBi.Core.Analysis
{
    public class PathParser
    {
        public PathFilter Filter { get; protected set; }
        public PathPosition Position { get; protected set; }

        public static PathParser Build(string perspective, string path)
        {
            var pathParser = new PathParser();
            pathParser.Filter = pathParser.GetFilter(perspective, path);
            pathParser.Position = pathParser.GetPosition(path);
            return pathParser;
        }


        protected PathFilter GetFilter(string perspective, string path)
        {
            if (string.IsNullOrEmpty(perspective))
                throw new ArgumentNullException();

            var filter = new PathFilter();
            filter.Perspective = perspective;

            var parts = path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
                filter.DimensionUniqueName = parts[0];

            if (parts.Length > 1)
                filter.HierarchyUniqueName = string.Format("{0}.{1}", parts);

            if (parts.Length > 2)
                filter.LevelUniqueName = string.Format("{0}.{1}.{2}", parts);

            return filter;
        }

        protected PathPosition GetPosition(string path)
        {
            var partsCount = path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Count();
            return new PathPosition(partsCount);
        }

        public class PathFilter
        {
            public string Perspective { get; set; }
            public string DimensionUniqueName { get; set; }
            public string HierarchyUniqueName { get; set; }
            public string LevelUniqueName { get; set; }

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
