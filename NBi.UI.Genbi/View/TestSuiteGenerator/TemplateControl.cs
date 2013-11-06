using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class TemplateControl : UserControl
    {
        public TemplateControl()
        {
            InitializeComponent();
        }

        internal void DataBind(TemplatePresenter presenter)
        {
            if (presenter != null)
            {
                template.DataBindings.Add("Text", presenter, "Template", true, DataSourceUpdateMode.OnPropertyChanged);
                presenter.PropertyChanged += (sender, e) => { template.Text = presenter.Template; };
            }
        }
    }
}
