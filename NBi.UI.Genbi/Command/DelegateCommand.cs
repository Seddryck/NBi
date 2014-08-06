namespace NBi.UI.Genbi.Command
{
	/// <summary>
	/// A command that delegates its implementation somewhere else.
	/// </summary>
	class DelegateCommand : CommandBase
	{
		private readonly CanExecute canDoAction;
		private readonly Execute doAction;
		private readonly string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand"/> class.
		/// </summary>
		/// <param name="canDoAction">The delegate that defines if the command is enabled.</param>
		/// <param name="doAction">The delegate that defines the command action.</param>
		public DelegateCommand(CanExecute canDoAction, Execute doAction)
			: this(null, canDoAction, doAction)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="canDoAction">The can do action.</param>
		/// <param name="doAction">The do action.</param>
		public DelegateCommand(string name, CanExecute canDoAction, Execute doAction)
		{
			this.name = name;
			this.canDoAction = canDoAction;
			this.doAction = doAction;
		}

		/// <summary>
		/// Gets the name of the command.
		/// </summary>
		public override string Name
		{
			get { return this.name ?? base.Name; }
		}

		public override void Invoke()
		{
			this.Refresh();
			if (!this.IsEnabled) return;
			this.doAction();
		}

		public override void Refresh()
		{
			this.IsEnabled = this.canDoAction();
		}
	}
}