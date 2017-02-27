
namespace YmtSystem.Infrastructure.Container.Unity.Bootstrapper
{
    public enum BootstrapperStatus
    {
        NotStarted = 0,
        Starting = 1,
        Started = 2,
        FailedToStart = 3,
        Ending = 4,
        Ended = 5,
    }
}
