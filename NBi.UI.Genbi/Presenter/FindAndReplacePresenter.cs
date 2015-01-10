using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Presenter
{
    class FindAndReplacePresenter : PresenterBase
    {
        public FindAndReplacePresenter(NbiTextEditor editor)
        {
            this.FindCommand = new FindCommand(this, editor);
            this.CancelFindCommand = new CancelFindCommand(editor);
            this.ReplaceCommand = new ReplaceCommand(this, editor);
            this.ReplaceAllCommand = new ReplaceAllCommand(this, editor);

            this.TextToFind = string.Empty;
            this.TextToReplace = string.Empty;
            this.MatchWord = false;
            this.CaseSensitive = false;
        }

        #region Bindable properties

        public string TextToFind
        {
            get { return this.GetValue<string>("TextToFind"); }
            set { this.SetValue("TextToFind", value); }
        }

        public string TextToReplace
        {
            get { return this.GetValue<string>("TextToReplace"); }
            set { this.SetValue("TextToReplace", value); }
        }

        public bool CaseSensitive
        {
            get { return this.GetValue<bool>("CaseSensitive"); }
            set { this.SetValue("CaseSensitive", value); }
        }

        public bool MatchWord
        {
            get { return this.GetValue<bool>("MatchWord"); }
            set { this.SetValue("MatchWord", value); }
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "TextToFind":
                case "TextToReplace":
                    this.FindCommand.Refresh();
                    this.CancelFindCommand.Refresh();
                    this.ReplaceCommand.Refresh();
                    this.ReplaceAllCommand.Refresh();
                    break;
            }
        }

        public ICommand FindCommand { get; private set; }
        public ICommand CancelFindCommand { get; private set; }
        public ICommand ReplaceCommand { get; private set; }
        public ICommand ReplaceAllCommand { get; private set; }
    }
}
