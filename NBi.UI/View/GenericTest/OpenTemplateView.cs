using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Interface;

namespace NBi.UI.View.GenericTest
{
    public partial class OpenTemplateView : Form
    {
        protected CsvImporterView Origin { get; set; }
        public string EmbeddedName { get; set; }
        public string FullPath { get; set; }

        public OpenTemplateView(CsvImporterView origin)
        {
            Origin = origin;
            InitializeComponent();
            DeclareBindings();
        }

        private void DeclareBindings()
        {
            predefinedTemplateName.DataSource = BindingEmbeddedTemplates;
        }

        private void OpenTemplateView_Load(object sender, EventArgs e)
        {
            TemplateChoice_CheckedChanged(this, EventArgs.Empty);
        }

        public event EventHandler<VariableRenamedEventArgs> Apply;
        public void InvokeApply(VariableRenamedEventArgs e)
        {
            EventHandler<VariableRenamedEventArgs> handler = Apply;
            if (handler != null)
                handler(this, e);
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            NewTemplateSelectedEventArgs eventArgs = null;
            if (isUserTemplate.Checked)
            {
                eventArgs = new NewTemplateSelectedEventArgs(
                    NewTemplateSelectedEventArgs.TemplateType.External, 
                    userTemplateFullPath.Text);
            }
            else
            {
                eventArgs = new NewTemplateSelectedEventArgs(
                    NewTemplateSelectedEventArgs.TemplateType.Embedded,
                    predefinedTemplateName.SelectedValue.ToString());
            }
            Origin.InvokeNewTemplateSelected(eventArgs);
            this.Hide();
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
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                userTemplateFullPath.Text = openFileDialog.FileName;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
