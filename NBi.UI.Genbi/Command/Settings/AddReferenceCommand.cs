using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.Settings
{
	class AddReferenceCommand: CommandBase
	{
		private readonly SettingsPresenter presenter;
		private readonly NewReferenceWindow window;

		public AddReferenceCommand(SettingsPresenter presenter, NewReferenceWindow window)
		{
			this.presenter = presenter;
			this.window = window;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.IsValidReferenceName(window.ReferenceName);
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			DialogResult result = window.ShowDialog();
			if (result == DialogResult.OK)
				presenter.AddReference(window.ReferenceName);
		}
	}
}
