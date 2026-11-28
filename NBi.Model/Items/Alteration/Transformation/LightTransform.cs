using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Model.Items.Alteration.Transform;

public class LightTransform : ITransformationInfo
{
    public string Code { get; set; }

    public LanguageType Language { get; set; }

    public ColumnType OriginalType { get; set; }
}
