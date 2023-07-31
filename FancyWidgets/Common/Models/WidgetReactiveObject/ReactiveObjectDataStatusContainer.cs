namespace FancyWidgets.Common.Models.WidgetReactiveObject;

internal static class ReactiveObjectDataStatusContainer
{
    private static readonly HashSet<ReactiveObjectDataStatus?> CurrentObjectDataStatuses = new();

    private static readonly object LockObject = new();

    public static IEnumerable<ReactiveObjectDataStatus?> GetDataStatus()
    {
        lock (LockObject)
        {
            return CurrentObjectDataStatuses.AsEnumerable();
        }
    }

    public static void Add(ReactiveObjectDataStatus status)
    {
        lock (LockObject)
        {
            CurrentObjectDataStatuses.Add(status);
        }
    }

    public static void Remove(ReactiveObjectDataStatus status)
    {
        lock (LockObject)
        {
            CurrentObjectDataStatuses.Remove(status);
        }
    }
}