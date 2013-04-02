using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.Generator;

namespace NBi.UI.Genbi.View.Generator
{
    public partial class DisplayTestView : Form
    {
        protected ICsvGeneratorView Origin { get; set; }
        public string TestContent { get; set; }

        public DisplayTestView(ICsvGeneratorView origin)
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
