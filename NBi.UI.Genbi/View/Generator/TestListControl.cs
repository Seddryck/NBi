using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NBi.Service.Dto;
using NBi.UI.Genbi.Interface.Generator.Events;

namespace NBi.UI.Genbi.View.Generator
{
    public partial class TestListControl : UserControl
    {

        internal  TestSuiteViewAdapter Adapter { get; set; }

        public TestListControl()
        {
            InitializeComponent();
        }

        internal void DeclareBindings()
        {
            testsList.DataSource = bindingTests;
        }

        public BindingList<Test> Tests
        {
            get
            {
                return (BindingList<Test>)(bindingTests.DataSource);
            }
            set
            {
                bindingTests.DataSource = value;
                testsList.DisplayMember = "Title";
            }
        }

        public int TestSelectedIndex
        {
            get
            {
                return testsList.SelectedIndex;
            }
            set
            {
                testsList.SelectedIndex = value;
            }
        }

        public int ProgressValue
        {
            set
            {
                if (progressBarTest.Value != value)
                {
                    progressBarTest.Value = value;
                    progressBarTest.Refresh();
                }
            }
        }


        private void TestsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Adapter.InvokeTestSelect(new TestSelectEventArgs(testsList.SelectedIndex));
        }

        private void TestsList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //select the item under the mouse pointer
                testsList.SelectedIndex = testsList.IndexFromPoint(e.Location);
                if (testsList.SelectedIndex != -1)
                {
                    testsListMenu.Show();
                }
            }
        }

        private void DeleteTest_Click(object sender, EventArgs e)
        {
            Adapter.InvokeTestDelete(EventArgs.Empty);
        }


        
    }
}
