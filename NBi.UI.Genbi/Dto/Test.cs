using System;
using System.Linq;
using NBi.Xml;

namespace NBi.IO.Genbi.Dto
{
    public class Test
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public TestXml Reference { get; set; }
    }
}
