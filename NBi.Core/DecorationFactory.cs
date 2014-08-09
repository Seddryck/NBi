using System;
using System.Linq;
using NBi.Core.DataManipulation;
using NBi.Core.Etl;
using NBi.Core.WindowsService;
using NBi.Core.Analysis.Process;

namespace NBi.Core
{
    public class DecorationFactory
    {
        public IDecorationCommandImplementation Get(IDecorationCommand command)
        {
            if (command is IWindowsServiceCommand)
            {
                return new WindowsServiceCommandFactory().Get(command as IWindowsServiceCommand);
            }

            if (command is IDataManipulationCommand)
            {
                return new DataManipulationFactory().Get(command as IDataManipulationCommand);
            }

            if (command is IEtlRunCommand)
            {
                return new EtlRunnerFactory().Get(command as IEtlRunCommand);
            }

            if (command is ICubeProcessCommand)
            {
                return new CubeProcessFactory().Get(command as ICubeProcessCommand);
            }

            throw new ArgumentException();
        }

        public IDecorationCheckImplementation Get(IDecorationCheck check)
        {
            if (check is IWindowsServiceCheck)
            {
                return new WindowsServiceCheckFactory().Get(check as IWindowsServiceCheck);
            }

            throw new ArgumentException();
        }

    }
}
