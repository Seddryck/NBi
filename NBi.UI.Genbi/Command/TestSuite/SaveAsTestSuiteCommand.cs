using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.TestSuite
{
	class SaveAsTestSuiteCommand : CommandBase
	{
		private readonly TestSuitePresenter presenter;

		public SaveAsTestSuiteCommand(TestSuitePresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.Tests.Count != 0;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			var saveAsDialog = new SaveFileDialog();
			saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Suite Files (*.nbits)|*.nbits|Xml Files (*.xml)|*.xml|Text Files (*.txt)|*.txt";
			saveAsDialog.FilterIndex = 2;
			DialogResult result = saveAsDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				//TODO presenter.State.Suite.Save(saveAsDialog.FileName);
				ShowInform(String.Format("Test-suite '{0}' persisted.", saveAsDialog.FileName));
			}
				
		}
	}
}
