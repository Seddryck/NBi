using System;
using System.IO;
using System.Linq;

namespace NBi.Service
{
    public class TemplateManager
    {
        public TemplateManager()
        {

        }

        public void Persist(string filename, string content)
        {
            using (TextWriter tw = new StreamWriter(filename))
            {
                tw.Write(content);
            }
        }
    }
}
