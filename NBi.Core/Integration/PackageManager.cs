using System;
using System.Data;
using System.Diagnostics;
using Microsoft.SqlServer.Dts.DtsClient;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Integration
{
    public class PackageManager
    {
        protected string _packageLocation;
        public Package OriginalPackage {get; protected set;}
        public Package ModifiedPackage { get; protected set; }
        public Application Application { get; protected set; }
        
        public void Load(string packageLocation)
        {
            _packageLocation = packageLocation;
            
            Application = new Application();

            if (!System.IO.File.Exists(packageLocation))
                throw new ArgumentException(String.Format("Impossible to load the package. No file found at {0}.", packageLocation));
            
            OriginalPackage = Application.LoadPackage(packageLocation, null);
            
           
        }

        public bool Execute()
        {
            if (ModifiedPackage == null)
                throw new Exception("Load the package before trying to execute it!");

            DTSExecResult pkgResults;
            pkgResults = ModifiedPackage.Execute();
            
            if(pkgResults.Equals(DTSExecResult.Failure) || pkgResults.Equals(DTSExecResult.Canceled))
                return false;
            return true;
        }

        public bool Contains(string taskName)
        {
            TaskHost taskHost = ExtractTask(taskName);

            return (taskHost != null);
        }

        protected TaskHost ExtractTask(string taskName)
        {
            foreach (var executable in OriginalPackage.Executables)
            {
                TaskHost taskHost = executable as TaskHost;
                if (taskHost.Name==taskName)
                {
                    return taskHost;
                }
            }
            return null;
        }

        public DataFlowTask GetDataFlowTask(string taskName)
        {
            TaskHost taskHost;
            
            if (Contains(taskName))
                taskHost = ExtractTask(taskName);
            else
                throw new Exception("Task not found");

            if (DataFlowTask.IsValid(taskHost))
                return new DataFlowTask(taskHost, Application);
            else
                throw new Exception("Task is not a data flow task");
        }

        public void Save(string filename)
        {
            Application.SaveToXml(filename, OriginalPackage, null);

            if (!System.IO.File.Exists(filename))
                throw new ArgumentException(String.Format("Impossible to load the package. No file found at {0}.", filename));
            
            ModifiedPackage = Application.LoadPackage(filename, null);
            
        }

        public void Read()
        {
            using (var connection = new DtsConnection())
            {
                connection.ConnectionString = string.Format(@"/File ""{0}""", @"C:\Users\Seddryck\Documents\Visual Studio 2010\Projects\NBi\NBi.Testing.Ssis\FirstSample-test 1.dtsx");
                
                connection.Open();
                var command = new DtsCommand(connection);
                command.CommandText = "DataReaderDest";
                var reader = command.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    Debug.WriteLine(reader.GetData(0));
                }
                
                
            }
        }
    }
}
