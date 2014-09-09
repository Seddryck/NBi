using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.NUnit.Runtime
{
    class CategoryHelper
    {
        internal static string Format(string category)
        {
            return category.Replace("-", "_");
        }
    }
}
