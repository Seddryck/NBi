using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Query;
using NBi.Xml.Items;

namespace NBi.Xml.Constraints
{
    public class ContainsXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        public bool IgnoreCase { get; set; }

        [XmlAttribute("caption")]
        public string Caption { get; set; }

        [XmlElement("item")]
        public List<string> ItemList { get; set; }

        protected internal List<string> Items { get; set; }

        [XmlElement("one-column-query")]
        public QueryXml Query { get; set; }


        public ContainsXml()
        {
            Items = new List<string>();
            Specification = new SpecificationContains();
        }

        protected string displayFolder;
        [XmlAttribute("display-folder")]
        public string DisplayFolder
        {
            get
            { return displayFolder; }

            set
            {
                displayFolder = value;
                Specification.IsDisplayFolderSpecified = true;
            }
        }

            
        [XmlIgnore()]
        public SpecificationContains Specification { get; protected set; }


        public class SpecificationContains
        {
            public bool IsDisplayFolderSpecified { get; internal set; }
        }

        public override void Initialize()
        {
            if (ItemList.Count == 0 && !string.IsNullOrEmpty(Caption))
                Items.Add(Caption);

            if (ItemList.Count > 0)
                Items.AddRange(ItemList);

            if (GetCommand() != null)
            {
                var listBuilder = new ListBuilder();
                Items.AddRange(listBuilder.Build(GetCommand()));
            }

            base.Initialize();
        }

        public IDbCommand GetCommand()
        {
            if (Query == null)
                return null;

            var conn = ConnectionFactory.Instance.Get(Query.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = Query.GetQuery();

            return cmd;
        }
    }
}
