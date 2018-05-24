using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Ranking
{
    public class BaseRankXml
    {
        [XmlAttribute("count")]
        [DefaultValue(1)]
        public int Count { get; set; }

        public BaseRankXml()
        {
            Count = 1;
        }
    }

    public class TopRankingXml : BaseRankXml { }
    public class BottomRankingXml : BaseRankXml { }
}
