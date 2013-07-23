﻿using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Constraints;

namespace NBi.Xml
{
    internal class SpecificReadAttributes : XmlAttributeOverrides
    {

        public SpecificReadAttributes()
            : base()
        {
        }

        public void Build()
        {

            XmlAttributes attrs = null;

            attrs = new XmlAttributes();
            attrs.XmlAttribute = new XmlAttributeAttribute("description");
            Add(typeof(TestXml), "Description", attrs);

            attrs = new XmlAttributes();
            attrs.XmlAttribute = new XmlAttributeAttribute("ignore");
            Add(typeof(TestXml), "Ignore", attrs);

            attrs = new XmlAttributes();
            attrs.XmlAttribute = new XmlAttributeAttribute("caption");
            Add(typeof(ContainXml), "Caption", attrs);
        }
    }
}