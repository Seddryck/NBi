using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class TestVariableFactory
    {
        public ITestVariable Instantiate(LanguageType languageType, string code)
        {
            if (languageType == LanguageType.CSharp)
                return new CSharpTestVariable(code.Trim());
            else
                throw new ArgumentException();
        }
    }
}
