namespace NBi.Core.Decoration.Process;

internal interface IServiceManagement
{
    void Start(string serviceName, int timeout);
    void Stop(string serviceName, int timeout);
    bool IsRunning(string serviceName, int timeout);
}
