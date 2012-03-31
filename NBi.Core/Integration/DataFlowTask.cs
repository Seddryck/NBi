using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace NBi.Core.Integration
{
    public class DataFlowTask
    {
        protected TaskHost _taskHost;
        protected MainPipe _dataFlowTask;
        protected Application _application;

        public DataFlowTask(TaskHost taskHost, Application application)
        {
            _application = application;
            _taskHost = taskHost;
            _dataFlowTask = taskHost.InnerObject as MainPipe;
        }

        public static bool IsValid(TaskHost taskHost)
        {
            return (taskHost.InnerObject is MainPipe);
        }

        public void AddDataReaderDestination()
        {

            IDTSComponentMetaData100 drd = _dataFlowTask.ComponentMetaDataCollection.New();
            drd.Name = "DataReader Destination";
            drd.ComponentClassID = _application.PipelineComponentInfos["DataReader Destination"].CreationName;

            CManagedComponentWrapper drdDesignTime = drd.Instantiate();
            drdDesignTime.ProvideComponentProperties();

            var source = _dataFlowTask.ComponentMetaDataCollection[1];

            // Create the path.
            IDTSPath100 path = _dataFlowTask.PathCollection.New();
            path.AttachPathAndPropagateNotifications(source.OutputCollection[0],
              drd.InputCollection[0]);

        }
    }
}
