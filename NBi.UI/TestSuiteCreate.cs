using System.IO;
using System.Windows.Forms;
using System;

namespace NBi.UI
{
    public partial class TestSuiteCreate : Form
    {
        public class ActualSettings
        {
            public string QueriesDirectory { get; internal set; }
            public string ConnectionString { get; internal set; }
        }

        public class ExpectSettings
        {
            protected bool _isResultSetsBased;
            public bool IsResultSetsBased
            {
                get
                {
                   return _isResultSetsBased;
                }
                internal set
                {
                    if (value != _isResultSetsBased)
                    {
                        _resultSetsDirectory = string.Empty;
                        _queriesDirectory = string.Empty;
                        _connectionString = string.Empty;
                    }
                    _isResultSetsBased = value;
                }
            }

            protected string _resultSetsDirectory;
            public string ResultSetsDirectory
            {
                get
                {
                    if (!IsResultSetsBased)
                        throw new InvalidOperationException();
                    else
                        return _resultSetsDirectory;
                }
                internal set
                {
                    if (!IsResultSetsBased)
                        throw new InvalidOperationException();
                    else
                        _resultSetsDirectory = value;
                }
            }

            protected string _queriesDirectory;
            public string QueriesDirectory
            {
                get
                {
                    if (IsResultSetsBased)
                        throw new InvalidOperationException();
                    else
                        return _queriesDirectory;
                }
                internal set
                {
                    if (IsResultSetsBased)
                        throw new InvalidOperationException();
                    else
                        _queriesDirectory = value;
                }
            }

            protected string _connectionString;
            public string ConnectionString
            {
                get
                {
                    if (IsResultSetsBased)
                        throw new InvalidOperationException();
                    else
                        return _connectionString;
                }
                internal set
                {
                    if (IsResultSetsBased)
                        throw new InvalidOperationException();
                    else
                        _connectionString = value;
                }
            }

            public ExpectSettings()
            {
                _isResultSetsBased = true;
            }
            
        }

        public ActualSettings Actual { get; internal set; }
        public ExpectSettings Expect { get; internal set; }

        public TestSuiteCreate()
        {
            InitializeComponent();
            Actual = new ActualSettings();
            Expect = new ExpectSettings();
        }


        private void queriesDirectoryActualSelect_Click(object sender, EventArgs e)
        {
            SynchronizeFolderBrowserTextBox(folderBrowserDialog, queriesDirectoryActual);
        }

        private void resultSetsDirectoryExpectSelect_Click(object sender, EventArgs e)
        {
            SynchronizeFolderBrowserTextBox(folderBrowserDialog, resultSetsDirectoryExpect);
        }

        private void queriesDirectoryExpectSelect_Click(object sender, EventArgs e)
        {
            SynchronizeFolderBrowserTextBox(folderBrowserDialog, queriesDirectoryExpect);
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
            Actual.QueriesDirectory = queriesDirectoryActual.Text;
            Actual.ConnectionString = connectionStringActual.Text;

            Expect.IsResultSetsBased = isResultSetsDirectory.Checked;
            if (Expect.IsResultSetsBased)
                Expect.ResultSetsDirectory = resultSetsDirectoryExpect.Text;
            else
            {
                Expect.QueriesDirectory = queriesDirectoryExpect.Text;
                Expect.ConnectionString = connectionStringExpect.Text;
            }

            if (!CheckDirectoryExists(Actual.QueriesDirectory)) return;
            if (Expect.IsResultSetsBased)
                if (!CheckDirectoryExists(Expect.ResultSetsDirectory)) return;
            else
                if (!CheckDirectoryExists(Actual.QueriesDirectory)) return;

            DialogResult = DialogResult.OK;
        }
  
        private bool CheckDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                MessageBox.Show(String.Format("Directory \"{0}\" doesn't exist!", directory), "Non existing directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }

        private void ResultSetCreate_Load(object sender, EventArgs e)
        {
            queriesDirectoryActual.Text = Actual.QueriesDirectory;
            connectionStringActual.Text = Actual.ConnectionString;
            isResultSetsDirectory.Checked = true;
            RadioButton_CheckedChanged(null, null);
            resultSetsDirectoryExpect.Text = Expect.ResultSetsDirectory;
            queriesDirectoryExpect.Text = string.Empty;
            connectionStringExpect.Text = string.Empty;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (isQueriesDirectory.Checked)
            {
                queriesDirectoryExpect.Text = Actual.QueriesDirectory;
                connectionStringExpect.Text = Actual.ConnectionString;
            }

            resultSetsDirectoryExpect.Enabled = isResultSetsDirectory.Checked;
            resultSetsDirectoryExpectSelect.Enabled = isResultSetsDirectory.Checked;
            queriesDirectoryExpect.Enabled = isQueriesDirectory.Checked;
            queriesDirectoryExpectSelect.Enabled = isQueriesDirectory.Checked;
            connectionStringExpect.Enabled = isQueriesDirectory.Checked;

        }

        

        
        
    }
}
