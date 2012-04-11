using System;
using System.Windows.Forms;

namespace NBi.UI
{
    public partial class ResultSetCreate : Form
    {
        public string QueriesDirectory { get; private set; }
        public string ResultsDirectory { get; private set; }
        public string ConnectionString { get; private set; }
        
        public ResultSetCreate()
        {
            InitializeComponent();
            queriesDirectory.Text = @"C:\Users\Seddryck\Documents\TestCCH\Queries\";
            resultsDirectory.Text = @"C:\Users\Seddryck\Documents\TestCCH\Results\";
        }

        private void resultsDirectorySelect_Click(object sender, EventArgs e)
        {
            SynchronizeFolderBrowserTextBox(folderBrowserDialog, resultsDirectory);
        }

        private void queriesDirectorySelect_Click(object sender, EventArgs e)
        {
            SynchronizeFolderBrowserTextBox(folderBrowserDialog, queriesDirectory);
        }

        private void SynchronizeFolderBrowserTextBox(FolderBrowserDialog fbd, TextBox textBox)
        {
            fbd.SelectedPath = textBox.Text;
            if (fbd.ShowDialog().Equals(DialogResult.OK))
            {
                textBox.Text = fbd.SelectedPath;
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            QueriesDirectory = queriesDirectory.Text;
            ResultsDirectory = resultsDirectory.Text;
            ConnectionString = connectionString.Text;

            this.Close();
        }

        private void ResultSetCreate_Load(object sender, EventArgs e)
        {

        }

    }
}
