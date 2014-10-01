using Moq;
using NBi.Service.RunnerConfig;
using NUnit.Framework;

namespace NBi.Testing.Unit.Service.RunnerConfig
{
    [TestFixture]
    public class RunnerConfigManagerTest
    {
        
        [Test]
        public void Build_YesNUnitNoGallio_OnlyNUnitBuilderCalled()
        {
            var nunitBuilderMockFactory = new Mock<IRunnerConfigBuilder>();
            nunitBuilderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            var nunit = nunitBuilderMockFactory.Object;

            var gallioBuilderMockFactory = new Mock<IRunnerConfigBuilder>();
            gallioBuilderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));
            var gallio = gallioBuilderMockFactory.Object;

            var manager = new RunnerConfigManager(nunit, gallio);

            manager.Build(@"C:\", @"C:\Framework", @"C:\TestSuite", true, false);

            nunitBuilderMockFactory.Verify(builder => builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once());

            gallioBuilderMockFactory.Verify(builder => builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Never());
        }

        [Test]
        public void Build_NoNUnitYesGallio_OnlyGallioBuilderCalled()
        {
            var nunitBuilderMockFactory = new Mock<IRunnerConfigBuilder>();
            nunitBuilderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            var nunit = nunitBuilderMockFactory.Object;

            var gallioBuilderMockFactory = new Mock<IRunnerConfigBuilder>();
            gallioBuilderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));
            var gallio = gallioBuilderMockFactory.Object;

            var manager = new RunnerConfigManager(nunit, gallio);

            manager.Build(@"C:\", @"C:\Framework", @"C:\TestSuite", false, true);

            nunitBuilderMockFactory.Verify(builder => builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Never());

            gallioBuilderMockFactory.Verify(builder => builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once());
        }

        [Test]
        public void Build_YesNUnitYesGallio_BothBuildersCalled()
        {
            var nunitBuilderMockFactory = new Mock<IRunnerConfigBuilder>();
            nunitBuilderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            var nunit = nunitBuilderMockFactory.Object;

            var gallioBuilderMockFactory = new Mock<IRunnerConfigBuilder>();
            gallioBuilderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));
            var gallio = gallioBuilderMockFactory.Object;

            var manager = new RunnerConfigManager(nunit, gallio);

            manager.Build(@"C:\", @"C:\Framework", @"C:\TestSuite", true, true);

            nunitBuilderMockFactory.Verify(builder => builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once());

            gallioBuilderMockFactory.Verify(builder => builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once());
        }

        [Test]
        public void Build_ValidParameters_CorrectParametersTransferedToBuilder()
        {
            var builderMockFactory = new Mock<IRunnerConfigBuilder>();
            builderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            var nunit = builderMockFactory.Object;
            var gallio = builderMockFactory.Object;

            var manager = new RunnerConfigManager(nunit, gallio);

            manager.Build(@"C:\", @"C:\Framework", @"C:\TestSuite\ts.nbits", true, true);

            builderMockFactory.Verify(builder => builder.Build(
                    @"C:\",
                    @"..\",
                    @"Framework\",
                    @"TestSuite\",
                    "ts"));
        }

        [Test]
        public void Build_ValidParametersWithSubPath_CorrectParametersTransferedToBuilder()
        {
            var builderMockFactory = new Mock<IRunnerConfigBuilder>();
            builderMockFactory.Setup(builder =>
                builder.Build(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            var nunit = builderMockFactory.Object;
            var gallio = builderMockFactory.Object;

            var manager = new RunnerConfigManager(nunit, gallio);

            manager.Build(@"C:\QA\", @"C:\QA\Framework\Version", @"C:\QA\TestSuites\Serie\Alpha\ts.nbits", true, true);

            builderMockFactory.Verify(builder => builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts"));
        }

        public void Build_Parameters_NeverCopyDll()
        {
            var filePersisterMockFactory = new Mock<IFilePersister>();
            //Project file
            filePersisterMockFactory.Setup(fp => fp.Copy(
                It.IsAny<string>()
                , It.IsAny<string>()
                ));


            var filePersister = filePersisterMockFactory.Object;


            var builder = new NUnitRunnerConfigBuilder(filePersister);
            builder.Build(
                    @"C:\QA\",
                    @"..\..\..\",
                    @"Framework\Version\",
                    @"TestSuites\Serie\Alpha\",
                    "ts");

            filePersisterMockFactory.Verify(fp => fp.Copy(
                It.IsAny<string>()
                , It.IsAny<string>()
                ), Times.Never());
        }

    }
}
