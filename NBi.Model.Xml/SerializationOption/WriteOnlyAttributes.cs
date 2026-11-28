using NBi.Model.Xml.Constraints.Comparer;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Model.Xml.SerializationOption;

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
