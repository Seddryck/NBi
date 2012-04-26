using System;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI
{
    public partial class MetadataExtract : Form
    {
        public MetadataAdomdExtractor MetadataExtractor { get; set; }

        public string ConnectionString 
        {
            get
            {
                return connectionString.Text;
            }
            set
            {
                connectionString.Text = value;
            }
        }
        
        public MetadataExtract()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            MetadataExtractor = new MetadataAdomdExtractor(connectionString.Text);
            this.Close();
        }
    }
}
