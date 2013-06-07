using System;
using System.Linq;
using System.Text;
using NBi.Core.ResultSet;

namespace NBi.Core.Log
{
    internal class ResultSetLogger : ILogger
    {


        public string FullPath { get; private set; }
        private readonly Condition condition;
        private readonly IResultSetWriter writer;
        private NBi.Core.ResultSet.ResultSet resultSet;

        public Condition Condition
        {
            get { return condition; }
        }

        public Content Content
        {
            get { return NBi.Core.Log.Content.ResultSet; }
        }
        

        public ResultSetLogger(string fullPath, Condition condition)
        {
            this.condition = condition;
            FullPath = fullPath;

            var path = System.IO.Path.GetDirectoryName(FullPath);
            if (string.IsNullOrEmpty(path))
                path = System.IO.Directory.GetCurrentDirectory();
            writer = new ResultSetCsvWriter(path);
        }

        protected internal ResultSetLogger(string fullPath, Condition condition, IResultSetWriter writer)
            : this(fullPath, condition)
        {
            this.writer = writer;
        }
        
        public void Persist(bool isFailure)
        {
            if (isFailure || condition == Log.Condition.Always)
            {
                var filename = System.IO.Path.GetFileName(FullPath);

                writer.Write(filename, resultSet);
            }
        }

        public virtual void Write(Object message)
        {
            if (!(message is NBi.Core.ResultSet.ResultSet))
                throw new ArgumentException("Parameter 'message' must be of type 'ResultSet'");

            Write((NBi.Core.ResultSet.ResultSet)message);
        }

        public void Write(NBi.Core.ResultSet.ResultSet resultSet)
        {
            this.resultSet = resultSet;
        }

    }
}
