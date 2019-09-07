using NBi.GenbiL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.GenbiL
{
    public class TestSuiteGeneratorTest
    {
        [Test]
        public void Load_FileWithEuro_EuroCorrectlyRead()
        {
            var filename = DiskOnFile.CreatePhysicalFile("Simple.genbil", "NBi.Testing.Integration.GenbiL.Resources.Simple.genbil");

            var generator = new TestSuiteGenerator();
            generator.Load(filename);
            Assert.That(generator.Text, Does.Contain("€"));
        }
    }
}
