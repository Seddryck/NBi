using System;
using System.IO;
using System.Linq;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.NUnit.Runtime
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        public bool EnableAutoCategories { get; set; }

        internal XmlManager TestSuiteManager { get; private set; }
        internal TestSuiteFinder TestSuiteFinder { get; set; }
        internal ConnectionStringsFinder ConnectionStringsFinder { get; set; }
        internal ConfigurationFinder ConfigurationFinder { get; set; }

        public virtual void ExecuteSetUpFixture()
        {
            Console.Out.WriteLine(string.Format("Test suite loaded from {0}", GetOwnFilename()));
            Console.Out.WriteLine(string.Format("Test suite defined in {0}", TestSuiteFinder.Find()));

            TestSuiteManager.Load(TestSuiteFinder.Find());

            //Find configuration of NBi
            if (ConfigurationFinder != null)
                ApplyConfig(ConfigurationFinder.Find());

            //Find connection strings referenced from an external file
            if (ConnectionStringsFinder != null)
                TestSuiteManager.ConnectionStrings = ConnectionStringsFinder.Find();

        }


        public void ApplyConfig(NBiSection config)
        {
            EnableAutoCategories = config.EnableAutoCategories;
        }


        protected internal string GetOwnFilename()
        {
            //get the full location of the assembly with DaoTests in it
            var fullPath = System.Reflection.Assembly.GetAssembly(typeof(TestSuite)).Location;

            //get the filename that's in
            var fileName = Path.GetFileName(fullPath);

            return fileName;
        }

        protected internal string GetManifestName()
        {
            //get the full location of the assembly with DaoTests in it
            var fullName = System.Reflection.Assembly.GetAssembly(typeof(TestSuite)).ManifestModule.Name;

            return fullName;
        }
    }
}
