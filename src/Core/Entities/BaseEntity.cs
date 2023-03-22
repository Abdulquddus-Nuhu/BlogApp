using System;

namespace Core.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public string LastModifyBy { get; set; } = string.Empty;

        protected BaseEntity()
        {
            IsDeleted = false;
            Created = DateTimeOffset.Now;
        }
    }
}

