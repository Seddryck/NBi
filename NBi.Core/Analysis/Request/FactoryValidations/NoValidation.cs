using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class NoValidation : Validation
{
    internal NoValidation()
    {
    }

    internal override void Apply()
    {
        //Do Nothing
    }

    internal override void GenerateException()
    {
        throw new InvalidOperationException("It should be impossible to be there");
    }
}
