using System;
using System.IO;
using System.Linq;

namespace NBi.Service.RunnerConfig
{
    internal class GallioRunnerConfigBuilder : AbstractRunnerConfigBuilder
    {

        public GallioRunnerConfigBuilder()
            : base("NBi.config", "Gallio.gallio")
        {

        }

        public GallioRunnerConfigBuilder(IFilePersister filePersister)
            : this()
        {
            base.filePersister = filePersister;
        }

        protected override string CalculateRunnerProjectFullPath()
        {
            return base.CalculateRunnerProjectFullPath() + ".gallio";
        }

        protected override string CalculateConfigFullPath()
        {
            return BasePath + Filename + ".NBi.NUnit.Runtime.dll.config";
        }

        protected override void CopyRuntimeFile()
        {
            var source =  BasePath + Framework + "NBi.NUnit.Runtime.dll";
            var destination =  BasePath + Filename + ".NBi.NUnit.Runtime.dll";

            filePersister.Copy(source, destination);
        }
    }
}
