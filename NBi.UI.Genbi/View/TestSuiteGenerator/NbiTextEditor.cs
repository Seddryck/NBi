using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    class NbiTextEditor : TextEditorControl
    {
        private DocumentPresenterBase presenter;
        public DocumentPresenterBase Presenter
        {
            get { return this.presenter; }
            set
            {
                if (value == this.presenter) return;
                this.presenter = value;
                this.RefreshCommands(this, EventArgs.Empty);
            }
        }

        #region Extended properties

        public string SelectedText
        {
            get
            {
                return base.ActiveTextAreaControl.SelectionManager.SelectedText;
            }
        }

        public string[] Lines
        {
            get
            {
                return base.Text.Split(new[] { "\r\n" }, StringSplitOptions.None);
            }
        }

        #endregion

        private int previousSearchLine = -1;
        private int previousSearchWord;

        // Methods
        public NbiTextEditor()
        {
            base.Document.DocumentChanged += this.Document_DocumentChanged;

            this.UndoCommand = new DelegateCommand(CanUndo, Undo);
            this.RedoCommand = new DelegateCommand(CanRedo, Redo);

            this.CutCommand = new DelegateCommand(CanCut, DoCut);
            this.CopyCommand = new DelegateCommand(CanCopy, DoCopy);
            this.PasteCommand = new DelegateCommand(CanPaste, DoPaste);

            this.SelectAllCommand = new DelegateCommand(CanSelectAll, DoSelectAll);
            this.FindAndReplaceCommand = new FindAndReplaceCommand(this);
            this.ToggleFoldingsCommand = new DelegateCommand(() => true, this.DoToggleFoldings);

            this.CreateContextMenu();

            Application.Idle += RefreshCommands;

            //base.Document.FoldingManager.UpdateFoldings(string.Empty, null);
        }

        private void RefreshCommands(object sender, EventArgs e)
        {
            this.UndoCommand.Refresh();
            this.RedoCommand.Refresh();

            this.CutCommand.Refresh();
            this.CopyCommand.Refresh();
            this.PasteCommand.Refresh();

            this.SelectAllCommand.Refresh();
            this.FindAndReplaceCommand.Refresh();
            this.ToggleFoldingsCommand.Refresh();
        }

        

        #region Commands definitions

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand CutCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }

        public ICommand SelectAllCommand { get; private set; }
        public ICommand ToggleFoldingsCommand { get; private set; }
        public ICommand FindAndReplaceCommand { get; private set; }

        #endregion

        #region Commands implementations

        private bool CanUndo()
        {
            return this.Presenter != null && base.Document.UndoStack.CanUndo;
        }

        private bool CanRedo()
        {
            return this.Presenter != null && base.Document.UndoStack.CanRedo;
        }

        private bool CanCopy()
        {
            return this.Presenter != null && base.ActiveTextAreaControl.SelectionManager.HasSomethingSelected;
        }

        private bool CanCut()
        {
            return this.Presenter != null && base.ActiveTextAreaControl.SelectionManager.HasSomethingSelected;
        }

        private bool CanPaste()
        {
            return this.Presenter != null && base.ActiveTextAreaControl.TextArea.ClipboardHandler.EnablePaste;
        }

        private bool CanSelectAll()
        {
            if (this.Presenter == null) return false;
            if (base.Document.TextContent == null) return false;
            return !base.Document.TextContent.Trim().Equals(String.Empty);
        }




        private void DoCut()
        {
            new Cut().Execute(base.ActiveTextAreaControl.TextArea);
            base.ActiveTextAreaControl.Focus();
        }

        private void DoCopy()
        {
            new Copy().Execute(base.ActiveTextAreaControl.TextArea);
            base.ActiveTextAreaControl.Focus();
        }

        private void DoPaste()
        {
            new Paste().Execute(base.ActiveTextAreaControl.TextArea);
            base.ActiveTextAreaControl.Focus();
        }

        private void DoSelectAll()
        {
            new SelectWholeDocument().Execute(base.ActiveTextAreaControl.TextArea);
            base.ActiveTextAreaControl.Focus();
        }

        public void DoToggleFoldings()
        {
            new ToggleAllFoldings().Execute(base.ActiveTextAreaControl.TextArea);
        }

        #endregion

        # region Initialization

        private void CreateContextMenu()
        {
            //contextmenu
            var mnu = new ContextMenuStrip();
            var mnuFind = new ToolStripMenuItem("Find/Replace");
            var mnuFold = new ToolStripMenuItem("Open/close all foldings");

            CommandManager.Instance.Bindings.Add(this.FindAndReplaceCommand, mnuFind);
            CommandManager.Instance.Bindings.Add(this.ToggleFoldingsCommand, mnuFold);


            //Add to main context menu
            mnu.Items.AddRange(new ToolStripItem[] { mnuFind, mnuFold });

            //Assign to datagridview
            base.ActiveTextAreaControl.ContextMenuStrip = mnu;
        }

        #endregion

        public void SelectText(int start, int length)
        {
            var textLength = base.Document.TextLength;
            if (textLength < (start + length))
            {
                length = (textLength - 1) - start;
            }
            base.ActiveTextAreaControl.Caret.Position = base.Document.OffsetToPosition(start + length);
            base.ActiveTextAreaControl.SelectionManager.ClearSelection();
            base.ActiveTextAreaControl.SelectionManager.SetSelection(new DefaultSelection(base.Document, base.Document.OffsetToPosition(start), base.Document.OffsetToPosition(start + length)));
            base.Refresh();
        }

        public void Find(string search)
        {
            this.Find(search, false);
        }

        public void Find(string search, bool caseSensitive)
        {
            var found = false;

            var i = 0;
            var lines = this.Lines;
            foreach (var line in lines)
            {
                if (i > previousSearchLine)
                {
                    int start;
                    if (previousSearchWord > line.Length)
                    {
                        start = caseSensitive ?
                            line.IndexOf(search) :
                            line.ToLower().IndexOf(search.ToLower());

                        previousSearchWord = 0;
                    }
                    else
                    {
                        start = caseSensitive ?
                            line.IndexOf(search, previousSearchWord) :
                            line.ToLower().IndexOf(search.ToLower(), previousSearchWord);
                    }
                    var end = start + search.Length;
                    if (start != -1)
                    {
                        var p1 = new Point(start, i);
                        var p2 = new Point(end, i);

                        //TODO base.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
                        base.ActiveTextAreaControl.ScrollTo(i);
                        base.Refresh();

                        previousSearchWord = end;
                        previousSearchLine = i - 1;
                        found = true;
                        break;
                    }

                    previousSearchWord = 0;
                }

                i += 1;
                if (i >= lines.Length - 1)
                {
                    previousSearchLine = -1;
                }
            }

            if (!found)
            {
                MessageBox.Show("The following specified text was not found: " + Environment.NewLine + Environment.NewLine + search);
            }
        }

        public void Replace(string search, string replace, bool caseSensitive)
        {
            if (base.ActiveTextAreaControl.SelectionManager.HasSomethingSelected && base.ActiveTextAreaControl.SelectionManager.SelectedText == search)
            {
                var text = base.ActiveTextAreaControl.SelectionManager.SelectedText;
                base.ActiveTextAreaControl.Caret.Position = base.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].StartPosition;
                base.ActiveTextAreaControl.SelectionManager.ClearSelection();
                base.ActiveTextAreaControl.Document.Replace(base.ActiveTextAreaControl.Caret.Offset, text.Length, replace);
            }
            this.Find(search, caseSensitive);
        }

        public void ReplaceAll(string search, string replace)
        {
            this.ReplaceAll(search, replace, false);
        }

        public void ReplaceAll(string search, string replace, bool caseSensitive)
        {
            base.Text = Regex.Replace(base.Text, search, replace, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
            base.Refresh();
        }

        public void ResetLastFound()
        {
            previousSearchLine = -1;
            previousSearchWord = 0;
        }

        private void Document_DocumentChanged(object sender, DocumentEventArgs e)
        {
            //base.Document.FoldingManager.UpdateFoldings(string.Empty, null);
            bool isVisible = (base.Document.TotalNumberOfLines > this.ActiveTextAreaControl.TextArea.TextView.VisibleLineCount);
            base.ActiveTextAreaControl.VScrollBar.Visible = isVisible;               

            if (this.Presenter == null) return;
            if (this.Text == Environment.NewLine && string.IsNullOrEmpty(this.presenter.Text)) return;

            this.Presenter.Text = this.Text;
        }
    }
}
