using System;
using System.ComponentModel;
using System.Linq;
using NBi.Service;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Template;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL;
using NBi.GenbiL.Stateful;

namespace NBi.UI.Genbi.Presenter
{
    class TemplatePresenter : PresenterBase
    {
        private readonly GenerationState state;
        public bool IsModified {get; private set;}

        public TemplatePresenter(GenerationState state)
        {
            EmbeddedTemplateLabels = new BindingList<string>();

            this.state = state;
            ReloadEmbeddedTemplateLabels();

            var window = new OpenTemplateWindow(EmbeddedTemplateLabels);


            OpenTemplateCommand = new OpenTemplateCommand(this, window);
            SaveTemplateCommand = new SaveTemplateCommand(this);

            Template = string.Empty;
            IsModified = false;
            SaveTemplateCommand.Refresh();
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
            if (state.Template.PredefinedLabels.Count()==0)
            {
                var action = new ListPredefinedTemplateAction();
                action.Execute(state);
            }

            foreach (var label in state.Template.PredefinedLabels)
                EmbeddedTemplateLabels.Add(label);
        }

        internal void LoadExternalTemplate(string fullPath)
        {
            var action = new LoadExternalTemplateAction(fullPath);
            action.Execute(state);
            IsModified = false;
            OnPropertyChanged("Template");
        }

        internal void LoadEmbeddedTemplate(string name)
        {
            var action = new LoadPredefinedTemplateAction(name);
            action.Execute(state);
            IsModified = false;
            OnPropertyChanged("Template");
        }

        internal void Save(string fullPath)
        {
            var action = new SaveTemplateAction(fullPath);
            action.Execute(state);
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
