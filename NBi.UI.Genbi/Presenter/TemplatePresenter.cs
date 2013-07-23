using System;
using System.ComponentModel;
using System.Linq;
using NBi.Service;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Template;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Presenter
{
    class TemplatePresenter : BasePresenter<ITemplateView>
    {
        private readonly TemplateManager templateManager;
        public bool IsModified {get; private set;}

        public TemplatePresenter(ITemplateView templateView, TemplateManager templateManager)
            : base(templateView)
        {
            EmbeddedTemplateLabels = new BindingList<string>();

            this.templateManager = templateManager;
            ReloadEmbeddedTemplateLabels();

            var window = new OpenTemplateWindow(EmbeddedTemplateLabels);


            OpenTemplateCommand = new OpenTemplateCommand(this, window);
            SaveTemplateCommand = new SaveTemplateCommand(this);

            Template = string.Empty;
            IsModified = false;
        }

        #region Bindable properties

        public BindingList<string> EmbeddedTemplateLabels
        {
            get { return GetValue<BindingList<string>>("EmbeddedTemplateLabels"); }
            set { SetValue("EmbeddedTemplateLabels", value); }
        }

        public string Template
        {
            get { return GetValue<string>("Template"); }
            set { SetValue("Template", value); }
        }

        //public string Template { get; set; }
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "Template":
                    OnTemplateChanged(EventArgs.Empty);
                    IsModified = true;
                    this.SaveTemplateCommand.Refresh();
                    break;
                default:
                    break;
            }
            
        }

        private void ReloadEmbeddedTemplateLabels()
        {
            EmbeddedTemplateLabels.Clear();
            foreach (var label in templateManager.GetEmbeddedLabels())
                EmbeddedTemplateLabels.Add(label);
        }

        internal void LoadExternalTemplate(string fullPath)
        {
            var templateManager = new TemplateManager();
            Template = templateManager.GetExternalTemplate(fullPath);
            OnPropertyChanged("Template");
        }

        internal void LoadEmbeddedTemplate(string name)
        {
            var templateManager = new TemplateManager();
            Template = templateManager.GetEmbeddedTemplate(name);
            OnPropertyChanged("Template");
        }

        internal void Save(string fullPath)
        {
            var manager = new TemplateManager();
            manager.Persist(fullPath, Template);
            IsModified = false;
            this.SaveTemplateCommand.Refresh();
        }

        public ICommand OpenTemplateCommand { get; private set; }
        public ICommand SaveTemplateCommand { get; private set; }

        public event EventHandler<EventArgs> TemplateChanged;

        protected void OnTemplateChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = TemplateChanged;
            if (handler != null)
                handler(this, e);
        }
        
    }
}
