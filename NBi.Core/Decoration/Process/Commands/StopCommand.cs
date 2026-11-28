namespace NBi.Core.Decoration.Process.Commands;

class StopCommand : IDecorationCommand
{
    private readonly ServiceStopCommandArgs args;
    public StopCommand(ServiceStopCommandArgs args) => this.args = args;

    public void Execute() => Execute(args.ServiceName.Execute() ?? throw new ArgumentNullException(), args.TimeOut.Execute());

    internal void Execute(string serviceName, int timeOut)
        => ServiceManagement.Instance.Stop(serviceName, timeOut);
}
