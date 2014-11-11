﻿using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Substitutions
{
    public class StubXml : AbstractSubstitutionXml
    {
        [XmlElement("return")]
        public ReturnXml Return { get; set; }
    }
}
