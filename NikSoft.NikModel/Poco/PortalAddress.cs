namespace NikSoft.NikModel
{
    public class PortalAddress
    {
        public int ID { get; set; }
        public int PortalID { get; set; }
        public string DomainAddress { get; set; }
        public string Desciption { get; set; }

        public virtual Portal Portal { get; set; }
    }
}
