using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Constraints;
using NBi.Core.ResultSet;
using NBi.Xml.Items.ResultSet;
using System.IO;
using NBi.Xml.Items.Alteration;
using NBi.Xml.Items.ResultSet.Combination;
using System;
using NBi.Xml.Items.Alteration.Renaming;
using NBi.Xml.Items.Alteration.Extension;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Alteration.Summarization;
using NBi.Xml.Items.Alteration.Reshaping;
using NBi.Xml.Items.Alteration.Projection;
using NBi.Xml.Items.Alteration.Lookup;
using NBi.Xml.Variables.Sequence;
using NBi.Xml.Items.Hierarchical.Xml;
using NBi.Xml.Items.Hierarchical.Json;
using NBi.Xml.Items.Alteration.Merging;
using NBi.Xml.Items.Alteration.Duplication;

namespace NBi.Xml.Systems
{
    public class ResultSetSystemXml : AbstractSystemUnderTestXml
    {
        [XmlElement("row")]
        public List<RowXml> _rows { get; set; } = [];

        public IList<IRow> Rows
            => _rows.Cast<IRow>().ToList();

        public IList<string> Columns
        {
            get
            {
                if (_rows.Count == 0)
                    return new List<string>();

                var names = new List<string>();
                var row = _rows[0];

                foreach (var cell in row.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.ColumnName))
                        names.Add(cell.ColumnName);
                    else
                        names.Add(string.Empty);
                }
                return names;
            }
        }

        [XmlIgnore]
        public IContent Content
            => new Content(Rows, Columns);

        [XmlAttribute("path")]
        [Obsolete("Use File in place of FileAttribute")]
        public virtual string FilePath
        {
            get => File.Path + (File.IsBasic() ? string.Empty : $"!{File.Parser!.Name}");
            set
            {
                var tokens = value.Split(new[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
                File.Path = tokens[0];
                if (tokens.Length > 1)
                    File.Parser = new ParserXml() { Name = tokens[1] };
                else
                    File.Parser = null;
            }
        }

        [XmlElement("iteration")]
        public virtual IterationXml? Iteration { get; set; } = null;

        [XmlElement("result-set")]
        public virtual ResultSetSystemXml? NestedResultSet { get; set; } = null;

        [XmlElement("file")]
        public virtual FileXml File { get; set; } = new FileXml();

        public bool ShouldSerializeFilePath() => File.IsBasic() && !File.IsEmpty();
        public bool ShouldSerializeFile() => !File.IsBasic() || !File.IsEmpty();

        public override BaseItem? BaseItem
            => Query;

        [XmlElement("query")]
        public virtual QueryXml? Query { get; set; }

        [XmlElement("sequences-combination")]
        public virtual SequenceCombinationXml? SequenceCombination { get; set; }

        [XmlElement("sequence")]
        public virtual SequenceXml? Sequence { get; set; }

        [XmlElement("xml-source")]
        public virtual XmlSourceXml? XmlSource { get; set; }

        [XmlElement("json-source")]
        public virtual JsonSourceXml? JsonSource { get; set; }

        [XmlElement("empty")]
        public virtual EmptyResultSetXml? Empty { get; set; }

        [XmlIgnore]
        public bool SequenceCombinationSpecified { get => SequenceCombination != null; set { } }

        [XmlArray("alteration"),
            XmlArrayItem(Type = typeof(RenamingXml), ElementName = "rename"),
            XmlArrayItem(Type = typeof(ExtendXml), ElementName = "extend"),
            XmlArrayItem(Type = typeof(FilterXml), ElementName = "filter"),
            XmlArrayItem(Type = typeof(ConvertXml), ElementName = "convert"),
            XmlArrayItem(Type = typeof(TransformXml), ElementName = "transform"),
            XmlArrayItem(Type = typeof(SummarizeXml), ElementName = "summarize"),
            XmlArrayItem(Type = typeof(UnstackXml), ElementName = "unstack"),
            XmlArrayItem(Type = typeof(ProjectXml), ElementName = "project"),
            XmlArrayItem(Type = typeof(ProjectAwayXml), ElementName = "project-away"),
            XmlArrayItem(Type = typeof(LookupReplaceXml), ElementName = "lookup-replace"),
            XmlArrayItem(Type = typeof(MergeXml), ElementName = "merge"),
            XmlArrayItem(Type = typeof(UnionXml), ElementName = "union"),
            XmlArrayItem(Type = typeof(DuplicateXml), ElementName = "duplicate"),
        ]
        public virtual List<AlterationXml> Alterations { get; set; } = [];

        [XmlIgnore]
        public bool AlterationsSpecified
        {
            get => Alterations.Count > 0;
            set {}
        }

        [XmlElement("if-unavailable")]
        public virtual IfUnavailableXml? IfUnavailable { get; set; }

        public override ICollection<string> GetAutoCategories()
        {
            return new List<string>() { "Result-set" };
        }

        public ResultSetSystemXml()
        {
            _rows = new List<RowXml>();
        }
    }

    public class ResultSetSystemOldXml : ResultSetSystemXml { }
}
