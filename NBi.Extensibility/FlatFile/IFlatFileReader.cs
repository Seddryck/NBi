using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.FlatFile;

public interface IFlatFileReader
{
    event ProgressStatusHandler ProgressStatusChanged;
    DataTable ToDataTable(string filename);
}
