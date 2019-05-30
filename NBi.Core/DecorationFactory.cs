using System;
using System.Linq;
using NBi.Core.DataManipulation;
using NBi.Core.Etl;
using NBi.Core.WindowsService;
using NBi.Core.Batch;
using NBi.Core.FileManipulation;
using NBi.Core.Process;
using NBi.Core.Connection;
using NBi.Core.Assemblies;

namespace NBi.Core
{
    public class DecorationFactory
    {
        public IDecorationCommandImplementation Instantiate(IDecorationCommand commandMetadata)
        {
            switch (commandMetadata)
            {
                case IGroupCommand group: return new GroupCommandFactory().Get(group);
                case IWindowsServiceCommand windowsService: return new WindowsServiceCommandFactory().Get(windowsService);
                case IDataManipulationCommand dataManipulation: return new DataManipulationFactory().Get(dataManipulation);
                case IBatchRunCommand batchRun: return new BatchRunnerFactory().Get(batchRun);
                case IEtlRunCommand etlRun: return new EtlRunnerFactory().Get(etlRun);
                case IFileManipulationCommand fileManipulation: return new FileManipulationFactory().Get(fileManipulation);
                case IProcessCommand process: return new ProcessCommandFactory().Get(process);
                case IConnectionWaitCommand connectionWait: return new ConnectionWaitFactory().Get(connectionWait);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public IDecorationCondition Instantiate(IDecorationConditionMetadata condition)
        {
            switch (condition)
            {
                case IWindowsServiceConditionMetadata windowsServiceCheck:
                    return new WindowsServiceConditionFactory().Instantiate(windowsServiceCheck);
                case ICustomConditionMetadata customCondition:
                    return new CustomConditionFactory().Instantiate(customCondition);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
