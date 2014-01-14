﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Format;
using NBi.Xml.Settings;

namespace NBi.Xml.Items.Format
{
    public class NumericFormatXml : INumericFormat
    {
        private readonly bool isEmpty = false;

        public bool IsEmpty 
        {
            get{ return isEmpty;}
        }

        [XmlAttribute("ref")]
        public string Reference {get; set;}

        internal void AssignReferences(IEnumerable<ReferenceXml> references)
        {
            if (!string.IsNullOrEmpty(Reference))
                InitializeFromReferences(references, Reference);
        }

        protected virtual void InitializeFromReferences(IEnumerable<ReferenceXml> references, string value)
        {
            var refChoice = GetReference(references, value);

            if (refChoice.NumericFormat==null)
                throw new NullReferenceException(string.Format("A reference named '{0}' has been defined, but it's numeric-format is not defined", value));

            Initialize(refChoice.NumericFormat);
        }

        protected void Initialize(INumericFormat format)
        {            
            DecimalDigits = format.DecimalDigits;
            DecimalSeparator = format.DecimalSeparator;
            GroupSeparator = format.GroupSeparator;
        }

        protected ReferenceXml GetReference(IEnumerable<ReferenceXml> references, string value)
        {
            if (references == null || references.Count() == 0)
                throw new InvalidOperationException("No reference has been defined for this constraint");

            var refChoice = references.FirstOrDefault(r => r.Name == value);
            if (refChoice == null)
                throw new IndexOutOfRangeException(string.Format("No reference named '{0}' has been defined.", value));
            return refChoice;
        }
        
        [XmlAttribute("decimal-digits")]
        public int DecimalDigits { get; set; }

        [XmlAttribute("decimal-separator")]
        [DefaultValue(".")]
        public string DecimalSeparator { get; set; }

        [XmlAttribute("group-separator")]
        [DefaultValue(",")]
        public string GroupSeparator { get; set; }

        public NumericFormatXml() : base()
        {
            DecimalSeparator = ".";
            GroupSeparator = ",";
        }

        public NumericFormatXml(bool isEmpty)
            : base()
        {
            this.isEmpty = isEmpty;
        }

    }
}
