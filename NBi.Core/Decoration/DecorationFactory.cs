using System;
using NBi.Core.Etl;
using NBi.Core.Assemblies;
using NBi.Core.Decoration;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.Process;
using NBi.Core.Decoration.Grouping;
using NBi.Core.Assemblies.Decoration;

namespace NBi.Core.Decoration
{
    public class DecorationFactory
    {
        public IDecorationCommand Instantiate(IDecorationCommandArgs args)
        {
            switch (args)
            {
                case IGroupCommandArgs groupArgs: return new GroupCommandFactory().Instantiate(groupArgs);
                case IDataEngineeringCommandArgs dataEngineeringArgs: return new DataEngineeringFactory().Instantiate(dataEngineeringArgs);
                case IIoCommandArgs ioArgs: return new IOFactory().Instantiate(ioArgs);
                case IProcessCommandArgs processArgs: return new ProcessCommandFactory().Instantiate(processArgs);
                case ICustomCommandArgs customArgs: return new CustomCommandFactory().Instantiate(customArgs);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public IDecorationCondition Instantiate(IDecorationConditionArgs args)
        {
            switch (args)
            {
                case IProcessConditionArgs processArgs: return new ProcessConditionFactory().Instantiate(processArgs);
                case ICustomConditionArgs customConditionArgs: return new CustomConditionFactory().Instantiate(customConditionArgs);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
