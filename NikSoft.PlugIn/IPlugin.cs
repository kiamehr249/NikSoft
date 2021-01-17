namespace NikSoft.PlugIn
{
    public interface IPlugin
    {
        void Initialize();
    }

    public interface IMetadata
    {
        string PluginName { get; }
        string PlugInVersion { get; }
    }
}