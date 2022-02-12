using Domain.Common;

namespace Domain.Entities
{
    public class Todo : AuditableEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
