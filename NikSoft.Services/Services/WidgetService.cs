using NikSoft.Model;
using NikSoft.NikModel;

namespace NikSoft.Services
{
    public interface IWidgetService : INikService<Widget>
    {

    }
    public class WidgetService : NikService<Widget>, IWidgetService
    {

        public WidgetService(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}