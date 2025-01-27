using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation;

public enum ComparerType
{
    LessThan = -2,
    LessThanOrEqual = -1,
    Equal = 0,
    MoreThanOrEqual = 1,
    MoreThan = 2,
    Null = 10,
    Empty = 11,
    NullOrEmpty = 12,
    StartsWith = 20,
    EndsWith = 21,
    Contains = 22,
    MatchesRegex = 23,
    MatchesNumeric = 25,
    MatchesDate = 26,
    MatchesTime = 27,
    MatchesDateTime = 28,
    UpperCase = 30,
    LowerCase = 31,
    WithinRange = 40,
    AnyOf = 41,
    Integer = 50,
    Modulo = 51,
    OnTheDay = 60,
    OnTheHour = 61,
    OnTheMinute = 62,
    True = 70,
    False = 71,

}
