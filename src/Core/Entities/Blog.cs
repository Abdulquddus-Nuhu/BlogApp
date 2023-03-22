using System;
namespace Core.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid PersonaId { get; set; }
    }
}

