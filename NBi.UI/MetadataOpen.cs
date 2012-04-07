using System;
using System.Linq;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI
{
    public partial class MetadataOpen : Form
    {
        public MetadataExcelOleDbReader MetadataReader { get; set; }
        public string Track  { get; protected set; }
                
        public MetadataOpen()
        {
            InitializeComponent();
        }

        private void TrackSelection_Load(object sender, EventArgs e)
        {
            sheetSelected.Items.Clear();
            MetadataReader.GetSheets();
            sheetSelected.Items.AddRange(MetadataReader.Sheets.ToArray());
            sheetSelected.SelectedIndex = 0;       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Track = (string)trackSelected.SelectedItem;
            this.Close();
        }

        private void sheetSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            MetadataReader.SheetName = (string)sheetSelected.SelectedItem;
            
            trackSelected.Items.Clear();
            trackSelected.Items.Add("None");
            MetadataReader.GetTracks();
            trackSelected.Items.AddRange(MetadataReader.Tracks.ToArray());
            trackSelected.SelectedIndex = 0;

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Track = null;
            this.Close();
        }

       

    }
}
