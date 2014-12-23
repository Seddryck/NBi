using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Service.RunnerConfig
{
    public interface IFilePersister
    {
        void Save(string fullPath, string content);
        void Copy(string source, string destination);
    }
}
