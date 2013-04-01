using System;
using System.Linq;
using System.Windows.Forms;

namespace NBi.UI.View.GenericTest
{
    public partial class DisplayTestView : Form
    {
        protected CsvImporterView Origin { get; set; }
        public string TestContent { get; set; }
        
        public DisplayTestView(CsvImporterView origin)
        {
            Origin = origin;
            InitializeComponent();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public new void Show()
        {
            textBox1.Text = TestContent;
            base.Show();
        }


        

        
    }
}
