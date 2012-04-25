using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Query;
using NBi.Core;
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

#region Treeview

        private TreeNode[] MapTreeview(CubeMetadata metadata)
        {
            var tnc = new List<TreeNode>();
            foreach (var perspective in metadata.Perspectives)
            {
                var pNode = new TreeNode(perspective.Value.Name);
                pNode.Tag = perspective.Key;
                tnc.Add(pNode);

                foreach (var mg in perspective.Value.MeasureGroups)
                {
                    var mgNode = new TreeNode(mg.Value.Name);
                    mgNode.Tag = mg.Key;
                    pNode.Nodes.Add(mgNode);

                    var dimsNode = new TreeNode("Linked dimensions");
                    mgNode.Nodes.Add(dimsNode);
                    foreach (var dim in mg.Value.LinkedDimensions)
                    {
                        var dimNode = new TreeNode(dim.Value.Caption);
                        dimNode.Tag = dim.Key;
                        dimsNode.Nodes.Add(dimNode);
                        foreach (var h in dim.Value.Hierarchies)
                        {
                            var hNode = new TreeNode(h.Value.Caption);
                            hNode.Tag = h.Key;
                            dimNode.Nodes.Add(hNode);
                        }
                    }

                    var measuresNode = new TreeNode("Measures");
                    mgNode.Nodes.Add(measuresNode);
                    foreach (var measure in mg.Value.Measures)
                    {
                        var measureNode = new TreeNode(measure.Value.Caption);
                        measureNode.Tag = measure.Key;
                        measuresNode.Nodes.Add(measureNode);
                    }
                }
            }

            return tnc.ToArray();
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

        private CubeMetadata SelectedMetadata
        {
            get
            {
                CubeMetadata sel = new CubeMetadata();

                foreach (TreeNode perspNode in metadataTreeview.Nodes)
                {
                    var selPersp = Metadata.Perspectives[(string)perspNode.Tag].Clone();
                    sel.Perspectives.Add(selPersp);
                    foreach (TreeNode mgNode in perspNode.Nodes)
                    {
                        if (mgNode.Checked)
                        {
                            var selMg = selPersp.MeasureGroups[(string)mgNode.Tag];
                            foreach (TreeNode dimNode in mgNode.FirstNode.Nodes)
                            {
                                if (dimNode.Checked)
                                {
                                    var cleanDim = Metadata.Perspectives[(string)perspNode.Tag].Dimensions[(string)dimNode.Tag].Clone(); //NOT A TRUE CLONE !!!!
                                    cleanDim.Hierarchies.Clear();

                                    selMg.LinkedDimensions.Add(cleanDim);
                                    foreach (TreeNode hierarchyNode in dimNode.Nodes)
                                    {
                                        if (hierarchyNode.Checked)
                                            selMg.LinkedDimensions[(string)dimNode.Tag].Hierarchies.Add(Metadata.Perspectives[(string)perspNode.Tag].Dimensions[(string)dimNode.Tag].Hierarchies[(string)hierarchyNode.Tag].Clone());
                                    }
                                }
                            }
                            foreach (TreeNode measureNode in mgNode.LastNode.Nodes)
                            {
                                if (measureNode.Checked)
                                    selMg.Measures.Add(Metadata.Perspectives[(string)perspNode.Tag].MeasureGroups[(string)mgNode.Tag].Measures[(string)measureNode.Tag].Clone());
                            }
                            
                        }
                        
                    }
                }
                return sel;
            }
        }

        private void SelectMetadata(CubeMetadata metadata)
        {
            foreach (TreeNode pNode in metadataTreeview.Nodes)
            {
                pNode.Checked = metadata.Perspectives.ContainsKey((string)pNode.Tag);
                if (pNode.Checked)
                {
                    var perspective = metadata.Perspectives[(string)pNode.Tag];
                    foreach (TreeNode mgNode in pNode.Nodes)
                    {
                        mgNode.Checked = perspective.MeasureGroups.ContainsKey((string)mgNode.Tag);

                        if (mgNode.Checked)
                        {
                            foreach (TreeNode dimNode in mgNode.FirstNode.Nodes)
                            {
                                dimNode.Checked = perspective.MeasureGroups[(string)mgNode.Tag].LinkedDimensions.ContainsKey((string)dimNode.Tag);
                                mgNode.FirstNode.Checked = dimNode.Checked || mgNode.FirstNode.Checked;
                                if (dimNode.Checked)
                                {
                                    foreach (TreeNode hierarchyNode in dimNode.Nodes)
                                    {
                                        hierarchyNode.Checked = perspective.MeasureGroups[(string)mgNode.Tag].LinkedDimensions[(string)dimNode.Tag].Hierarchies.ContainsKey((string)hierarchyNode.Tag);
                                    }
                                }
                            }

                            foreach (TreeNode measureNode in mgNode.LastNode.Nodes)
                            {
                                measureNode.Checked = perspective.MeasureGroups[(string)mgNode.Tag].Measures.ContainsKey((string)measureNode.Tag);
                                mgNode.LastNode.Checked = measureNode.Checked || mgNode.LastNode.Checked;
                            }
                        }
                    }
                }
            }
        }

        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void CheckParentNode(TreeNode treeNode, bool nodeChecked)
        {
            if (treeNode == null) return;
            if (treeNode.Parent == null) return;

            if (!nodeChecked)
                foreach (TreeNode cousinNode in treeNode.Parent.Nodes)
                    if (cousinNode.Checked)
                        return;

            treeNode.Parent.Checked = nodeChecked;
            CheckParentNode(treeNode.Parent, nodeChecked);
        }

        // NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
        // After a tree node's Checked property is changed, all its child nodes are updated to the same value.
        private void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckParentNode(e.Node, e.Node.Checked);
                }
            }
        }

        private void RegisterEvents(TreeView tv)
        {
            tv.AfterCheck += node_AfterCheck;
        }

        private void UnregisterEvents(TreeView tv)
        {
            tv.AfterCheck -= node_AfterCheck;
        }


