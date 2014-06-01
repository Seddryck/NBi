using NBi.Service.Dto;
using NBi.UI.Genbi.Command;

namespace NBi.UI.Genbi.Presenter
{
    abstract class DocumentPresenterBase : PresenterBase
    {
        private readonly DocumentBase document;

        protected DocumentPresenterBase(DocumentBase document)
        {
            this.document = document;
            this.AutoIndentCommand = new AutoIndentCommand(this);

            this.RefreshProperties();
        }

        #region Bindable properties

        public string Name
        {
            get { return this.GetValue<string>("Name"); }
            set { this.SetValue("Name", value); }
        }

        public string Path
        {
            get { return this.GetValue<string>("Path"); }
            set { this.SetValue("Path", value); }
        }

        public string FullPath
        {
            get { return this.GetValue<string>("FullPathProperty"); }
            set { this.SetValue("FullPathProperty", value); }
        }

        public bool IsDirty
        {
            get { return this.GetValue<bool>("IsDirty"); }
            set { this.SetValue("IsDirty", value); }
        }

        public bool IsPersistent
        {
            get { return this.GetValue<bool>("IsPersistent"); }
            set { this.SetValue("IsPersistent", value); }
        }

        public string Text
        {
            get { return this.GetValue<string>("Text"); }
            set { this.SetValue("Text", value); }
        }

        public string Output
        {
            get { return this.GetValue<string>("Output"); }
            set { this.SetValue("Output", value); }
        }

        #endregion

        #region Commands definitions

        public ICommand AutoIndentCommand { get; private set; }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case "Text":
                    this.document.Text = this.Text;
                    this.IsDirty = true;
                    break;

                case "IsDirty":
                    this.AutoIndentCommand.Refresh();
                    break;
            }
        }

        protected void RefreshProperties()
        {
            this.FullPath = document.FullName;
            this.IsPersistent = document.IsPersistent;
            this.Name = document.Name;
            this.Path = document.Path;
            this.Text = document.Text;
            this.IsDirty = document.IsDirty;
        }


        public abstract void Open(string fileName);


        public virtual void Save()
        {
            this.Save(this.FullPath);
        }

        public abstract void Save(string fileName);
    }
}
