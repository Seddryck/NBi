using Moq;
using NBi.Service.RunnerConfig;
using NUnit.Framework;

namespace NBi.Testing.Unit.Service.RunnerConfig
{
    [TestFixture]
    public class GallioRunnerConfigBuilderTest
    {
        
        [Test]
        public void Build_Parameters_CorrectConfigFullPath()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            filePersisterMockFactory.Setup(fp => fp.Save(It.IsAny<string>(), It.IsAny<string>()));
            var filePersister = filePersisterMockFactory.Object;


            var builder = new GallioRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            Assert.That(builder.ConfigFullPath, Is.EqualTo(@"C:\QA\ts.NBi.NUnit.Runtime.dll.config"));
        }

        [Test]
        public void Build_Parameters_CorrectProjectFullPath()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            filePersisterMockFactory.Setup(fp => fp.Save(It.IsAny<string>(), It.IsAny<string>()));
            var filePersister = filePersisterMockFactory.Object;


            var builder = new GallioRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            Assert.That(builder.RunnerProjectFullPath, Is.EqualTo(@"C:\QA\TestSuites\Serie\Alpha\ts.gallio"));
        }

        [Test]
        public void Build_Parameters_PersistTwoFiles()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            filePersisterMockFactory.Setup(fp => fp.Save(It.IsAny<string>(), It.IsAny<string>()));
            var filePersister = filePersisterMockFactory.Object;


            var builder = new NUnitRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            filePersisterMockFactory.Verify(fp => fp.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Build_Parameters_ContenstOfFilesAreCorrect()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            //Project file
            filePersisterMockFactory.Setup(fp => fp.Save(It.IsAny<string>()
                , It.Is<string>(content => 
                    content.Contains("<testProject")
                    && content.Contains("applicationBaseDirectory=\"..\\..\\..\\\"")
                    && content.Contains(@"<file>..\..\..\ts.NBi.NUnit.Runtime.dll</file>")
                    && content.Contains(@"<hintDirectory>..\..\..\Framework\Version\</hintDirectory>")
                    )
                ));
            //
            filePersisterMockFactory.Setup(fp => fp.Save(It.IsAny<string>()
                , It.Is<string>(content =>
                    content.Contains("<nbi testSuite=\"TestSuites\\Serie\\Alpha\\ts.nbits\"/>")
                    )
                ));

            var filePersister = filePersisterMockFactory.Object;


            var builder = new GallioRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            filePersisterMockFactory.VerifyAll();
        }

        public void Build_Parameters_CopyDllOnce()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            //Project file
            filePersisterMockFactory.Setup(fp => fp.Copy(
                It.IsAny<string>()
                , It.IsAny<string>()
                ));
            

            var filePersister = filePersisterMockFactory.Object;


            var builder = new GallioRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            filePersisterMockFactory.Verify(fp => fp.Copy(
                It.IsAny<string>()
                , It.IsAny<string>()
                ), Times.Once());
        }

        public void Build_Parameters_CopyCorrectDllToCorrectLocation()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            //Project file
            filePersisterMockFactory.Setup(fp => fp.Copy(
                @"C:\QA\Framework\Version\NBi.NUnit.Runtime.dll"
                , @"C:\QA\ts.NBi.NUnit.Runtime.dll"
                ));


            var filePersister = filePersisterMockFactory.Object;


            var builder = new GallioRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            filePersisterMockFactory.VerifyAll();
        }
    }
}
