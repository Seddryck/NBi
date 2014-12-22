using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.Template
{
	class SaveTemplateCommand : CommandBase
	{
		private readonly TemplatePresenter presenter;

		public SaveTemplateCommand(TemplatePresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = !string.IsNullOrEmpty(presenter.Template) && presenter.IsModified;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			var saveAsDialog = new SaveFileDialog();
			saveAsDialog.Filter = "All Files (*.*)|*.*|NBi Test Template Files (*.nbitt)|*.nbitt|Text Files (*.txt)|*.txt";
			saveAsDialog.FilterIndex = 2;
			DialogResult result = saveAsDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				presenter.Save(saveAsDialog.FileName);
				ShowInform(String.Format("Template '{0}' saved.", saveAsDialog.FileName));
			}
				
		}
	}
}
