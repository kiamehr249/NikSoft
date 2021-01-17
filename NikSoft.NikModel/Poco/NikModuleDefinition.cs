using NikSoft.Model;
using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class NikModuleDefinition : LogEntity
    {

        public NikModuleDefinition()
        {
            this.NikModules = new HashSet<NikModule>();
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

        public virtual ICollection<NikModule> NikModules { get; set; }
    }
}