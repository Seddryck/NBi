using System;
using System.Linq;
using System.Windows.Forms;
using NBi.GenbiL;

namespace NBi.UI.Genbi.Command.Macro
{
	class PlayMacroCommand: CommandBase
	{
		public PlayMacroCommand()
		{
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
			var openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "All Files (*.*)|*.*|genbiL (Genbi Language) (*.genbiL)|*.genbil";
			openFileDialog.FilterIndex = 2;
			DialogResult result = openFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				var generator = new TestSuiteGenerator();
				generator.Load(openFileDialog.FileName);
				try 
				{	        
					generator.Execute();
				}
				catch (Exception ex)
				{
					ShowException(String.Format("Exception generated during execution of the macro.\r\n\r\n{0}", ex.Message));
					return;
				}

				ShowInform(String.Format("Macro has been executed succesfully."));
			}
			
		}
	}
}
