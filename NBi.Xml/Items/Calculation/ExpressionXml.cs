using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation
{
    [XmlType("")]
    public class ExpressionXml: IColumnExpression
    {
        [XmlText()]
        public string Value
        {
            get => ShouldSerializeValue() ? Script.Code : null;
            set => Script = new ScriptXml() { Language = LanguageType.NCalc, Code = value };
        }

        public bool ShouldSerializeValue() => Script?.Language == LanguageType.NCalc;

        [XmlElement("script")]
        public ScriptXml Script { get; set; }

        public bool ShouldSerializeScript() => Script?.Language != LanguageType.NCalc;

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("column-index")]
        [DefaultValue(0)]
        public int Column { get; set; }

        [XmlAttribute("type")]
        [DefaultValue(ColumnType.Text)]
        public ColumnType Type { get; set; }

        [XmlAttribute("tolerance")]
        [DefaultValue("")]
        public string Tolerance { get; set; }
    }
}
