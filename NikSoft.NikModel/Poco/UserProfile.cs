namespace NikSoft.NikModel
{
    public class UserProfile
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
