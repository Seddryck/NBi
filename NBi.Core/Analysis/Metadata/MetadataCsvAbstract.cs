using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;


namespace NBi.Core.Analysis.Metadata
{
    public abstract class MetadataCsvAbstract
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public CsvDefinition Definition { get; private set; }

        public string Filename { get; private set; }

        public bool SupportSheets { get {return false;}}

        private string _sheetName;
        public string SheetName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        protected readonly List<string> _sheets;
        public IEnumerable<string> Sheets 
        {
            get { throw new NotSupportedException(); }
        }

        protected readonly List<string> _tracks;
        public IEnumerable<string> Tracks
        {
            get { throw new NotImplementedException(); }
        }

        protected MetadataCsvAbstract(string filename)
        {
            Filename = filename;
            Definition = CsvDefinition.SemiColumnDoubleQuote();
        }

        public void GetSheets()
        {
            throw new NotSupportedException();
        }

        public void GetTracks()
        {
            throw new NotImplementedException();
        }

        public void RaiseProgressStatus(string status)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(status));
        }

        public void RaiseProgressStatus(string status, int current, int total)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format(status, current, total), current, total));
        }
    }
}
