using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations
{
    internal abstract class FilterNotNull : Validation
    {
        private readonly string data;

        internal FilterNotNull(string data)
        {
            this.data = data;
        }

        internal override void Apply()
        {
            if (string.IsNullOrEmpty(data))
                GenerateException();
        }
    }
}
