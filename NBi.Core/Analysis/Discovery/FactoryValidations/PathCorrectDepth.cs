using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class PathCorrectDepth : Validation
    {
        private readonly string Path;
        private readonly int ExpectedDepth;

        internal PathCorrectDepth(string path, int expectedDepth)
        {
            Path = path;
            ExpectedDepth = expectedDepth;
        }
        
        internal override void Apply()
        {
            var depth = Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Count();
            
            if (depth!=ExpectedDepth)
                GenerateException(depth);
        }

        internal void GenerateException(int actualDepth)
        {
            throw new DiscoveryFactoryException(string.Format("The path '{2}' was expected to have a depth of {0} but had a depth of {1}", ExpectedDepth, actualDepth, Path), "path");
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException(string.Format("The path '{1}' was expected to have a depth of {0} but it hasn't", ExpectedDepth, Path), "path");
        }
    }
}
