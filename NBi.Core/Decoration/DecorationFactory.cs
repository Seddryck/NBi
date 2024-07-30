using System;
using NBi.Core.Etl;
using NBi.Core.Assemblies;
using NBi.Core.Decoration;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.Process;
using NBi.Core.Decoration.Grouping;
using NBi.Core.Assemblies.Decoration;
using NBi.Extensibility;
using NBi.Extensibility.Decoration;
using System.Collections.Generic;

namespace NBi.Core.Decoration
{
    public class DecorationFactory  
    {
        public IDecorationCommand Instantiate(IDecorationCommandArgs args)
        {
            return args switch
            {
                IGroupCommandArgs groupArgs => InstantiateGroup(groupArgs),
                IDataEngineeringCommandArgs dataEngineeringArgs => new DataEngineeringFactory().Instantiate(dataEngineeringArgs),
                IIoCommandArgs ioArgs => new IOFactory().Instantiate(ioArgs),
                IProcessCommandArgs processArgs => new ProcessCommandFactory().Instantiate(processArgs),
                CustomCommandArgs customArgs => new CustomCommandFactory().Instantiate(customArgs),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private IGroupCommand InstantiateGroup(IGroupCommandArgs args)
        {
            var children = new List<IDecorationCommand>();
            foreach (var chidrenArgs in args.Commands)
                children.Add(Instantiate(chidrenArgs));

            return new GroupCommandFactory().Instantiate(args, children);
        }

        public IDecorationCondition Instantiate(IDecorationConditionArgs args)
        {
            return args switch
            {
                IProcessConditionArgs processArgs => new ProcessConditionFactory().Instantiate(processArgs),
                IIoConditionArgs ioArgs => new IoConditionFactory().Instantiate(ioArgs),
                ICustomConditionArgs customConditionArgs => new CustomConditionFactory().Instantiate(customConditionArgs),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
