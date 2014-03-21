using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core;

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
            
    }
}
