using System;
using System.Linq;
using System.Windows.Forms;
using NBi.Service.Dto;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class DisplayTestView : Form
    {
        public Test Test { get; set; }

        public DisplayTestView()
        {
            InitializeComponent();
            DeclareBindings();
            BindPresenter();
        }

        public void DeclareBindings()
        {
            this.DataBind(Test);
        }

        private void DataBind(Test test)
        {
            xmlTextEditor.DataBindings.Clear();
            if (test!=null)
                xmlTextEditor.DataBindings.Add("Text", test, "Content");
        }

        protected void BindPresenter()
        {
            
        }


        private void Close_Click(object sender, EventArgs e)
        {
            this.Hide();
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
