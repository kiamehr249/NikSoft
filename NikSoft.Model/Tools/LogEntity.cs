using System;

namespace NikSoft.Model
{
    public class LogEntity
    {
        public Nullable<int> CreatedBy { set; get; }
        public Nullable<int> ModifiedBy { set; get; }
        public Nullable<DateTime> CreateDateTime { set; get; }
        public Nullable<DateTime> LastModifiedDateTime { set; get; }
    }
}