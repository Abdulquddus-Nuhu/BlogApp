using System;
namespace Infrastructure.Identity
{
    public class Persona : BaseIdentity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => FirstName + " " + LastName;
    }
}

