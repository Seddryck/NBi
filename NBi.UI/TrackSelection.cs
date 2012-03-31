using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NBi.QueryGenerator;

namespace NBi.UI
{
    public partial class TrackSelection : Form
    {
        public MetadataExcelReader MetadataExcelReader { get; set; }
        public string Track  { get; protected set; }
                
        public TrackSelection()
        {
            InitializeComponent();
        }

        private void TrackSelection_Load(object sender, EventArgs e)
        {
            sheetSelected.Items.Clear();
            MetadataExcelReader.GetSheets();
            sheetSelected.Items.AddRange(MetadataExcelReader.Sheets.ToArray());
            sheetSelected.SelectedIndex = 0;       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Track = (string)trackSelected.SelectedItem;
            this.Close();
        }

        private void sheetSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            MetadataExcelReader.SheetName = (string)sheetSelected.SelectedItem;
            
            trackSelected.Items.Clear();
            trackSelected.Items.Add("None");
            MetadataExcelReader.GetTracks();
            trackSelected.Items.AddRange(MetadataExcelReader.Tracks.ToArray());
            trackSelected.SelectedIndex = 0;

        }

       

    }
}
