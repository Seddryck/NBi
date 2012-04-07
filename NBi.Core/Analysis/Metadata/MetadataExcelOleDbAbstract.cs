using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;


namespace NBi.Core.Analysis.Metadata
{
    public abstract class MetadataExcelOleDbAbstract
    {


        public string Filename { get; private set; }

        private string _sheetName;
        public string SheetName
        {
            get { return _sheetName; }
            set
            {
                if (value.EndsWith("$"))
                    value = value.Remove(value.Length - 1);
                _sheetName = value;
            }
        }

        public IList<string> Sheets { get; private set; }

        public MetadataExcelOleDbAbstract(string filename)
        {
            Filename = filename;
        }

        public MetadataExcelOleDbAbstract(string filename, string sheetname) : this(filename)
        {
            SheetName = sheetname;
        }

        protected string GetConnectionString(string filename)
        {
            return
               @"Provider=Microsoft.Jet.OLEDB.4.0;" +
               @"Data Source=" + filename + ";" +
               @"Extended Properties=" + Convert.ToChar(34).ToString() +
               @"Excel 8.0;HDR=YES"//;Excel 12.0 Xml;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text" 
               + Convert.ToChar(34).ToString()
               ;
        }

        public void GetSheets()
        {
            var dt = new DataTable("Track");

            using (var conn = new OleDbConnection())
            {
                conn.ConnectionString = GetConnectionString(Filename);
                conn.Open();

                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                Sheets = excelSheets;
            }

        }
    }
}
