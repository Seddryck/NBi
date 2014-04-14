using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command
{
	class ReplaceAllCommand : CommandBase
	{
		private readonly FindAndReplacePresenter presenter;
        private readonly NbiTextEditor editor;

        public ReplaceAllCommand(FindAndReplacePresenter presenter, NbiTextEditor editor)
		{
			this.presenter = presenter;
			this.editor = editor;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = !string.IsNullOrEmpty(this.presenter.TextToFind);
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			if (string.IsNullOrEmpty(this.presenter.TextToFind)) return;

			this.editor.ReplaceAll(this.presenter.TextToFind, this.presenter.TextToReplace, this.presenter.CaseSensitive);
		}
	}
}