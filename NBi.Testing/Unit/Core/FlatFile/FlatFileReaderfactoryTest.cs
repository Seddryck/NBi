using NBi.Core.FlatFile;
using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.FlatFile
{
    public class FlatFileReaderFactoryTest
    {
        #region Fake
        public class FakeFlatFileReader : IFlatFileReader
        {
            public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

            public DataTable ToDataTable(string filename) => throw new NotImplementedException();
        }

        public class FakeFlatFileReader2 : IFlatFileReader
        {
            public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

            public DataTable ToDataTable(string filename) => throw new NotImplementedException();
        }

        public class FakeFlatFileReader3 : IFlatFileReader
        {
            public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

            public DataTable ToDataTable(string filename) => throw new NotImplementedException();
        }

        public class FakeFlatFileReaderWrong : IFlatFileReader
        {
            public FakeFlatFileReaderWrong(string whatsup)
                : base() { }

            public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

            public DataTable ToDataTable(string filename) => throw new NotImplementedException();
        }
        #endregion

        [Test]
        public void Instantiate_OneExtensionNoPattern_BasicReader()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReader), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileReaderFactory();
            var engine = factory.Instantiate(string.Empty, CsvProfile.SemiColumnDoubleQuote);
            Assert.IsInstanceOf<CsvReader>(engine);
        }

        [Test]
        public void Instantiate_OneExtension_ThisExtensionIsReturned()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReader), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileReaderFactory();
            var engine = factory.Instantiate("fake", null);
            Assert.IsInstanceOf<FakeFlatFileReader>(engine);
        }

        [Test]
        public void Instantiate_ThreeExtensions_CorrectExtensionLoaded()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReader), new Dictionary<string, string>() { { "extension", "fake" } } },
                { typeof(FakeFlatFileReader2), new Dictionary<string, string>() { { "extension", "correct" } } },
                { typeof(FakeFlatFileReader3), new Dictionary<string, string>() { { "extension", "other" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileReaderFactory();
            Assert.IsInstanceOf<FakeFlatFileReader2>(factory.Instantiate("correct", null));
            Assert.IsInstanceOf<FakeFlatFileReader>(factory.Instantiate("fake", null));
            Assert.IsInstanceOf<FakeFlatFileReader3>(factory.Instantiate("other", null));
        }

        [Test]
        public void Instantiate_Extension_MultipleInstanceAreDifferent()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReader), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileReaderFactory();
            var engine = factory.Instantiate("fake", null);
            var engine2 = factory.Instantiate("fake", null);

            Assert.That(engine, Is.Not.EqualTo(engine2));
        }

        [Test]
        public void Instantiate_ExtensionsAdditionalParameters_ExtensionReturned()
        {

            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                {
                    typeof(FakeFlatFileReader),
                    new Dictionary<string, string>()
                    {
                        { "extension", "fake" },
                        { "multiLines", "true" },
                    }
                },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileReaderFactory();
            var engine = factory.Instantiate("fake", null);

            Assert.That(engine, Is.Not.Null);
        }

        [Test]
        public void Instantiate_ExtensionsRegisteringSameExtensions_ExceptionThrown()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReader), new Dictionary<string, string>() { {  "extension", "fake" } } },
                { typeof(FakeFlatFileReader2), new Dictionary<string, string>() { { "extension", "fake" } } },
                { typeof(FakeFlatFileReader3), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);
            var ex = Assert.Throws<ArgumentException>(() => localServiceLocator.GetFlatFileReaderFactory());
            Console.WriteLine(ex.Message);
        }

        [Test]
        public void Instantiate_ExtensionRegisteringSameExtensions_ExceptionThrown()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReader), new Dictionary<string, string>() { {  "extension", "fake" } } },
                { typeof(FakeFlatFileReader2), new Dictionary<string, string>() { { "extension", "fake" } } },
                { typeof(FakeFlatFileReader3), new Dictionary<string, string>() { { "extension", "none" } } },
            };
            config.LoadExtensions(extensions);
            var ex = Assert.Throws<ArgumentException>(() => localServiceLocator.GetFlatFileReaderFactory());
            Console.WriteLine(ex.Message);
        }

        [Test]
        public void Instantiate_InvalidConstructorAvailable_ExceptionThrown()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileReaderWrong), new Dictionary<string, string>() { {  "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);
            var ex = Assert.Throws<ArgumentException>(() => localServiceLocator.GetFlatFileReaderFactory());
            Console.WriteLine(ex.Message);
        }
    }
}
