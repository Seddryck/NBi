using System;
using System.Linq;

namespace NBi.Core.Format;

public interface ICurrencyFormat : INumericFormat
{
    string CurrencySymbol { get; set; }

    CurrencyPattern CurrencyPattern { get; set; }
}
