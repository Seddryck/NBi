using System;
using System.Linq;
using System.Windows.Forms;
using NBi.Service.Dto;
using NBi.UI.Genbi.Interface.TestSuiteGenerator;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class DisplayTestView : Form
    {
        protected ITestSuiteGeneratorView Origin { get; set; }
        public string TestContent { get; set; }

        public DisplayTestView(ITestSuiteGeneratorView origin)
        {
            Origin = origin;
            InitializeComponent();
        }


        private Test testSelected;
        public Test TestSelected
        {
            get
            {
                return testSelected;
            }
            set
            {
                testSelected = value;
                if (value == null)
                    TestContent = string.Empty;
                else
                    TestContent = value.Content;
            }
        }


        private void Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public new void Show()
        {
            xmlTextEditor.Text = TestContent;
            base.Show();
        }

        private void DisplayTestView_Load(object sender, EventArgs e)
        {

        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        

        
    }
}
