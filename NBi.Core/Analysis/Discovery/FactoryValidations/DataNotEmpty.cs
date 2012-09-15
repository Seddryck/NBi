using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal abstract class DataNotEmpty : Validation
    {
        private readonly string Data;

        internal DataNotEmpty(string data)
        {
            Data = data;
        }

        internal override void Apply()
        {
            if (string.IsNullOrEmpty(Data))
                GenerateException();
        }
    }
}
