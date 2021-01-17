namespace NikSoft.UILayer
{
    public interface IWidgetHost
    {

        void SaveState(string state);

        string GetState();

        void Close();

        void SkinChanged();

        string SkinIDofWidget
        {
            get;
            set;
        }
    }
}