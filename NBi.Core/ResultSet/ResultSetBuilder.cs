using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using System.Xml;

namespace NBi.Core.ResultSet
{
    public class ResultSetBuilder : IResultSetBuilder
    {
        private readonly CsvProfile profile;
        public ResultSetBuilder()
            : this(CsvProfile.SemiColumnDoubleQuote)
        {
        }

        public ResultSetBuilder(CsvProfile profile)
        {
            this.profile = profile;
        }

        public virtual ResultSet Build(Object obj)
        {
            //Console.WriteLine("Debug: {0} {1}", obj.GetType(), obj.ToString()); 
            
            if (obj is ResultSet)
                return Build((ResultSet)obj);
            else if (obj is IList<IRow>)
                return Build((IList<IRow>)obj);
            else if (obj is IDbCommand)
                return Build((IDbCommand)obj);
            else if (obj is ResultSetFile)
                return Build((string)obj);

            throw new ArgumentOutOfRangeException(string.Format("Type '{0}' is not expected when building a ResultSet", obj.GetType()));
        }
        
        public virtual ResultSet Build(ResultSet resultSet)
        {
            return resultSet;
        }

        public virtual ResultSet Build(IList<IRow> rows)
        {
            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
        
        public virtual ResultSet Build(IDbCommand cmd)
        {
            var qe = new QueryEngineFactory().GetExecutor(cmd);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
            return rs;
        }

        public virtual ResultSet Build(ResultSetFile file)
        {
            switch (file.Type)
            {
                case ResultSetFileType.Csv:
                    return BuildFromCsv(file.Path);
                case ResultSetFileType.Xml:
                    return BuildFromXml(file.Path);
                default:
                    break;
            }
            throw new ArgumentOutOfRangeException();
        }

        public virtual ResultSet BuildFromCsv(string csvPath)
        {
            var reader = new CsvReader(profile);
            var dataTable = reader.Read(csvPath, false);

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }

        public virtual ResultSet BuildFromXml(string xmlPath)
        {
            var reader = XmlReader.Create(xmlPath);
            var dataSet = new DataSet();
            dataSet.ReadXml(reader);
            
            var rs = new ResultSet();
            rs.Load(dataSet.Tables[0]);
            return rs;
        }

    }
}
