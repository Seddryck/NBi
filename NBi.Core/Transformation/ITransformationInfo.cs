using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation;

public interface ITransformationInfo
{
    ColumnType OriginalType { get; set; }
    LanguageType Language { get; set; }
    string Code { get; set; }
}
