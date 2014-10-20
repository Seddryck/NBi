using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataExcelOleDbWriter : MetadataExcelOleDbAbstract, IMetadataWriter
    {
        protected int _rowCount;
        protected int _rowTotal;

        public MetadataExcelOleDbWriter(string filename) : base(filename) {}

        public MetadataExcelOleDbWriter(string filename, string sheetname) : base(filename, sheetname) { }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void Write(CubeMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException();

            GetSheets();
            if (_sheets.IndexOf(SheetName + "$") == -1)
                CreateNewSheet(SheetName);

            RaiseProgressStatus("Opening Xls file");

            DataTable dataTable = MetadataFileFormat.WriteInDataTable(metadata);
            dataTable.TableName = SheetName + "$";
            _rowTotal = dataTable.Rows.Count;

            using (var conn = new OleDbConnection(GetConnectionString(Filename)))
            {
                using (var da = new OleDbDataAdapter())
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("INSERT INTO [{0}$] (", SheetName);

                    foreach (DataColumn col in dataTable.Columns)
                        sb.AppendFormat(" {0},", col.ColumnName);
                    sb.Remove(sb.Length - 1, 1); //Remove the last comma
                    sb.Append(") VALUES (");
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                        sb.Append(" ?,");
                    sb.Remove(sb.Length - 1, 1); //Remove the last comma
                    sb.Append(")");
                    da.InsertCommand = new OleDbCommand(sb.ToString(), conn);
                    foreach (DataColumn col in dataTable.Columns)
                        da.InsertCommand.Parameters.Add(string.Format("@{0}", col.ColumnName), OleDbType.VarChar, 255, col.ColumnName);

                    da.RowUpdated += new OleDbRowUpdatedEventHandler(OnRowUpdated);
                    conn.Open();
                    da.Update(dataTable);
                    da.RowUpdated -= new OleDbRowUpdatedEventHandler(OnRowUpdated);
                }
            }
            RaiseProgressStatus("Xls file written");
        }

        protected void OnRowUpdated(object sender, OleDbRowUpdatedEventArgs args)
        {
            _rowCount++;
            RaiseProgressStatus("Saving row {0} of {1}", _rowCount, _rowTotal);
        }

        protected void CreateNewSheet(string sheetName)
        {
            var dt = MetadataFileFormat.CreateDataTable(sheetName);
            CreateNewSheet(dt);
            GetSheets();
        }

        protected void CreateNewSheet(DataTable dataTable)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("CREATE TABLE [{0}] (", dataTable.TableName);

            foreach (DataColumn col in dataTable.Columns)
            {
                sb.AppendFormat(" {0} char(255),", col.ColumnName);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");

            Execute(sb.ToString());
        }

        protected void InsertMetadata(DataRow row)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO [{0}$] VALUES (", SheetName);

            foreach (var item in row.ItemArray)
            {
                sb.AppendFormat(" '{0}',", item.ToString());
            }
            sb.Remove(sb.Length-1,1);
            sb.Append(")");

            Execute(sb.ToString());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected void Execute(string sql)
        {
            using (var conn = new OleDbConnection())
            {
                conn.ConnectionString = GetConnectionString(Filename);
                conn.Open();

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
