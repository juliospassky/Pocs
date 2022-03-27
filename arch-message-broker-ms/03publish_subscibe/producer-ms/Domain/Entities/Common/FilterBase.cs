namespace Domain.Entities.Common
{
    public class FilterBase
    {
        public int SkipSize { get; set; } = 10;
        public int LimitSize { get; set; } = 10;
    }
}
