using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension;

public class ExtendArgs : IExtensionArgs
{
    public IColumnIdentifier NewColumn { get; set; }
    public string Code { get; set; }
    public LanguageType Language { get; set; }

    public ExtendArgs(IColumnIdentifier newColumn, string code, LanguageType language)
        => (NewColumn, Code, Language) = (newColumn, code, language);
}
