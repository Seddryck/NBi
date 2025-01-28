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

namespace NBi.Core.Testing.FlatFile;

public class FlatFileReaderFactoryTest
{
    #region Fake
    internal class FakeFlatFileReader : IFlatFileReader
    {
        public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

        public DataTable ToDataTable(string filename) => throw new NotImplementedException();
    }

    internal class FakeFlatFileReader2 : IFlatFileReader
    {
        public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

        public DataTable ToDataTable(string filename) => throw new NotImplementedException();
    }

    internal class FakeFlatFileReader3 : IFlatFileReader
    {
        public event ProgressStatusHandler ProgressStatusChanged { add { } remove { } }

        public DataTable ToDataTable(string filename) => throw new NotImplementedException();
    }

    internal class FakeFlatFileReaderWrong : IFlatFileReader
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
        Assert.That(engine, Is.InstanceOf<CsvReader>());
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
        var engine = factory.Instantiate("fake", CsvProfile.SemiColumnDoubleQuote);
        Assert.That(engine, Is.InstanceOf<FakeFlatFileReader>());
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
        Assert.That(factory.Instantiate("correct", CsvProfile.SemiColumnDoubleQuote), Is.InstanceOf<FakeFlatFileReader2>());
        Assert.That(factory.Instantiate("fake", CsvProfile.SemiColumnDoubleQuote), Is.InstanceOf<FakeFlatFileReader>());
        Assert.That(factory.Instantiate("other", CsvProfile.SemiColumnDoubleQuote), Is.InstanceOf<FakeFlatFileReader3>());
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
        var engine = factory.Instantiate("fake", CsvProfile.SemiColumnDoubleQuote);
        var engine2 = factory.Instantiate("fake", CsvProfile.SemiColumnDoubleQuote);

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
        var engine = factory.Instantiate("fake", CsvProfile.SemiColumnDoubleQuote);

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
    }
}
