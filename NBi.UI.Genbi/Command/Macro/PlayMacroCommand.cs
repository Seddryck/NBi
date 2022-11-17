using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NBi.GenbiL;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using static System.Net.Mime.MediaTypeNames;

namespace NBi.UI.Genbi.Command.Macro
{
    class PlayMacroCommand : CommandBase
    {
        private readonly MacroWindow window;
        public PlayMacroCommand(MacroWindow window)
        {
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
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|genbiL (Genbi Language) (*.genbiL)|*.genbil";
            openFileDialog.FilterIndex = 2;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Execute(openFileDialog.FileName, false);
            }
        }

        public void Execute(string filename, bool quiet)
        {
            if (quiet)
                ExecuteQuiet(filename);
            else
                ExecuteVisual(filename);
        }

        public void ExecuteVisual(string filename)
        {
            var generator = new TestSuiteGenerator();
            generator.Load(filename);
            try
            {
                window.Show();
                generator.ActionInfoEvent += ActionInfo;
                generator.Execute();
                generator.ActionInfoEvent -= ActionInfo;
            }
            catch (Exception ex)
            {
                ShowException(String.Format("Exception generated during execution of the macro.\r\n\r\n{0}", ex.Message));
                return;
            }

            ShowInform(String.Format("Macro has been executed succesfully."));
        }

        public void ExecuteQuiet(string filename)
        {
            try
            {
                var generator = new TestSuiteGenerator();
                generator.Load(filename);
                generator.Execute();
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", String.Format("Exception generated during execution of the macro.\r\n\r\n{0}", ex.Message));
                File.AppendAllText("error.txt", String.Format("\nInner Exception.\r\n\r\n{0}", ex.InnerException));
                return;
            }
        }

        protected virtual void ActionInfo(object sender, TestSuiteGenerator.ActionInfoEventArgs e)
        {
            window.AppendText(e.Message);
        }
    }
}
