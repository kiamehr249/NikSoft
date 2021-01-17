namespace NikSoft.UILayer
{
    public interface INikContentManage
    {
        void InitHost(IEngineContainer host);

        IEngineContainer Container { get; }

        INotification Notification { get; }

        event Error404Event Error404;
        event ContentNotFoundEvent ContentNotFound;
    }

    public delegate void Error404Event();
    public delegate void ContentNotFoundEvent();
}