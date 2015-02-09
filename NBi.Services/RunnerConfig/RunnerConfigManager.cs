using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace NBi.Service.RunnerConfig
{
    public class RunnerConfigManager
    {
        private readonly IRunnerConfigBuilder nunitBuilder;
        private readonly IRunnerConfigBuilder gallioBuilder;
      
        public RunnerConfigManager()
        {
            nunitBuilder = new NUnitRunnerConfigBuilder();
            gallioBuilder = new GallioRunnerConfigBuilder();
        }

        internal RunnerConfigManager(IRunnerConfigBuilder nunitBuilder, IRunnerConfigBuilder gallioBuilder)
        {
            this.nunitBuilder = nunitBuilder;
            this.gallioBuilder = gallioBuilder;
        }
        
        private RelativePaths Refine(string rootPath, string frameworkPath, string testSuiteFile)
        {
            if (!Path.IsPathRooted(rootPath))
                throw new ArgumentException();

            if (!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                rootPath += Path.DirectorySeparatorChar.ToString();

            if (!frameworkPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                frameworkPath += Path.DirectorySeparatorChar.ToString();

            if (!testSuiteFile.StartsWith(rootPath) && !Path.IsPathRooted(testSuiteFile))
                throw new ArgumentException();

            if (!frameworkPath.StartsWith(rootPath) && !Path.IsPathRooted(frameworkPath))
                throw new ArgumentException();

            var testSuitePath = Path.GetDirectoryName(testSuiteFile) + Path.DirectorySeparatorChar.ToString();

            var relative = new RelativePaths();

            int upDirectory = testSuiteFile.Remove(0, rootPath.Length).Count(c => c == Path.DirectorySeparatorChar);
            for (int i = 0; i < upDirectory; i++)
                relative.Root +=  ".." + Path.DirectorySeparatorChar;

            if (relative.Root == null)
                relative.Root = string.Empty;

            relative.Framework = frameworkPath.Remove(0, rootPath.Length);
            relative.TestSuite = testSuitePath.Remove(0, rootPath.Length);
            relative.Base = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(testSuiteFile), relative.Root));
            relative.Filename = Path.GetFileNameWithoutExtension(testSuiteFile);

            return relative;
        }

        public void Build(string rootPath, string frameworkPath, string testSuitePath, bool isNUnit, bool isGallio)
        {
            var relative = Refine(rootPath,  frameworkPath,  testSuitePath);
            
            if (isNUnit)
                nunitBuilder.Build(relative.Base, relative.Root, relative.Framework, relative.TestSuite, relative.Filename);
            if (isGallio)
                gallioBuilder.Build(relative.Base, relative.Root, relative.Framework, relative.TestSuite, relative.Filename);
        }

        private class RelativePaths
        {
            public string Root {get; set;}
            public string Framework { get; set; }
            public string TestSuite { get; set; }
            public string Base { get; set; }
            public string Filename { get; set; }
        }
    }
}
