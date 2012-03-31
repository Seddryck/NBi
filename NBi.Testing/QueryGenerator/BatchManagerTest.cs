using NUnit.Framework;
using System.IO;
using NBi.QueryGenerator;

namespace NBi.Testing.QueryGenerator
{
    [TestFixture]
    public class BatchManagerTest
    {
        [Test]
        public void Run_Directory_HasResults()
        {
            if (Directory.Exists(@"C:\Users\Seddryck\Documents\TestCCH\Results\"))
                Directory.Delete(@"C:\Users\Seddryck\Documents\TestCCH\Results\", true);

            var bm = new BatchManager();
            bm.CreateResultSet(@"C:\Users\Seddryck\Documents\TestCCH\", @"C:\Users\Seddryck\Documents\TestCCH\Results\", "Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");

            var resCount = Directory.GetFiles(@"C:\Users\Seddryck\Documents\TestCCH\Results\").Length;
            var qryCount = Directory.GetFiles(@"C:\Users\Seddryck\Documents\TestCCH\").Length;

            Assert.AreEqual(qryCount, resCount);
        }
    }
}
