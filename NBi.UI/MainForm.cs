using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Query;
using NBi.Xml;

namespace NBi.UI
{
    public partial class MainForm : Form
    {
        protected CubeMetadata Metadata { get; set; }

        public MainForm()
        {
            InitializeComponent();
            metadataTreeview.Nodes.Clear();
        }
        
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            hierarchyFunction.SelectedIndex = 3;
        }


        private bool ConfirmBuildMdxQueries(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (Directory.GetFiles(path).Length == 0)
                return true;

            DialogResult dialogResult = MessageBox.Show(
                string.Format("Target directory {0} is not empty.\nDo you want to clean it before generating the queries?", path),
                "Not empty directory",
                MessageBoxButtons.YesNoCancel);

            if (dialogResult == DialogResult.Yes)
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }

            return (dialogResult != DialogResult.Cancel);

        }


#region Progress and Toolstrip
        
        private void StartClick(Button sender)
        {
            if (sender != null)
                sender.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
        }

        private void EndClick(Button sender)
        {
            statusStrip.Refresh();
            this.Cursor = Cursors.Default;
            if (sender != null)
                sender.Enabled = true;
        }

        private DateTime _statusTripLastRefresh;
        private const int STATUS_TRIP_REFRESH_RATE = 200;

        private void ProgressStatus(object sender, ProgressStatusEventArgs e)
        {
            toolStripStatus.Text = e.Status;
            toolStripStatus.Invalidate();

            toolStripProgressBar.Maximum = e.Progress.Total;
            toolStripProgressBar.Value = e.Progress.Current;

            if (DateTime.Now.Subtract(_statusTripLastRefresh).TotalMilliseconds > STATUS_TRIP_REFRESH_RATE)
            {
                statusStrip.Refresh();
                _statusTripLastRefresh = DateTime.Now;
            }
        }

#endregion

