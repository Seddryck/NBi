using NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor;

namespace NBi.UI.Genbi.Command
{
	 class CancelFindCommand : CommandBase
	{
		private readonly XmlTextEditor editor;

		public CancelFindCommand(XmlTextEditor editor)
		{
			this.editor = editor;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = true;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			this.editor.ResetLastFound();
		}
	}
}