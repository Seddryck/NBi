using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;

namespace NBi.Service
{
    public class CsvManager
    {
        private readonly string filename;
        public CsvManager(string filename)
        {
            this.filename=filename;
            columnHeaders = new List<string>();
        }

        private bool isRead = false;
        private void Read()
        {
            if (!isRead)
            {
                var csvReader = new CsvReader(filename, true);
                Content = csvReader.Read();
                isRead = true;
                foreach (DataColumn col in Content.Columns)
                    ColumnHeaders.Add(col.ColumnName);
            }
        }

        private DataTable content;
        public DataTable Content
        {
            get
            {
                Read();
                return content;
            }

            private set
            {
                content = value;
            }
        }

        private readonly List<string> columnHeaders;
        public IList<string> ColumnHeaders
        {
            get
            {
                Read();
                return columnHeaders;
            }
        }
            
    }
}
