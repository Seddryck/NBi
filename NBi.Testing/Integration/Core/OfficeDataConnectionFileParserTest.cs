using NBi.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core
{
    [TestFixture]
    public class OfficeDataConnectionFileParserTest
    {
        private const string ODC_FILE = "Sample.odc";

        [Test]
        public void GetConnectionString_ValidRelativeFile_CorrectConnectionString()
        {
            var fullPath = DiskOnFile.CreatePhysicalFile(ODC_FILE, "NBi.Testing.Integration.Core.Resources." + ODC_FILE);
            var parser = new OfficeDataConnectionFileParser(Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar);
            var connectionString = parser.GetConnectionString(ODC_FILE);
            Assert.That(connectionString, Is.EqualTo("Provider=MSOLAP.7;Integrated Security=ClaimsToken;Identity Provider=AAD;Data Source=https://analysis.windows.net/powerbi/api;;Initial Catalog=sobe_wowvirtualserver-ccdf3d76-59d9-4e10-83e8-82eb0d27d1e9;Location=https://wabi-north-europe-redirect.analysis.windows.net/xmla?vs=sobe_wowvirtualserver&db=ccdf3d76-59d9-4e10-83e8-82eb0d27d1e9;MDX Compatibility= 1; MDX Missing Member Mode= Error; Safety Options= 2; Update Isolation Level= 2"));
        }

        [Test]
        public void GetConnectionString_ValidAbsoluteFile_CorrectConnectionString()
        {
            var fullPath = DiskOnFile.CreatePhysicalFile(ODC_FILE, "NBi.Testing.Integration.Core.Resources." + ODC_FILE);
            var parser = new OfficeDataConnectionFileParser(Path.GetDirectoryName(fullPath));
            var connectionString = parser.GetConnectionString(fullPath);
            Assert.That(connectionString, Is.EqualTo("Provider=MSOLAP.7;Integrated Security=ClaimsToken;Identity Provider=AAD;Data Source=https://analysis.windows.net/powerbi/api;;Initial Catalog=sobe_wowvirtualserver-ccdf3d76-59d9-4e10-83e8-82eb0d27d1e9;Location=https://wabi-north-europe-redirect.analysis.windows.net/xmla?vs=sobe_wowvirtualserver&db=ccdf3d76-59d9-4e10-83e8-82eb0d27d1e9;MDX Compatibility= 1; MDX Missing Member Mode= Error; Safety Options= 2; Update Isolation Level= 2"));
        }

        [Test]
        public void GetConnectionString_InvalidFile_FileNotFoundException()
        {
            var fullPath = DiskOnFile.CreatePhysicalFile(ODC_FILE, "NBi.Testing.Integration.Core.Resources." + ODC_FILE);
            var parser = new OfficeDataConnectionFileParser(Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar);
            Assert.Throws<FileNotFoundException>(delegate { parser.GetConnectionString("NonExisting.odc"); });
        }
    }
}
