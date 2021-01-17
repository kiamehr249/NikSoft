using System;

namespace NikSoft.ContentManager.Service
{
    public class ContactUs
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int? UserID { get; set; }
        public int PortalID { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
