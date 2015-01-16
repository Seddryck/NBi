using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.TestSuiteGenerator.Events;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class OpenTemplateWindow : Form
    {
        
        public enum TemplateType
        {
            Embedded,
            External
        }

        internal TestSuiteView Origin { get; set; }
        public string EmbeddedName
        {
            get
            {
                return predefinedTemplateName.SelectedValue.ToString().Replace(" ", "");
            }
        }
        public string FullPath
        {
            get
            {
                return userTemplateFullPath.Text;
            }
        }
        public TemplateType Type
        {
            get
            {
                if (isUserTemplate.Checked)
                    return TemplateType.External;
                return TemplateType.Embedded;
            }
        }

        public OpenTemplateWindow(BindingList<string> embeddedTemplates)
        {
            InitializeComponent();

            EmbeddedTemplates = embeddedTemplates;
            DeclareBindings();
        }

        private void DeclareBindings()
        {
            predefinedTemplateName.DataSource = EmbeddedTemplates;
        }

        public BindingList<string> EmbeddedTemplates { get; set; }
        

        private void OpenTemplateView_Load(object sender, EventArgs e)
        {
            TemplateChoice_CheckedChanged(this, EventArgs.Empty);
        }

        private void Apply_Click(object sender, EventArgs e)
        {
        }

        private void TemplateChoice_CheckedChanged(object sender, EventArgs e)
        {
            userTemplateFullPath.Enabled = isUserTemplate.Checked;
            openUserTemplate.Enabled = isUserTemplate.Checked;
            predefinedTemplateName.Enabled = isPredefinedTemplate.Checked;
        }

        private void UserTemplateFileSelection_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|NBi Test Template Files (*.nbitt)|*.nbitt|Text Files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            DialogResult result = openFileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
                userTemplateFullPath.Text = openFileDialog.FileName;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
