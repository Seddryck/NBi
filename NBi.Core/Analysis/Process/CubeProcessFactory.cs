using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Process
{
    public class CubeProcessFactory
    {
        public ICubeProcessor Get(ICubeProcess cubeProcess)
        {
            var factory = new SsasProcessorFactory();
            var runner = factory.Get(etl);
            return runner;
        }
    }
}
