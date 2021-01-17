using NikSoft.Model;
using NikSoft.NikModel;

namespace NikSoft.Services
{
    public interface IWidgetDefinitionService : INikService<WidgetDefinition>
    {
    }
    public class WidgetDefinitionService : NikService<WidgetDefinition>, IWidgetDefinitionService
    {

        public WidgetDefinitionService(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}