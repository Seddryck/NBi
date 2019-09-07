using NBi.NUnit.Runtime.Embed.Result;
using NBi.NUnit.Runtime;
using NBi.Xml;
using NUnit.Core;
using NUnit.Core.Filters;
using NUnit.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed
{
    public class Engine
    {
        protected string BinPath { get; }

        public TestResult Execute(string configFileName) => Execute(configFileName, TestFilter.Empty);

        public TestResult Execute(string configFileName, ITestFilter filter)
        {
            if (ServiceManager.Services.GetService(typeof(DomainManager))==null)
            {
                ServiceManager.Services.AddService(new DomainManager());
                ServiceManager.Services.InitializeServices();
            }
            
            var package = new NBiPackage(BinPath, configFileName);

            using (var runner = new TestDomain())
            { 
                runner.Load(package);
                var testResult = runner.Run(new NullListener(), filter, false, LoggingThreshold.Warn);
                return testResult;
            }
        }

        public Engine()
        {
            BinPath = string.Empty;
        }

        public Engine(string binPath)
        {
            BinPath = binPath;
        }


    }
}
