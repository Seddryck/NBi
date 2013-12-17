using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor;

namespace NBi.UI.Genbi.Command
{
	class FindAndReplaceCommand : CommandBase
	{
		private readonly XmlTextEditor editor;

		public FindAndReplaceCommand(XmlTextEditor editor)
		{
			this.editor = editor;
		}

		public override string Name
		{
			get { return "FindAndReplace"; }
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			var textToFind = this.editor.SelectedText;
			var presenter = new FindAndReplacePresenter(this.editor) { TextToFind = textToFind };
			var window = new FindAndReplaceWindow(presenter);
			window.Show(editor);
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = this.editor.Presenter != null && !string.IsNullOrEmpty(this.editor.Text);
		}
	}
}