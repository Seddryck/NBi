using NBi.Core.Analysis.Process.Agent;
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
            var agent = new SamoSsasAgent(cubeProcess);
            return agent;
        }
    }
}
