namespace NBi.UI.Genbi.Command
{
	interface ICommandBinding
	{
		ICommand Command { get; }
		object Trigger { get; }

		void Unbind();
	}
}