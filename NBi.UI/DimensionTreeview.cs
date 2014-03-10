using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI
{
    public class DimensionTreeview : TreeView
    {
        // constants used to hide a checkbox
        public const int TVIF_STATE = 0x8;
        public const int TVIS_STATEIMAGEMASK = 0xF000;
        public const int TV_FIRST = 0x1000;
        public const int TVM_SETITEM = TV_FIRST + 76;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam,
        IntPtr lParam);

        // struct used to set node properties
        [StructLayout(LayoutKind.Sequential, Pack=8, CharSet=CharSet.Auto)]
        private struct TVITEM
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
        
        
        protected DimensionCollection _content;
        public DimensionCollection Content 
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

        public DimensionCollection Selection
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

        internal TreeNode[] MapContent(DimensionCollection metadata)
        {
            var tnc = new List<TreeNode>();

            if (metadata == null)
                return tnc.ToArray();

            foreach (var dim in metadata)
            {
                var dimNode = new TreeNode(dim.Value.Caption);
                dimNode.Tag = dim.Key;
                tnc.Add(dimNode);
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
            return tnc.ToArray();
        }

        internal DimensionCollection GetSelection()
        {
            var sel = new DimensionCollection();

            if (this.Nodes == null || Content==null)
                return null;

            foreach (TreeNode dimNode in this.Nodes)
            {
                if (dimNode.Checked)
                {
                    var cleanDim = Content[(string)dimNode.Tag].Clone(); 
                    cleanDim.Hierarchies.Clear();

                    sel.Add(cleanDim);
                    foreach (TreeNode hierarchyNode in dimNode.Nodes)
                    {
                        if (hierarchyNode.Checked)
                        {
                            var cleanHierarchy = Content[(string)dimNode.Tag]
                                .Hierarchies[(string)hierarchyNode.Tag].Clone();
                            cleanHierarchy.Levels.Clear();

                            sel[(string)dimNode.Tag].Hierarchies.Add(cleanHierarchy);

                            foreach (TreeNode levelNode in hierarchyNode.Nodes)
                            {
                                if (levelNode.Checked)
                                    sel[(string)dimNode.Tag]
                                        .Hierarchies[(string)hierarchyNode.Tag].Levels.Add((string)levelNode.Tag,
                                        sel[(string)dimNode.Tag]
                                        .Hierarchies[(string)hierarchyNode.Tag]
                                        .Levels[(string)levelNode.Tag]
                                        .Clone());
                            }
                        }
                    }
                }
            }
            return sel;
        }

        internal void SetSelection(DimensionCollection selection)
        {
           
            foreach (TreeNode dimNode in this.Nodes)
            {
                dimNode.Checked = selection.ContainsKey((string)dimNode.Tag);
                if (dimNode.Checked)
                {
                    foreach (TreeNode hierarchyNode in dimNode.Nodes)
                    {
                        hierarchyNode.Checked = selection[(string)dimNode.Tag].
                            Hierarchies.ContainsKey((string)hierarchyNode.Tag);
                        if (hierarchyNode.Checked)
                        {
                            foreach (TreeNode levelNode in hierarchyNode.Nodes)
                            {
                                levelNode.Checked = selection[(string)dimNode.Tag].
                                    Hierarchies[(string)hierarchyNode.Tag].
                                    Levels.ContainsKey((string)levelNode.Tag);
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
        
        public DimensionTreeview()
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
            if (e.Node.Level == 3)
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
