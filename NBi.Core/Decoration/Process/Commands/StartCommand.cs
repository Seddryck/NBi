namespace NBi.Core.Decoration.Process.Commands;

class StartCommand : IDecorationCommand
{
    private readonly ServiceStartCommandArgs args;
    public StartCommand(ServiceStartCommandArgs args) => this.args = args;

    public void Execute()
        => Execute(args.ServiceName.Execute() ?? throw new NullReferenceException(), args.TimeOut.Execute());

    internal void Execute(string serviceName, int timeOut)
        => ServiceManagement.Instance.Start(serviceName, timeOut);
}
