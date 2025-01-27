using Deedle;
using NBi.Core.Scalar.Casting;
using NBi.Extensibility.Resolving;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function;

abstract class Concatenation<T> : BaseAggregation<T>
{
    protected IScalarResolver<string> Separator { get; }
    public Concatenation(ICaster<T> caster, IScalarResolver<string> separator) : base(caster)
        => Separator = separator;

    protected override T? Execute(Series<int, T>? series) 
        => Caster.Execute(string.Join(Separator.Execute(), series?.Values.ToArray() ?? []));
}

class ConcatenationText : Concatenation<string>
{
    public ConcatenationText(IScalarResolver<string> separator) : base(new TextCaster(), separator)
    { }
}
