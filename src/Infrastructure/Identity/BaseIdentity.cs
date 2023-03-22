using System;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class BaseIdentity : IdentityUser<Guid>
    {
        public bool IsDeleted { get; set; }
        public virtual DateTimeOffset Created { get; protected set; }
        public virtual DateTimeOffset? Modified { get; protected set; }
        public virtual string? LastModifyBy { get; protected set; }
        protected BaseIdentity()
        {
            IsDeleted = false;
            Created = DateTimeOffset.UtcNow;
        }
    }
}