#endregion

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
            foreach (TreeNode t in metadataTreeview.Nodes)
            {
                t.Checked = false;
                node_AfterCheck(metadataTreeview, new TreeViewEventArgs(t, TreeViewAction.ByMouse));
            }
        }

        private void selectAllMetadata_Click(object sender, System.EventArgs e)
        {
            foreach (TreeNode t in metadataTreeview.Nodes)
            {
                t.Checked = true;
                node_AfterCheck(metadataTreeview, new TreeViewEventArgs(t, TreeViewAction.ByMouse));
            }
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

                    UnregisterEvents(metadataTreeview);
                    metadataTreeview.Nodes.Clear();

                    mr.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                    Metadata = mr.Read();
                    mr.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                    metadataTreeview.Nodes.AddRange(MapTreeview(Metadata));
                    RegisterEvents(metadataTreeview);
                    metadataTreeview.Refresh();

                    if (mr.SupportSheets && openMetadataDetailsForm.Track != "None")
                    {
                        var perspTrack = mr.Read(openMetadataDetailsForm.Track);
                        SelectMetadata(perspTrack);
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

            if (extractForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StartClick(null);
                try
                {
                    var metadataExtractor = extractForm.MetadataExtractor;
                    metadataExtractor.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                    Metadata = metadataExtractor.GetMetadata();
                    metadataExtractor.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                    UnregisterEvents(metadataTreeview);
                    metadataTreeview.Nodes.Clear();
                    metadataTreeview.Nodes.AddRange(MapTreeview(Metadata));
                    RegisterEvents(metadataTreeview);
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
                        mb.Build(SelectedMetadata, (string)hierarchyFunction.SelectedItem, slicer.Text, notEmpty.Checked);
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
            var createForm = new TestSuiteCreate();
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
                var qsm = QuerySetManager.BuildDefault(createForm.QueriesDirectory, createForm.ResultsDirectory, createForm.ConnectionString);
                qsm.ProgressStatusChanged += new ProgressStatusHandler(ProgressStatus);
                qsm.PersistResultSets();
                qsm.ProgressStatusChanged -= new ProgressStatusHandler(ProgressStatus);

                Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName=createForm.QueriesDirectory;
                Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName=createForm.ResultsDirectory;
                Configuration.Project.ConnectionStrings[
                    Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                    Configuration.ConnectionStringCollection.ConnectionType.Expect
                    ].Value=createForm.ConnectionString;

                EndClick(null);
            }
        }

    #endregion

    #region TestSuite

        private void buildTestSuiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tsCreate = new TestSuiteCreate();
            tsCreate.QueriesDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName;
            tsCreate.ResultsDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName;
            tsCreate.ConnectionString = Configuration.Project.ConnectionStrings[
                Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                Configuration.ConnectionStringCollection.ConnectionType.Actual
                ].Value;

            if (tsCreate.ShowDialog() == DialogResult.OK)
            {
                var xm = new XmlManager();
                var ts = xm.BuildTestSuite(tsCreate.QueriesDirectory, tsCreate.ResultsDirectory, tsCreate.ConnectionString);
                using (var sfd = new SaveFileDialog())
                {
                    sfd.InitialDirectory = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite].FullPath;
                    sfd.FileName = Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite].FilenameWithoutExtension;
                    sfd.Filter = "Xml|*.xml";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        xm.Persist(sfd.FileName, ts);
                        Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.TestSuite].FullFileName = sfd.FileName;
                        Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Query].FullFileName = tsCreate.QueriesDirectory;
                        Configuration.Project.Directories[Configuration.DirectoryCollection.DirectoryType.Expect].FullFileName = tsCreate.ResultsDirectory;
                        Configuration.Project.ConnectionStrings[
                            Configuration.ConnectionStringCollection.ConnectionClass.Oledb,
                            Configuration.ConnectionStringCollection.ConnectionType.Actual
                            ].Value = tsCreate.ConnectionString;
                    }
                }
            }
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
    #endregion
        
#endregion
        
    }
}
