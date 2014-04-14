using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command
{
	 class CancelFindCommand : CommandBase
	{
         private readonly NbiTextEditor editor;

        public CancelFindCommand(NbiTextEditor editor)
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