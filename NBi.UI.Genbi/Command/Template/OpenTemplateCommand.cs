using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.Template
{
	class OpenTemplateCommand: CommandBase
	{
		private readonly TemplatePresenter presenter;
		private readonly OpenTemplateWindow window;


		public OpenTemplateCommand(TemplatePresenter presenter, OpenTemplateWindow window)
		{
			this.presenter = presenter;
			this.window = window;
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
			DialogResult result = window.ShowDialog();
			if (result == DialogResult.OK)
			{
				switch (window.Type)
				{
					case OpenTemplateWindow.TemplateType.Embedded:
						presenter.LoadEmbeddedTemplate(window.EmbeddedName);
						break;
					case OpenTemplateWindow.TemplateType.External:
						presenter.LoadExternalTemplate(window.FullPath);
						break;
					default:
						throw new ArgumentException();
				}
			}
			
		}
	}
}
