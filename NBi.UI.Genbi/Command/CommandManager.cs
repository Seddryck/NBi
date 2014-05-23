    namespace NBi.UI.Genbi.Command
{
	class CommandManager
	{
		public static readonly CommandManager Instance = new CommandManager();

		private CommandManager()
		{
			this.Bindings = new CommandBindings();
		}

		public CommandBindings Bindings { get; private set; }
	}
}
