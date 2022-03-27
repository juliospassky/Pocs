using Domain.Common;
using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Todo : EntityBase
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Audit? Audit { get; set; }
    }
}
