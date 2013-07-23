using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor;

namespace NBi.UI.Genbi.Command
{
	class ReplaceAllCommand : CommandBase
	{
		private readonly FindAndReplacePresenter presenter;
		private readonly XmlTextEditor editor;

		public ReplaceAllCommand(FindAndReplacePresenter presenter, XmlTextEditor editor)
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