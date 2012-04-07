using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Query;
using NBi.Core;

namespace NBi.UI
{
    public partial class MainForm : Form
    {
        protected MetadataAdomdExtractor Metadata { get; set; }
        
        public MainForm()
        {
            InitializeComponent();
            metadataTreeview.Nodes.Clear();
            folderBrowserDialog.RootFolder= System.Environment.SpecialFolder.MyDocuments;
            folderBrowserDialog.SelectedPath = @"C:\Users\Seddryck\Documents\TestCCH\Queries\";
            openFileDialog.InitialDirectory = @"C:\Users\Seddryck\Documents\TestCCH\";
            openFileDialog.FileName = "MyMetadata.xlsx";
        }

        private TreeNode[] MapTreeview(MeasureGroups mgs)
        {
            var tnc = new List<TreeNode>();
            
            foreach (var mg in mgs)
	        {
                
                var mgNode=new TreeNode(mg.Value.Name);
                mgNode.Tag = mg.Key;
                tnc.Add(mgNode);

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

            return tnc.ToArray();
        }

        private void ExtractMetadata_Click(object sender, System.EventArgs e)
        {
            StartClick((Button) sender);

            try
            {
                Metadata = new MetadataAdomdExtractor(connectionString.Text, perspective.Text);
                Metadata.GetMetadata();

                UnregisterEvents(metadataTreeview);
                metadataTreeview.Nodes.Clear();
                metadataTreeview.Nodes.AddRange(MapTreeview(Metadata.MeasureGroups));
                RegisterEvents(metadataTreeview);
            }
            finally
            {
                EndClick((Button)sender);
            }
        }

        private void BuildMdxQueries_Click(object sender, System.EventArgs e)
        {
            if (!ConfirmBuildMdxQueries())
                return; 
            StartClick((Button)sender);

            try
            {
                var mb = new MdxBuilder(folderBrowserDialog.SelectedPath);
                mb.Build(perspective.Text, SelectedMetadata, (string)hierarchyFunction.SelectedItem, slicer.Text, notEmpty.Checked);
            }
            finally
            {
                EndClick((Button) sender);
            }
        }

        private bool ConfirmBuildMdxQueries()
        {
            if (!Directory.Exists(folderBrowserDialog.SelectedPath))
                Directory.CreateDirectory(folderBrowserDialog.SelectedPath);

            if (Directory.GetFiles(folderBrowserDialog.SelectedPath).Length==0)
                return true;

            DialogResult dialogResult = MessageBox.Show(
                string.Format("Target directory {0} is not empty.\nDo you want to clean it before generating the queries?", folderBrowserDialog.SelectedPath),
                "Not empty directory",
                MessageBoxButtons.YesNoCancel);
            
            if (dialogResult == DialogResult.Yes)
            {
                Directory.Delete(folderBrowserDialog.SelectedPath, true);
                Directory.CreateDirectory(folderBrowserDialog.SelectedPath);
            }

            return (dialogResult != DialogResult.Cancel);

        }

        private MeasureGroups SelectedMetadata
        {
            get
            {
                MeasureGroups sel = new MeasureGroups();

                foreach (TreeNode mgNode in metadataTreeview.Nodes)
                {
                    if (mgNode.Checked)
                    {
                        var selMg = Metadata.MeasureGroups[(string)mgNode.Tag].Clone();
                        foreach (TreeNode dimNode in mgNode.FirstNode.Nodes)
                        {
                            if (dimNode.Checked)
                            {
                                var cleanDim = Metadata.Dimensions[(string)dimNode.Tag].Clone(); //NOT A TRUE CLONE !!!!
                                cleanDim.Hierarchies.Clear();

                                selMg.LinkedDimensions.Add(cleanDim);
                                foreach (TreeNode hierarchyNode in dimNode.Nodes)
                                {
                                    if (hierarchyNode.Checked)
                                        selMg.LinkedDimensions[(string)dimNode.Tag].Hierarchies.Add(Metadata.Dimensions[(string)dimNode.Tag].Hierarchies[(string)hierarchyNode.Tag].Clone());
                                }
                            }
                        }
                        foreach (TreeNode measureNode in mgNode.LastNode.Nodes)
                        {
                            if (measureNode.Checked)
                                selMg.Measures.Add(Metadata.MeasureGroups[(string)mgNode.Tag].Measures[(string)measureNode.Tag].Clone());
                        }
                        sel.Add(selMg);
                    }
                }
                return sel;
            }
        }

        private void SelectMetadata(MeasureGroups measureGroups)
        {
            foreach (TreeNode mgNode in metadataTreeview.Nodes)
            {
                mgNode.Checked = measureGroups.ContainsKey((string)mgNode.Tag);

                if (mgNode.Checked)
                {
                    foreach (TreeNode dimNode in mgNode.FirstNode.Nodes)
                    {
                        dimNode.Checked = measureGroups[(string)mgNode.Tag].LinkedDimensions.ContainsKey((string)dimNode.Tag);
                        mgNode.FirstNode.Checked = dimNode.Checked || mgNode.FirstNode.Checked;
                        if (dimNode.Checked)
                        {
                            foreach (TreeNode hierarchyNode in dimNode.Nodes)
                            {
                                hierarchyNode.Checked = measureGroups[(string)mgNode.Tag].LinkedDimensions[(string)dimNode.Tag].Hierarchies.ContainsKey((string)hierarchyNode.Tag);
                            }
                        }
                    }

                    foreach (TreeNode measureNode in mgNode.LastNode.Nodes)
                    {
                        measureNode.Checked = measureGroups[(string)mgNode.Tag].Measures.ContainsKey((string)measureNode.Tag);
                        mgNode.LastNode.Checked = measureNode.Checked || mgNode.LastNode.Checked;
                    }
                }
            }
        }


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

            toolStripProgressBar.Maximum=e.Progress.Total;
            toolStripProgressBar.Value=e.Progress.Current;

            if (DateTime.Now.Subtract(_statusTripLastRefresh).TotalMilliseconds > STATUS_TRIP_REFRESH_RATE)
            {
                statusStrip.Refresh();
                _statusTripLastRefresh = DateTime.Now;
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

        private void Form1_Load(object sender, System.EventArgs e)
        {
            hierarchyFunction.SelectedIndex = 3;
        }

        private void openFolderBrowser_Click(object sender, System.EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
        }

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

        private void openMetadataToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult.HasFlag(DialogResult.OK))
            {
                
                var mr = new MetadataExcelOleDbReader(openFileDialog.FileName);
                var openForm = new MetadataOpen();
                openForm.MetadataReader = mr;
                openForm.ShowDialog();
                
                StartClick(null);

                UnregisterEvents(metadataTreeview);
                metadataTreeview.Nodes.Clear();

                var mgs = new MeasureGroups();
                var dims = new Dimensions();
                mr.ProgressStatusChanged += new MetadataExcelOleDbReader.ProgressStatusHandler(ProgressStatus);
                mr.Read(ref mgs, ref dims);
                Metadata = new MetadataAdomdExtractor(mgs, dims);
                
                metadataTreeview.Nodes.AddRange(MapTreeview(mgs));
                RegisterEvents(metadataTreeview);
                metadataTreeview.Refresh();

                if (openForm.Track != "None")
                {
                    var mgsTrack = new MeasureGroups();
                    var dimsTrack = new Dimensions();
                    mr.Read(openForm.Track, ref mgsTrack, ref dimsTrack);
                    SelectMetadata(mgsTrack);
                }

                mr.ProgressStatusChanged -= new MetadataExcelOleDbReader.ProgressStatusHandler(ProgressStatus);
                EndClick(null);
            }
                
            
        }

        private void saveAsMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult.HasFlag(DialogResult.OK))
            {
                var mw = new MetadataExcelOleDbWriter(saveFileDialog.FileName);
                var saveForm = new MetadataSave();
                saveForm.MetadataWriter = mw;

                DialogResult dr = saveForm.ShowDialog();
                if (dr.HasFlag(DialogResult.OK))
                {
                    StartClick(null);
                    mw.ProgressStatusChanged += new MetadataExcelOleDbWriter.ProgressStatusHandler(ProgressStatus);
                    mw.Write(perspective.Text, Metadata.MeasureGroups);
                    mw.ProgressStatusChanged -= new MetadataExcelOleDbWriter.ProgressStatusHandler(ProgressStatus);
                    EndClick(null);
                }
            }
        }

      
        



       

        
    }
}
