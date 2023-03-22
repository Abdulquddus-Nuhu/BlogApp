using System;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }

        public Role(string roleName)
        {
            Name = roleName;
        }
    }
}

