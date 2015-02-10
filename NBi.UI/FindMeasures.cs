using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NBi.Core.Analysis.Metadata;

namespace NBi.UI
{
    public partial class FindMeasures : Form
    {
        
        protected CubeMetadata _metadata { get; set; }
        public SettingsFindMeasures Settings { get; protected set; }

        public class SettingsFindMeasures
        {
            public enum ActionFind
            {
                Select,
                Unselect
            }

            public CubeMetadata Match { get; internal set; }
            public ActionFind Action { get; internal set; }
        }

        public FindMeasures(CubeMetadata metadata)
        {
            InitializeComponent();
            _metadata = metadata;
            Settings = new SettingsFindMeasures();
        }

        private void FindMeasures_Load(object sender, EventArgs e)
        {

        }

        private void apply_Click(object sender, EventArgs e)
        {
            Settings.Match = _metadata.FindMeasures(pattern.Text);
            var count = Settings.Match.GetMeasuresCount();

            if (count == 0)
                MessageBox.Show("No measure found matching this pattern!", "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var diagAction = MessageBox.Show(String.Format("{0} measures found matching this pattern.\r\nClick on \"Yes\" to add them to the selection. Click on \"No\" to remove them to the selection or click on \"Cancel\" to abort the selection change.", count), "Results", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (diagAction)
                {
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.No:
                        Settings.Action = SettingsFindMeasures.ActionFind.Unselect;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                        break;
                    case DialogResult.Yes:
                        Settings.Action = SettingsFindMeasures.ActionFind.Select;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
