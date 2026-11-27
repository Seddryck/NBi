using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Format;
using NBi.Model.Settings;

namespace NBi.Model.Items.Format;

public class CurrencyFormat : NumericFormat, ICurrencyFormat
{
    private string currencySymbol;
    public string CurrencySymbol
    {
        get { return currencySymbol; }
        set { currencySymbol = value; isEmpty = false; }
    }

    private CurrencyPattern currencyPattern;
    public CurrencyPattern CurrencyPattern
    {
        get { return currencyPattern; }
        set { currencyPattern = value; isEmpty = false; }
    }

    public CurrencyFormat() : base()
    {
    }

    protected override void InitializeFromReferences(IEnumerable<Reference> references, string value)
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


    public CurrencyFormat(bool isEmpty)
        : base(isEmpty)
    {
    }

}
