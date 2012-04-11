using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI
{
    public partial class MetadataSave : Form
    {
        public IMetadataWriter MetadataWriter { get; set; }

        public MetadataSave()
        {
            InitializeComponent();
        }

        private void MetadataSave_Load(object sender, EventArgs e)
        {
            existingSheetSelected.Items.Clear();
            MetadataWriter.GetSheets();
            existingSheetSelected.Items.AddRange(MetadataWriter.Sheets.ToArray());
            existingSheetSelected.SelectedIndex = 0;    
        }

        private void existingSheetSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            MetadataWriter.SheetName = (string)existingSheetSelected.SelectedItem;
        }

        private void cancel_Click(object sender, EventArgs e)
        {

        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (newSheetName.Enabled)
                MetadataWriter.SheetName = newSheetName.Text;
            else
                MetadataWriter.SheetName = (string)existingSheetSelected.SelectedItem;

            this.Close();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            newSheetName.Enabled=sender.Equals(createNewSheet);
            existingSheetSelected.Enabled = sender.Equals(useExistingSheet);
        }

        


    
    }
}
