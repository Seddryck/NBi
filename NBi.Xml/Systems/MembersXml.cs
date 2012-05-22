using System;
using System.Xml.Serialization;
using NBi.Core.Analysis.Member;

namespace NBi.Xml.Systems
{
    public class MembersXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlAttribute("hierarchy")]
        public string Hierarchy { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override object Instantiate()
        {
            var cmd = new AdomdMemberCommand(ConnectionString);

            if (!string.IsNullOrEmpty(Level))
            {
                cmd.PlaceHolder = AdomdMemberCommand.PlaceHolderType.Level;
                cmd.PlaceHolderUniqueName = Level;
            }
            else if (!string.IsNullOrEmpty(Hierarchy))
            {
                cmd.PlaceHolder = AdomdMemberCommand.PlaceHolderType.Hierarchy;
                cmd.PlaceHolderUniqueName = Hierarchy;
            }
            else
                throw new Exception();

            cmd.Perspective = Perspective;

            return cmd;
        }
    }
}
