using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.IO;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataCsvWriter : MetadataCsvAbstract, IMetadataWriter
    {
        protected int _rowCount;
        protected int _rowTotal;

        public MetadataCsvWriter(string filename) : base(filename) {}

        public void Write(CubeMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException();

            RaiseProgressStatus("Building Csv file content");

            DataTable dataTable = MetadataFileFormat.WriteInDataTable(metadata);
            _rowTotal = dataTable.Rows.Count;

            var sb = new StringBuilder();

            foreach (DataColumn col in dataTable.Columns)
                sb.AppendFormat("\"{0}\"{1}", col.ColumnName, Definition.FieldSeparator);
            sb.Remove(sb.Length - 1, 1); //Remove the last comma
            sb.AppendLine(); //goto next line

            foreach (DataRow row in dataTable.Rows)
            {
                _rowCount++;
                foreach (string item in row.ItemArray)
                {
                    var str = (item).Replace(Definition.TextQualifier.ToString(), Definition.TextQualifier.ToString() + Definition.TextQualifier.ToString());
                    sb.AppendFormat("\"{0}\"{1}", str, Definition.FieldSeparator);
                }
                sb.Remove(sb.Length - 1, 1); //Remove the last comma
                sb.AppendLine(); //goto next line
                RaiseProgressStatus("Creating row {0} of {1}", _rowCount, _rowTotal);
            }

            RaiseProgressStatus("Csv file content built");

            RaiseProgressStatus("Writing Csv file content");

            using (StreamWriter outfile = new StreamWriter(Filename, false, Encoding.UTF8))
            {
                outfile.Write(sb);
            }

            RaiseProgressStatus("Csv file written");
        }

            
    }
}
