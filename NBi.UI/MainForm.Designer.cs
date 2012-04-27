namespace NBi.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Linked dimensions");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Measures");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Measure group", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("MG2");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.metadataTreeview = new System.Windows.Forms.TreeView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.notEmpty = new System.Windows.Forms.CheckBox();
            this.hierarchyFunction = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.slicer = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.unselectAllMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.metadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queriesSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createQueriesSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createResultsSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsSuiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildTestSuiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runWithNUnitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithNUnitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // metadataTreeview
            // 
            this.metadataTreeview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metadataTreeview.CheckBoxes = true;
            this.metadataTreeview.FullRowSelect = true;
            this.metadataTreeview.Location = new System.Drawing.Point(5, 27);
            this.metadataTreeview.Name = "metadataTreeview";
            treeNode9.Name = "Node2";
            treeNode9.Text = "Linked dimensions";
            treeNode10.Name = "Node3";
            treeNode10.Text = "Measures";
            treeNode11.Name = "MG1";
            treeNode11.Text = "Measure group";
            treeNode12.Name = "Node1";
            treeNode12.Text = "MG2";
            this.metadataTreeview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12});
            this.metadataTreeview.Size = new System.Drawing.Size(312, 352);
            this.metadataTreeview.TabIndex = 1;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 382);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(645, 22);
            this.statusStrip.TabIndex = 2;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(97, 17);
            this.toolStripStatus.Text = "No info provided";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(323, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Slicer";
            // 
            // notEmpty
            // 
            this.notEmpty.AutoSize = true;
            this.notEmpty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.notEmpty.Location = new System.Drawing.Point(559, 29);
            this.notEmpty.Name = "notEmpty";
            this.notEmpty.Size = new System.Drawing.Size(74, 17);
            this.notEmpty.TabIndex = 11;
            this.notEmpty.Text = "Not empty";
            this.notEmpty.UseVisualStyleBackColor = true;
            // 
            // hierarchyFunction
            // 
            this.hierarchyFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hierarchyFunction.FormattingEnabled = true;
            this.hierarchyFunction.Items.AddRange(new object[] {
            "AllMembers",
            "Ancestors",
            "Ascendants",
            "Children",
            "Descendants",
            "Members"});
            this.hierarchyFunction.Location = new System.Drawing.Point(425, 27);
            this.hierarchyFunction.Name = "hierarchyFunction";
            this.hierarchyFunction.Size = new System.Drawing.Size(121, 21);
            this.hierarchyFunction.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(323, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Hierarchy function:";
            // 
            // slicer
            // 
            this.slicer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slicer.Location = new System.Drawing.Point(326, 68);
            this.slicer.Multiline = true;
            this.slicer.Name = "slicer";
            this.slicer.Size = new System.Drawing.Size(307, 311);
            this.slicer.TabIndex = 14;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem1,
            this.metadataToolStripMenuItem,
            this.queriesSetToolStripMenuItem,
            this.resultSetToolStripMenuItem,
            this.testsSuiteToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(645, 24);
            this.menuStrip.TabIndex = 15;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openProjectToolStripMenuItem.Image")));
            this.openProjectToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.openProjectToolStripMenuItem.Text = "&Open ...";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(140, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsProjectToolStripMenuItem
            // 
            this.saveAsProjectToolStripMenuItem.Name = "saveAsProjectToolStripMenuItem";
            this.saveAsProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveAsProjectToolStripMenuItem.Text = "Save &As ...";
            this.saveAsProjectToolStripMenuItem.Click += new System.EventHandler(this.saveAsProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Enabled = false;
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Enabled = false;
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Enabled = false;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllMetadata,
            this.unselectAllMetadata});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem1.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(186, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(186, 6);
            // 
            // selectAllMetadata
            // 
            this.selectAllMetadata.Name = "selectAllMetadata";
            this.selectAllMetadata.Size = new System.Drawing.Size(189, 22);
            this.selectAllMetadata.Text = "Select All Metadata";
            this.selectAllMetadata.Click += new System.EventHandler(this.selectAllMetadata_Click);
            // 
            // unselectAllMetadata
            // 
            this.unselectAllMetadata.Name = "unselectAllMetadata";
            this.unselectAllMetadata.Size = new System.Drawing.Size(189, 22);
            this.unselectAllMetadata.Text = "Unselect All Metadata";
            this.unselectAllMetadata.Click += new System.EventHandler(this.unselectAllMetadata_Click);
            // 
            // metadataToolStripMenuItem
            // 
            this.metadataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMetadataToolStripMenuItem,
            this.saveAsMetadataToolStripMenuItem,
            this.extractMetadataToolStripMenuItem});
            this.metadataToolStripMenuItem.Name = "metadataToolStripMenuItem";
            this.metadataToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.metadataToolStripMenuItem.Text = "Metadata";
            // 
            // openMetadataToolStripMenuItem
            // 
            this.openMetadataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openMetadataToolStripMenuItem.Image")));
            this.openMetadataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openMetadataToolStripMenuItem.Name = "openMetadataToolStripMenuItem";
            this.openMetadataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openMetadataToolStripMenuItem.Text = "&Open ...";
            this.openMetadataToolStripMenuItem.Click += new System.EventHandler(this.openMetadataToolStripMenuItem_Click);
            // 
            // saveAsMetadataToolStripMenuItem
            // 
            this.saveAsMetadataToolStripMenuItem.Name = "saveAsMetadataToolStripMenuItem";
            this.saveAsMetadataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsMetadataToolStripMenuItem.Text = "Save &As ...";
            this.saveAsMetadataToolStripMenuItem.Click += new System.EventHandler(this.saveAsMetadataToolStripMenuItem_Click);
            // 
            // extractMetadataToolStripMenuItem
            // 
            this.extractMetadataToolStripMenuItem.Name = "extractMetadataToolStripMenuItem";
            this.extractMetadataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.extractMetadataToolStripMenuItem.Text = "E&xtract ...";
            this.extractMetadataToolStripMenuItem.Click += new System.EventHandler(this.extractMetadataToolStripMenuItem_Click);
            // 
            // queriesSetToolStripMenuItem
            // 
            this.queriesSetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createQueriesSetToolStripMenuItem});
            this.queriesSetToolStripMenuItem.Name = "queriesSetToolStripMenuItem";
            this.queriesSetToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.queriesSetToolStripMenuItem.Text = "Queries Set";
            // 
            // createQueriesSetToolStripMenuItem
            // 
            this.createQueriesSetToolStripMenuItem.Name = "createQueriesSetToolStripMenuItem";
            this.createQueriesSetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createQueriesSetToolStripMenuItem.Text = "Create ...";
            this.createQueriesSetToolStripMenuItem.Click += new System.EventHandler(this.createQueriesSetToolStripMenuItem_Click);
            // 
            // resultSetToolStripMenuItem
            // 
            this.resultSetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createResultsSetToolStripMenuItem});
            this.resultSetToolStripMenuItem.Name = "resultSetToolStripMenuItem";
            this.resultSetToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.resultSetToolStripMenuItem.Text = "Results Set";
            // 
            // createResultsSetToolStripMenuItem
            // 
            this.createResultsSetToolStripMenuItem.Name = "createResultsSetToolStripMenuItem";
            this.createResultsSetToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.createResultsSetToolStripMenuItem.Text = "Create...";
            this.createResultsSetToolStripMenuItem.Click += new System.EventHandler(this.createResultsSetToolStripMenuItem_Click);
            // 
            // testsSuiteToolStripMenuItem
            // 
            this.testsSuiteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildTestSuiteToolStripMenuItem,
            this.openWithNUnitToolStripMenuItem,
            this.runWithNUnitToolStripMenuItem});
            this.testsSuiteToolStripMenuItem.Name = "testsSuiteToolStripMenuItem";
            this.testsSuiteToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.testsSuiteToolStripMenuItem.Text = "Tests Suite";
            // 
            // buildTestSuiteToolStripMenuItem
            // 
            this.buildTestSuiteToolStripMenuItem.Name = "buildTestSuiteToolStripMenuItem";
            this.buildTestSuiteToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.buildTestSuiteToolStripMenuItem.Text = "Build ...";
            this.buildTestSuiteToolStripMenuItem.Click += new System.EventHandler(this.buildTestSuiteToolStripMenuItem_Click);
            // 
            // runWithNUnitToolStripMenuItem
            // 
            this.runWithNUnitToolStripMenuItem.Name = "runWithNUnitToolStripMenuItem";
            this.runWithNUnitToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.runWithNUnitToolStripMenuItem.Text = "Run with NUnit ...";
            this.runWithNUnitToolStripMenuItem.Click += new System.EventHandler(this.runWithNUnitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Enabled = false;
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Enabled = false;
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Enabled = false;
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Enabled = false;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Enabled = false;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // openWithNUnitToolStripMenuItem
            // 
            this.openWithNUnitToolStripMenuItem.Name = "openWithNUnitToolStripMenuItem";
            this.openWithNUnitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openWithNUnitToolStripMenuItem.Text = "Open with NUnit ...";
            this.openWithNUnitToolStripMenuItem.Click += new System.EventHandler(this.openWithNUnitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 404);
            this.Controls.Add(this.slicer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.hierarchyFunction);
            this.Controls.Add(this.notEmpty);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.metadataTreeview);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView metadataTreeview;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox notEmpty;
        private System.Windows.Forms.ComboBox hierarchyFunction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox slicer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unselectAllMetadata;
        private System.Windows.Forms.ToolStripMenuItem selectAllMetadata;
        private System.Windows.Forms.ToolStripMenuItem resultSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createResultsSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queriesSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createQueriesSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testsSuiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildTestSuiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runWithNUnitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWithNUnitToolStripMenuItem;
    }
}

