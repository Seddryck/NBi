using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Service.RunnerConfig
{
    public interface IRunnerConfigBuilder
    {
        void Build(string basePath, string rootPath, string frameworkPath, string testSuitePath, string testSuiteFilename);
    }
}
