using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class TestListControl : UserControl
    {

        public TestListControl()
        {
            InitializeComponent();
        }

        internal void DataBind(TestListPresenter presenter)
        {
            if (presenter != null)
            {
                testsList.DataSource = presenter.Tests;
                testsList.DisplayMember = "Title";

                testsList.DataBindings.Add("SelectedItem", presenter, "SelectedTest", true, DataSourceUpdateMode.OnPropertyChanged);
                testsList.SelectedIndexChanged += (s, args) => testsList.DataBindings["SelectedItem"].WriteValue();

                progressBarTest.DataBindings.Add("Value", presenter, "Progress");
            }
        }

        //public int ProgressValue
        //{
        //    set
        //    {
        //        //In case of issue ajust the value to be in [0,100]
        //        value = Math.Max((Math.Min(value, 100)), 0);

        //        //If value doesn't change effectively don't do the update
        //        if (progressBarTest.Value != value)
        //        {
        //            progressBarTest.Value = value;
        //            progressBarTest.Refresh();
        //        }
        //    }
        //}

        private void TestsList_MouseDown(object sender, MouseEventArgs e)
        {
            //select the item under the mouse pointer
            testsList.SelectedIndex = testsList.IndexFromPoint(e.Location);

            //If it's a right click (and something is selected) display the pop-up menu
            if (e.Button == MouseButtons.Right && testsList.SelectedIndex != -1)
                testsListMenu.Show(testsList, e.Location);
        }



        public ToolStripMenuItem DeleteCommand
        {
            get { return deleteTest; }
        }
    }
}
