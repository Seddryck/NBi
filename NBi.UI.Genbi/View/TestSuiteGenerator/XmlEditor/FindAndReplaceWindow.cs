using System;
using System.Windows.Forms;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator.XmlEditor
{
	partial class FindAndReplaceWindow : Form
	{
		public FindAndReplaceWindow(FindAndReplacePresenter presenter)
		{
			this.InitializeComponent();
			this.Presenter = presenter;
		}

		public FindAndReplacePresenter presenter;
		public FindAndReplacePresenter Presenter
		{
			get { return this.presenter; }
			set 
			{ 
				if (this.presenter == value) return;

				if (this.presenter != null)
				{
					this.UnbindPresenter();
				}

				this.presenter = value;
				this.UpdateView();

				if (this.presenter != null)
				{
					this.BindPresenter();
				}
			}
		}

		private void BindPresenter()
		{
			CommandManager.Instance.Bindings.Add(this.Presenter.FindCommand, btnFindNext);
			CommandManager.Instance.Bindings.Add(this.Presenter.ReplaceCommand, btnReplace);
			CommandManager.Instance.Bindings.Add(this.Presenter.ReplaceAllCommand, btnReplaceAll);
		}

		private void UnbindPresenter()
		{
			CommandManager.Instance.Bindings.Remove(this.Presenter.FindCommand, btnFindNext);
			CommandManager.Instance.Bindings.Remove(this.Presenter.ReplaceCommand, btnReplace);
			CommandManager.Instance.Bindings.Remove(this.Presenter.ReplaceAllCommand, btnReplaceAll);
		}

		private void UpdateView()
		{
			this.search.Text = this.Presenter.TextToFind;
			this.replace.Text = this.Presenter.TextToReplace;
			this.matchCaseOption.Checked = this.Presenter.CaseSensitive;
			this.matchWordOption.Checked = this.Presenter.MatchWord;
		}

		private void OnSearchKeyUp(object sender, KeyEventArgs e)
		{
			this.Presenter.TextToFind = this.search.Text;
		}

		private void OnReplaceKeyUp(object sender, KeyEventArgs e)
		{
			this.Presenter.TextToReplace = this.replace.Text;
		}

		private void OnOptionChanged(object sender, EventArgs e)
		{
			this.Presenter.CaseSensitive = this.matchCaseOption.Checked;
			this.Presenter.MatchWord = this.matchWordOption.Checked;
		}

		private void OnClosing(object sender, FormClosingEventArgs e)
		{
			this.Presenter.CancelFindCommand.Invoke();
		}

		private void OnFormKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void btnFindNext_Click(object sender, EventArgs e)
		{

		}
	}
}