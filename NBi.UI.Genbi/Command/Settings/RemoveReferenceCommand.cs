using System;
using System.Linq;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.Settings
{
    class RemoveReferenceCommand: CommandBase
	{
		private readonly SettingsPresenter presenter;

        public RemoveReferenceCommand(SettingsPresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.IsReferenceSelected();
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			presenter.RemoveReference(presenter.SettingsNameSelected);
		}
	}
}
