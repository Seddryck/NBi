using System;
using NBi.Core.Integration;
using NUnit.Framework;

namespace NBi.Testing.Core.Integration
{
    [TestFixture]
    [Ignore]
    public class PackageManagerTest
    {
        [Test]
        public void Load_ExistingPackage_Sucessful()
        {
            var pm = new PackageManager();
            pm.Load(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample.dtsx");
            Assert.That(pm.OriginalPackage, Is.Not.Null);
        }

        [Test]
        public void Load_NotExistingPackage_Failed()
        {
            var pm = new PackageManager();

            Assert.Throws<ArgumentException>(delegate { pm.Load(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\NotExisting.dtsx"); });
        }

        [Test]
        public void Find_ExistingTask_Sucessful()
        {
            var pm = new PackageManager();
            pm.Load(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample.dtsx");
            var found = pm.Contains("My DFT");

            Assert.That(found, Is.True);
        }

        [Test]
        public void Find_NotExistingTask_Sucessful()
        {
            var pm = new PackageManager();
            pm.Load(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample.dtsx");
            var found = pm.Contains("My DFT Non Existing");

            Assert.That(found, Is.False);
        }

        [Test]
        public void AddDataReaderSource_Null_Sucessful()
        {
            var pm = new PackageManager();
            pm.Load(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample.dtsx");
            var dft = pm.GetDataFlowTask("My DFT");
            dft.AddDataReaderDestination();
            pm.Save(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample-test.dtsx");

            //Assert.That(found, Is.False);
        }

        [Test]
        public void ExecuteRead_Null_Sucessful()
        {
            var pm = new PackageManager();
            //pm.Load(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample.dtsx");
            //var dft = pm.GetDataFlowTask("My DFT");
            //dft.AddDataReaderDestination();
            //pm.Save(@"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\bin\FirstSample-test.dtsx");

            //Thread thread = new Thread(new ThreadStart(delegate { pm.Execute(); }));
            //thread.Start();

            //Thread.Sleep(5000);

            pm.Read();
        }
    }
}
