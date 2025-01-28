using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

abstract public class AbstractItem : BaseItem
{
    [XmlAttribute("caption")]
    public string Caption { get; set; }

    public abstract string TypeName {get;}

    internal virtual Dictionary<string, string> GetRegexMatch()
    {
        var dico = new Dictionary<string, string>();
        dico.Add("sut:caption", Caption);
        dico.Add("sut:typeName", TypeName);
        return dico;
    }

    internal abstract ICollection<string> GetAutoCategories();
}
