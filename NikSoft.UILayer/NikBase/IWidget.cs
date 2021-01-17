namespace NikSoft.UILayer
{
    public interface IWidget
    {
        void Init(IWidgetHost host);

        void ShowSettings();

        void HideSettings();

        void Minimized();

        void Maximized();

        void Closed();
    }
}