using System.Web.UI.WebControls;

namespace NikSoft.UILayer
{
    public interface IEngineContainer
    {
        event NewItemEventHandler NewItem;
        event GotoListEventHandler GotoList;

        string EditItemKey { get; set; }
        string AddNewKey { get; set; }

        void gotoList();

        void gotonewItem();

        void gotoEditURI(string iID);

        string getEditURI(string iID);

        bool hasCUPermissionCurrentUser();

        bool hasRDPermissionCurrentUser();

        LinkButton btnDelete { get; }
        string ModuleHeader { get; set; }
        void HideSearch();
        void ShowSearch();
    }

    public delegate void NewItemEventHandler();

    public delegate void GotoListEventHandler();
}
