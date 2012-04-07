using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataExcelOleDbWriter : MetadataExcelOleDbAbstract
    {
        public event ProgressStatusHandler ProgressStatusChanged;
        public delegate void ProgressStatusHandler(Object sender, ProgressStatusEventArgs e);

        public MetadataExcelOleDbWriter(string filename) : base(filename) {}

        public MetadataExcelOleDbWriter(string filename, string sheetname) : base(filename, sheetname) { }
        

        //public void Write(string perspective, MeasureGroups measureGroups)
        //{
        //    if (measureGroups == null)
        //        throw new ArgumentNullException();

        //    GetSheets();
        //    if (Sheets.IndexOf(SheetName) == -1)
        //        CreateNewSheet(SheetName);

        //    if (ProgressStatusChanged != null)
        //        ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Opening Xls file")));

        //    DataTable dataTable = WriteInDataTable(perspective, measureGroups);

        //    int i = 0;
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        i++;
        //        if (ProgressStatusChanged != null)
        //            ProgressStatusChanged(this, new ProgressStatusEventArgs(String.Format("Writing row {0} of {1}", i, dataTable.Rows.Count), i, dataTable.Rows.Count));
                
        //        InsertMetadata(row);
        //    }

        //    if (ProgressStatusChanged != null)
        //        ProgressStatusChanged(this, new ProgressStatusEventArgs("Xls file written"));
        //}


        protected void CreateNewSheet(string sheetName)
        {
            var dt = CreateDataTable(sheetName);
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

        protected DataTable WriteInDataTable(string perspective, MeasureGroups measureGroups)
        {
            var dt = CreateDataTable(SheetName);

            foreach (var mg in measureGroups)
            {
                foreach (var m in mg.Value.Measures)
                {
                    foreach (var dim in mg.Value.LinkedDimensions)
                    {
                        foreach (var h in dim.Value.Hierarchies)
                        {
                            var row = dt.NewRow();
                            row[0] = perspective;
                            row[1] = mg.Value.Name;
                            row[2] = m.Value.Caption;
                            row[3] = m.Value.UniqueName;
                            row[4] = dim.Value.Caption;
                            row[5] = dim.Value.UniqueName;
                            row[6] = h.Value.Caption;
                            row[7] = h.Value.UniqueName;
                            dt.Rows.Add(row);
                        }
                    }
                }
                
            }
            return dt;
        }
  
        private DataTable CreateDataTable(string sheetName)
        {
            var dt = new DataTable(sheetName);
            dt.Columns.Add(new DataColumn("Perspective", typeof(string)));
            dt.Columns.Add(new DataColumn("MeasureGroup", typeof(string)));
            dt.Columns.Add(new DataColumn("MeasureCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("MeasureUniqueName", typeof(string)));
            dt.Columns.Add(new DataColumn("DimensionCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DimensionUniqueName", typeof(string)));
            dt.Columns.Add(new DataColumn("HierarchyCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("HierarchyUniqueName", typeof(string)));
            return dt;
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

        public void Write(string perspective, MeasureGroups measureGroups)
        {
            if (measureGroups == null)
                throw new ArgumentNullException();

            GetSheets();
            if (Sheets.IndexOf(SheetName + "$") == -1)
                CreateNewSheet(SheetName);

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Opening Xls file")));

            DataTable dataTable = WriteInDataTable(perspective, measureGroups);
            dataTable.TableName = SheetName + "$";
            
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

                    conn.Open();
                    da.Update(dataTable);
                }
            }
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Xls file written"));
        }

        
    }
}
