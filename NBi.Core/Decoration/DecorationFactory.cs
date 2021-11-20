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
            switch (args)
            {
                case IGroupCommandArgs groupArgs: return InstantiateGroup(groupArgs);
                case IDataEngineeringCommandArgs dataEngineeringArgs: return new DataEngineeringFactory().Instantiate(dataEngineeringArgs);
                case IIoCommandArgs ioArgs: return new IOFactory().Instantiate(ioArgs);
                case IProcessCommandArgs processArgs: return new ProcessCommandFactory().Instantiate(processArgs);
                case ICustomCommandArgs customArgs: return new CustomCommandFactory().Instantiate(customArgs);
                default: throw new ArgumentOutOfRangeException();
            }
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
            switch (args)
            {
                case IProcessConditionArgs processArgs: return new ProcessConditionFactory().Instantiate(processArgs);
                case IIoConditionArgs ioArgs: return new IoConditionFactory().Instantiate(ioArgs);
                case ICustomConditionArgs customConditionArgs: return new CustomConditionFactory().Instantiate(customConditionArgs);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
