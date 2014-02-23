using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Service.RunnerConfig
{
    internal class FilePersister : IFilePersister
    {
        public void Copy(string source, string destination)
        {
            File.Copy(source, destination);
        }

        public void Save(string fullPath, string content)
        {
            File.WriteAllText(fullPath, content, Encoding.UTF8);
        }
    }
}
