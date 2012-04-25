using System;
using System.IO;
using System.Windows.Forms;

namespace NBi.UI
{
    public partial class ResultSetCreate : Form
    {
        public string QueriesDirectory { get; internal set; }
        public string ResultsDirectory { get; internal set; }
        public string ConnectionString { get; internal set; }

        public ResultSetCreate()
        {
            InitializeComponent();
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

        protected virtual void ok_Click(object sender, EventArgs e)
        {
            QueriesDirectory = queriesDirectory.Text;
            ResultsDirectory = resultsDirectory.Text;
            ConnectionString = connectionString.Text;

            if (!Directory.Exists(QueriesDirectory))
                MessageBox.Show(String.Format("Directory \"{0}\" doesn't exist!", QueriesDirectory), "Non existing directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (!Directory.Exists(ResultsDirectory))
                MessageBox.Show(String.Format("Directory \"{0}\" doesn't exist!", ResultsDirectory), "Non existing directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                DialogResult = DialogResult.OK;
        }

        private void ResultSetCreate_Load(object sender, EventArgs e)
        {
            queriesDirectory.Text = QueriesDirectory;
            resultsDirectory.Text = ResultsDirectory;
            connectionString.Text = ConnectionString;
        }

    }
}
