using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class PathDiscoveryCommand : PerspectiveDiscoveryCommand
    {
        public string Path { get; private set; }

        public PathDiscoveryCommand(string connectionString, string perspectiveName, string path)
            : base(connectionString, perspectiveName)
        {
            Path = path;
        }

        public string[] GetUniqueNames()
        {
            var tokens = Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var uniqueNames = new List<string>();
            string previous = string.Empty;
            foreach (var token in tokens)
            {
                uniqueNames.Add(previous + token);
                previous += token + ".";
            }

            return uniqueNames.ToArray();
        }

        public string GetDepthName()
        {
            return GetDepthName(GetUniqueNames().Count());
        }

        public string GetNextDepthName()
        {
            return GetDepthName(GetUniqueNames().Count()+1);
        }


        private string GetDepthName(int depth)
        {
            switch (depth)
            {
                case 1:
                    return "DIMENSION";
                case 2:
                    return "HIERARCHY";
                case 3:
                    return "LEVEL";
                case 4:
                    return "PROPERTY";
            }
            throw new ArgumentException();
        }

        

    }
}
