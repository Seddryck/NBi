using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using NBi.Extensibility;

namespace NBi.Model.Items.Alteration.Transform;

public class Transform : AlterationDefinition, ITransformationInfo
{
    public string Code { get; set; }

    public LanguageType Language { get; set; }

    public ColumnType OriginalType { get; set; }

    [Obsolete("Use Identifier in place of ColumnOrdinal")]
    public int ColumnOrdinal
    {
        get => throw new InvalidOperationException();
        set => Identifier = new ColumnIdentifierFactory().Instantiate($"#{value}");
    }

    public string IdentifierSerializer { get; set; }
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }
}
