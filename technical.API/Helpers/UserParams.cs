namespace technical.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public int UserId { get; set; }

        public string Gender { get; set; }
        
        public int OlderThanDays { get; set; } = 0;
    }
}