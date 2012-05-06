using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI
{
    public class MetadataTreeview : TreeView
    {
        // constants used to hide a checkbox
        public const int TVIF_STATE = 0x8;
        public const int TVIS_STATEIMAGEMASK = 0xF000;
        public const int TV_FIRST = 0x1100;
        public const int TVM_SETITEM = TV_FIRST + 63;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam,
        IntPtr lParam);

        // struct used to set node properties
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;

        }
        
        
        protected CubeMetadata _content;
        public CubeMetadata Content 
        { 
            get
            {
                return _content;
            }
            set
            {
                _content=value;
                UnregisterEvents();
                this.Nodes.Clear();
                this.Nodes.AddRange(MapContent(_content));
                RegisterEvents();
                Refresh();
            }
            
        }

        public CubeMetadata Selection
        {
            get
            {
                return GetSelection();
            }
            set
            {
                SetSelection(value);
            }
        }

        internal TreeNode[] MapContent(CubeMetadata metadata)
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
                            foreach (var l in h.Value.Levels)
                            {
                                var lNode = new TreeNode(l.Value.Caption);
                                lNode.Tag = l.Key;
                                hNode.Nodes.Add(lNode);
                                foreach (var p in l.Value.Properties)
                                {
                                    var propNode = new TreeNode(p.Value.Caption);
                                    propNode.Tag = p.Key;
                                    lNode.Nodes.Add(propNode);
                                }
                            }
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

        internal CubeMetadata GetSelection()
        {
            CubeMetadata sel = new CubeMetadata();

            foreach (TreeNode perspNode in this.Nodes)
            {
                var selPersp = Content.Perspectives[(string)perspNode.Tag].Clone();
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
                                var cleanDim = Content.Perspectives[(string)perspNode.Tag].Dimensions[(string)dimNode.Tag].Clone(); //NOT A TRUE CLONE !!!!
                                cleanDim.Hierarchies.Clear();

                                selMg.LinkedDimensions.Add(cleanDim);
                                foreach (TreeNode hierarchyNode in dimNode.Nodes)
                                {
                                    if (hierarchyNode.Checked)
                                        selMg.LinkedDimensions[(string)dimNode.Tag].Hierarchies.Add(
                                            Content.Perspectives[(string)perspNode.Tag]
                                            .Dimensions[(string)dimNode.Tag]
                                            .Hierarchies[(string)hierarchyNode.Tag].Clone());

                                    foreach (TreeNode levelNode in hierarchyNode.Nodes)
                                    {
                                        if (levelNode.Checked)
                                            selMg.LinkedDimensions[(string)dimNode.Tag]
                                                .Hierarchies[(string)hierarchyNode.Tag].Levels.Add((string)levelNode.Tag,
                                                Content.Perspectives[(string)perspNode.Tag]
                                                .Dimensions[(string)dimNode.Tag]
                                                .Hierarchies[(string)hierarchyNode.Tag]
                                                .Levels[(string)levelNode.Tag]
                                                .Clone());
                                    }
                                }
                            }
                        }
                        foreach (TreeNode measureNode in mgNode.LastNode.Nodes)
                        {
                            if (measureNode.Checked)
                                selMg.Measures.Add(Content.Perspectives[(string)perspNode.Tag].MeasureGroups[(string)mgNode.Tag].Measures[(string)measureNode.Tag].Clone());
                        }
                    }

                }
            }
            return sel;
        }

        internal void SetSelection(CubeMetadata selection)
        {
            foreach (TreeNode pNode in this.Nodes)
            {
                pNode.Checked = selection.Perspectives.ContainsKey((string)pNode.Tag);
                if (pNode.Checked)
                {
                    var perspective = selection.Perspectives[(string)pNode.Tag];
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
                                        hierarchyNode.Checked = perspective.MeasureGroups[(string)mgNode.Tag].
                                            LinkedDimensions[(string)dimNode.Tag].
                                            Hierarchies.ContainsKey((string)hierarchyNode.Tag);
                                        if (hierarchyNode.Checked)
                                        {
                                            foreach (TreeNode levelNode in hierarchyNode.Nodes)
                                            {
                                                levelNode.Checked = perspective.MeasureGroups[(string)mgNode.Tag].
                                                    LinkedDimensions[(string)dimNode.Tag].
                                                    Hierarchies[(string)hierarchyNode.Tag].
                                                    Levels.ContainsKey((string)levelNode.Tag);
                                            }
                                        }
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

        protected void RegisterEvents()
        {
            this.AfterCheck += node_AfterCheck;
        }

        protected void UnregisterEvents()
        {
            this.AfterCheck -= node_AfterCheck;
        }

        public void CheckAll()
        {
            foreach (TreeNode t in this.Nodes)
            {
                t.Checked = true;
                node_AfterCheck(this, new TreeViewEventArgs(t, TreeViewAction.ByMouse));
            }
        }

        public void UncheckAll()
        {
            foreach (TreeNode t in this.Nodes)
            {
                t.Checked = false;
                node_AfterCheck(this, new TreeViewEventArgs(t, TreeViewAction.ByMouse));
            }
        }
        
        public MetadataTreeview()
        {
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.DrawNode += new DrawTreeNodeEventHandler(tree_DrawNode);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        void tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 6)
                HideCheckBox(e.Node);
            e.DrawDefault = true;
        }

        private void HideCheckBox(TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            IntPtr lparam = Marshal.AllocHGlobal(Marshal.SizeOf(tvi));
            Marshal.StructureToPtr(tvi, lparam, false);
            SendMessage(this.Handle, TVM_SETITEM, IntPtr.Zero, lparam);
        }

    }
}
