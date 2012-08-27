using System;
using System.Collections;

namespace NBi.Core.Analysis.Metadata
{
    public interface IFieldWithDisplayFolder : IField
    {
        string DisplayFolder { get; set; }
    }
}
