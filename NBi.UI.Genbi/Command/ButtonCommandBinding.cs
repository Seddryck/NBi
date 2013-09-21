using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NBi.UI.Genbi.Command
{
	class ButtonCommandBinding : ICommandBinding
	{
		private readonly ICommand command;
		private readonly Button item;

		public ButtonCommandBinding(ICommand command, Button item)
		{
			this.command = command;
			this.item = item;
			this.Bind();
			this.command.Refresh();
		}

		private void Bind()
		{
			this.command.PropertyChanged += OnCommandEnabledChanged;
			this.item.Click += OnItemClick;
		}

		public ICommand Command
		{
			get { return this.command; }
		}

		public object Trigger
		{
			get { return this.item; }
		}

		public void Unbind()
		{
			this.command.PropertyChanged -= OnCommandEnabledChanged;
			this.item.Click -= OnItemClick;
		}

		private void OnCommandEnabledChanged(object sender, PropertyChangedEventArgs e)
		{
			this.item.Enabled = this.command.IsEnabled;
		}

		private void OnItemClick(object sender, EventArgs e)
		{
			this.command.Invoke();
		}
	}
}