#region Toolstrip Menu

    #region File

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.StartupPath;
                ofd.FileName = "MyProject.nbi";
                ofd.Filter = "NBi|*.nbi";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Configuration.Project.Load(ofd.FileName);
                    toolStripStatus.Text = "Directories and connectionStrings defined";
                }
            }
        }
        
        private void saveAsProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "MyProject.nbi";
                sfd.Filter = "NBi|*.nbi";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Configuration.Project.Save(sfd.FileName);
                    toolStripStatus.Text = "Directories and connectionStrings saved";
                }
            }
        }
        
    #endregion

    #region Edit
        private void unselectAllMetadata_Click(object sender, System.EventArgs e)
        {
            this.metadataTreeview.UncheckAll();
        }

        private void selectAllMetadata_Click(object sender, System.EventArgs e)
        {
            this.metadataTreeview.CheckAll();
        }
        #endregion

    #region Metadata

        private void openMetadataToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var cfg = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Metadata];
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = cfg.FullPath;
                ofd.FileName = cfg.File;
                ofd.Filter = "CSV|*.csv|Excel 97-2003|*.xls";
                if (!string.IsNullOrEmpty(cfg.File))
                    ofd.FilterIndex = cfg.File.EndsWith("csv") ? 1 : 2;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var mr = MetadataFactory.GetReader(ofd.FileName);
                    var openMetadataDetailsForm = new MetadataOpen();
                    if (mr.SupportSheets)
                    {
                        openMetadataDetailsForm.MetadataReader = mr;
                        openMetadataDetailsForm.ShowDialog();
                    }

                    StartClick(null);

                    metadataTreeview.Nodes.Clear();

                    mr.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                    Metadata = mr.Read();
                    mr.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                    metadataTreeview.Content=Metadata;

                    if (mr.SupportSheets && openMetadataDetailsForm.Track != "None")
                    {
                        var perspTrack = mr.Read(openMetadataDetailsForm.Track);
                        metadataTreeview.Selection=perspTrack;
                    }

                    cfg.FullFileName = ofd.FileName;

                    EndClick(null);
                }
            }


        }

        private void saveAsMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartClick(null);
            var cfg = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Metadata];
            using (var sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory =cfg.Path;
                sfd.FileName = cfg.File;
                sfd.Filter = "CSV|*.csv|Excel 97-2003|*.xls";
                if(!string.IsNullOrEmpty(cfg.File))
                    sfd.FilterIndex = cfg.File.EndsWith("csv") ? 1 : 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    IMetadataWriter mw = null;
                    switch (Path.GetExtension(sfd.FileName))
                    {
                        case ".csv":
                            mw = new MetadataCsvWriter(sfd.FileName);
                            break;
                        case ".xls":
                        case ".xlsx":
                            mw = new MetadataExcelOleDbWriter(sfd.FileName);
                            var saveForm = new MetadataSave();
                            saveForm.MetadataWriter = mw;
                            if (saveForm.ShowDialog() != DialogResult.OK)
                            {
                                EndClick(null);
                                return;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    mw.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                    mw.Write(Metadata);
                    mw.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                    cfg.FullFileName = sfd.FileName;
                }
            }
            EndClick(null);
        }

        private void extractMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var extractForm = new MetadataExtract();
            var cfg = Configuration.Project.ConnectionStrings[
                Configuration.ConnectionStringCollection.ConnectionClass.Adomd,
                Configuration.ConnectionStringCollection.ConnectionType.Expect
                ];
            extractForm.ConnectionString = cfg.Value;

            if (extractForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StartClick(null);
                var metadataExtractor = extractForm.MetadataExtractor;
                metadataExtractor.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                try
                {
                    Metadata = metadataExtractor.GetMetadata();
                }
                catch (ConnectionException ex)
                {
                    MessageBox.Show(ex.Message, "Cannot connect with connectionString", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    metadataExtractor.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                    if (Metadata!=null)
                        metadataTreeview.Content=Metadata;

                    cfg.Value= extractForm.ConnectionString;

                    EndClick(null);
                }
            }
        }

        private void findMeasuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var findMeasuresForm = new FindMeasures(metadataTreeview.Content);

            if (findMeasuresForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StartClick(null);
                var settings = findMeasuresForm.Settings;
                //findMeasuresForm.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                try
                {
                    switch (settings.Action)
                    {
                        case FindMeasures.SettingsFindMeasures.ActionFind.Select:
                            metadataTreeview.ModifySelection(settings.Match,true);
                            break;
                        case FindMeasures.SettingsFindMeasures.ActionFind.Unselect:
                            metadataTreeview.ModifySelection(settings.Match, false);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unexpected error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    EndClick(null);
                }
            }
        }

    #endregion

    #region Queries Set

        private void createQueriesSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select a path to store the queries generated by NBi on base of your selection in the treeview.";
                fbd.SelectedPath = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if (!ConfirmBuildMdxQueries(fbd.SelectedPath))
                        return;
                    StartClick(null);

                    try
                    {
                        var mb = new MdxBuilder(fbd.SelectedPath);
                        mb.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                        mb.Build(metadataTreeview.Selection, (string)hierarchyFunction.SelectedItem, slicer.Text, notEmpty.Checked);
                        mb.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                    }
                    finally
                    {
                        Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName = fbd.SelectedPath;
                        EndClick(null);
                    }
                }
            }
        }

    #endregion

    #region Results Set

        private void createResultsSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var createForm = new ResultSetCreate();
            createForm.QueriesDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName;
            createForm.ResultsDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName;
            createForm.ConnectionString = Configuration.Project.ConnectionStrings[
                Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                Configuration.ConnectionStringCollection.ConnectionType.Expect
                ].Value;

            DialogResult dialogResult = createForm.ShowDialog();
            if (dialogResult.HasFlag(DialogResult.OK))
            {
                StartClick(null);
                QuerySetManager qsm = null;
                try
                {
                    qsm = QuerySetManager.BuildDefault(createForm.QueriesDirectory, createForm.ResultsDirectory, createForm.ConnectionString);
                    qsm.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                    qsm.PersistResultSets();
                }

                catch (ConnectionException ex)
                {
                    MessageBox.Show(ex.Message, "Cannot connect with connectionString", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    qsm.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                    Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName = createForm.QueriesDirectory;
                    Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName = createForm.ResultsDirectory;
                    Configuration.Project.ConnectionStrings[
                        Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                        Configuration.ConnectionStringCollection.ConnectionType.Expect
                        ].Value = createForm.ConnectionString;

                    EndClick(null);
                }               
            }
        }

    #endregion

    #region TestSuite

        private void buildTestSuiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tsCreate = new TestSuiteCreate();
            tsCreate.Actual.QueriesDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName;
            tsCreate.Actual.ConnectionString = Configuration.Project.ConnectionStrings[
                Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                Configuration.ConnectionStringCollection.ConnectionType.Expect
                ].Value;
            tsCreate.Expect.ResultSetsDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName;

            DialogResult dialogResult = tsCreate.ShowDialog();
            if (dialogResult.HasFlag(DialogResult.OK))
            {
                StartClick(null);
                try
                {
                    var tsb = new TestSuiteBuilder();
                    TestSuiteXml ts = null;

                    tsb.DefineActual(tsCreate.Actual.QueriesDirectory, tsCreate.Actual.ConnectionString);
                    if (tsCreate.Expect.IsResultSetsBased)
                        tsb.DefineExpect(tsCreate.Expect.ResultSetsDirectory);
                    else
                        tsb.DefineExpect(tsCreate.Expect.QueriesDirectory, tsCreate.Expect.ConnectionString);

                    ts = tsb.Build();

                    var xm = new XmlManager();

                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.InitialDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite].FullPath;
                        sfd.FileName = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite].FilenameWithoutExtension;
                        sfd.Filter = "Xml|*.xml";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite].FullFileName = sfd.FileName;
                            xm.Persist(sfd.FileName, ts);
                        }
                    }
                }
                finally
                {
                    Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName = tsCreate.Actual.QueriesDirectory;
                    Configuration.Project.ConnectionStrings[
                        Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                        Configuration.ConnectionStringCollection.ConnectionType.Actual
                        ].Value = tsCreate.Actual.ConnectionString;

                    if (tsCreate.Expect.IsResultSetsBased)
                        Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName = tsCreate.Expect.ResultSetsDirectory;
                    EndClick(null);
                }
            }
        }

        private void openWithNUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cfg = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite];

            var launcher = new NUnit.NUnitLauncher();
            if (!string.IsNullOrEmpty(cfg.FullFileName))
                launcher.Configure(cfg.FullFileName);
            else
                launcher.CleanConfiguration();
            launcher.Open();
        }

        private void runWithNUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cfg = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite];
            
            var launcher = new NUnit.NUnitLauncher();
            if (!string.IsNullOrEmpty(cfg.FullFileName))
                launcher.Configure(cfg.FullFileName);
            else
                launcher.CleanConfiguration();
            launcher.Run();
        }

        private void openWithNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cfg = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite];
            
            System.Diagnostics.Process.Start("notepad.exe", cfg.FullFileName);
        }
    #endregion

         

       

       
        
#endregion

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }
        
    }
}
