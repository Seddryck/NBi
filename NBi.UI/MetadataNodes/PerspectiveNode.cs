using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI.MetadataNodes
{
    class PerspectiveNode : TreeNode
    {
        public PerspectiveNode(Perspective perspective) : base(perspective.Name)
        {
            Tag = perspective;
        }
    }
}
