using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;

namespace NBi.Service
{
    public class TestCasesManager
    {
        public TestCasesManager()
        {
            variables = new List<string>();
            content = new DataTable();
        }

        public void ReadFromCsv(string filename)
        {
            var csvReader = new CsvReader(filename, true);
            content = csvReader.Read();          

            variables.Clear();
            foreach (DataColumn col in Content.Columns)
                variables.Add(col.ColumnName);
        }

        public void ReadFromQueryFile(string queryFile, string connectionString)
        {
            var query = System.IO.File.ReadAllText(queryFile);
            ReadFromQuery(query, connectionString);
        }

        public void ReadFromQuery(string query, string connectionString)
        {
            var queryEngineFactory = new QueryEngineFactory();
            var queryEngine = queryEngineFactory.GetExecutor(query, connectionString);
            var ds = queryEngine.Execute();
            content = ds.Tables[0];
                
            variables.Clear();
            foreach (DataColumn col in Content.Columns)
                variables.Add(col.ColumnName);
        }

        private DataTable content;
        public DataTable Content
        {
            get
            {
                return content;
            }
        }

        private readonly List<string> variables;
        public IList<string> Variables
        {
            get
            {
                return variables;
            }
        }

        public void MoveVariable(string variableName, int newPosition)
        {
            if(!variables.Contains(variableName))
                throw new ArgumentOutOfRangeException("variableName");
            //Move the variable
            var oldPosition = variables.IndexOf(variableName);
            variables.RemoveAt(oldPosition);
            variables.Insert(newPosition, variableName);
            //move the column
            content.Columns[oldPosition].SetOrdinal(newPosition);
        }


        public void FilterOut(string variableName, string text)
        {
            if (!variables.Contains(variableName))
                throw new ArgumentOutOfRangeException("variableName");

            var index = variables.IndexOf(variableName);

            var i = 0;
            while(i<content.Rows.Count)
            {
                if (content.Rows[i][index].ToString() == text)
                    content.Rows[i].Delete();
                i++;
            }
            content.AcceptChanges();
        }

        public void FilterIn(string variableName, string text)
        {
            if (!variables.Contains(variableName))
                throw new ArgumentOutOfRangeException("variableName");

            var index = variables.IndexOf(variableName);

            var i = 0;
            while (i < content.Rows.Count)
            {
                if (content.Rows[i][index].ToString() != text)
                    content.Rows[i].Delete();
                i++;
            }
            content.AcceptChanges();
        }
    }
}
