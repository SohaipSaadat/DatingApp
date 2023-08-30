namespace DatingApplication.Helper
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _PageSize { get; set; } = 10;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
