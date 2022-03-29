namespace Domain.Common
{
    public class Audit
    {
        public DateTime Created { get; private set; }

        public string? CreatedBy { get; private set; }

        public DateTime? LastModified { get; private set; }

        public string? LastModifiedBy { get; private set; }

        public Audit(string user)
        {
            CreatedBy = user;
            LastModifiedBy = user;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public void UpdateLastModified(string lastModifiedBy)
        {
            LastModifiedBy = lastModifiedBy.Trim();
            LastModified = DateTime.Now;
        }
    }
}
