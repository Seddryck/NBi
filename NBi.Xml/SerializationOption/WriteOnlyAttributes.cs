using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.SerializationOption;

public class WriteOnlyAttributes : ReadWriteAttributes
{

    public WriteOnlyAttributes()
        : base() { }

    protected override void AdditionalBuild()
    {
        AddAsIgnore((QueryXml x) => x.InlineQuery, true);
        AddAsAnyNotIgnore((QueryXml x) => x.InlineQueryWrite);

        AddAsAnyNotIgnore((MatchesRegexXml x) => x.ValueWrite);
    }
}
