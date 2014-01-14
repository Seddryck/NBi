﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Format;
using NBi.Xml.Settings;

namespace NBi.Xml.Items.Format
{
    public class CurrencyFormatXml : NumericFormatXml, ICurrencyFormat
    {
        [XmlAttribute("currency-symbol")]
        public string CurrencySymbol { get; set; }

        [XmlAttribute("currency-pattern")]
        public CurrencyPattern CurrencyPattern { get; set; }

        public CurrencyFormatXml() : base()
        {
        }

        protected override void InitializeFromReferences(IEnumerable<ReferenceXml> references, string value)
        {
            var refChoice = GetReference(references, value);

            if (refChoice.CurrencyFormat == null)
                throw new NullReferenceException(string.Format("A reference named '{0}' has been defined, but it's currency-format is not defined", value));

            Initialize(refChoice.CurrencyFormat);
        }

        protected void Initialize(ICurrencyFormat currencyFormat)
        {
            Initialize((INumericFormat)currencyFormat);
            CurrencyPattern = currencyFormat.CurrencyPattern;
            CurrencySymbol = currencyFormat.CurrencySymbol;
        }


        public CurrencyFormatXml(bool isEmpty)
            : base(isEmpty)
        {
        }

    }
}
