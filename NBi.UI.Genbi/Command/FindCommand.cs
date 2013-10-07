using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor;

namespace NBi.UI.Genbi.Command
{
	class FindCommand : CommandBase
	{
		private readonly FindAndReplacePresenter presenter;
		private readonly XmlTextEditor editor;

		public FindCommand(FindAndReplacePresenter presenter, XmlTextEditor editor)
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
			this.editor.Find(this.presenter.TextToFind, this.presenter.CaseSensitive);
		}
	}
}
