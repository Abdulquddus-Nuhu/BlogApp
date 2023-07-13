using System;
namespace Core.Entities
{
    public class Image : BaseEntity
    {
        public Guid EntityId { get; set; }
        public string Data { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}

