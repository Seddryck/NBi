using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges;

class CompositeBuilder : BaseBuilder
{
    private readonly IRangeMembersBuilder builder;
    private readonly IEnumerable<IDecoratorBuilder> decorators;

    public CompositeBuilder(IRangeMembersBuilder builder, IEnumerable<IDecoratorBuilder> decorators)
    {
        this.builder = builder;
        this.decorators = decorators;
    }

    public override void Setup(IRange range)
    {
        base.Setup(range);
        builder.Setup(range);
        foreach (var decorator in decorators)
            decorator.Setup(range);
    }

    public override void Build()
    {
        base.Build();
        builder.Build();
        Result = builder.GetResult();
        foreach (var decorator in decorators)
        {
            decorator.Apply(Result);
            Result = decorator.GetResult();
        }
            
    }

    protected override void InternalBuild()
    {
        return;
    }
}
