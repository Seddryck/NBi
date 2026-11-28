namespace NBi.Core.Decoration.Process.Conditions;

class RunningCondition : IDecorationCondition
{
    private readonly IRunningConditionArgs args;
    public RunningCondition(IRunningConditionArgs args) => this.args = args;

    public string Message { get => $"Check that the service named '{args.ServiceName.Execute()}' is running."; }

    public bool Validate()
        => Validate(args.ServiceName.Execute() ?? throw new NullReferenceException(), args.TimeOut.Execute());

    internal bool Validate(string serviceName, int timeOut)
        => ServiceManagement.Instance.IsRunning(serviceName, timeOut);
}
