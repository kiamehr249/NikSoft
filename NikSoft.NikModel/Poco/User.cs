using System;
using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OldUsername { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string LoginKey { get; set; }
        public int RandomID { get; set; }
        public bool IsLock { get; set; }
        public Nullable<DateTime> LastLogin { get; set; }
        public int PortalID { get; set; }
        public DateTime PassExpireDate { get; set; }
        public bool UnExpire { get; set; }
        public NikUserType UserType { get; set; }
        public int FailedPassword { get; set; }
        public Nullable<DateTime> FailedPasswordDateTime { get; set; }
        public string UserTitle { get; set; }
        public string ConfirmKey { get; set; }
        public bool ResetRequest { get; set; }
        public DateTime? ConfiramationExpireDate { get; set; }
        public DateTime? ConfirmationDateTime { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual ICollection<UserTypeGroup> UserTypeGroups { get; set; }
        public virtual ICollection<UserType> UserTypes { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
