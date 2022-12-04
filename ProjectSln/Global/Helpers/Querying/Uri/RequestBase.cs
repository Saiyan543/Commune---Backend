namespace Main.Global.Helpers.Querying.Uri
{
    public abstract record RequestBase
    {
        public int PageNumber { get; set; } = 1;

        private const int maxPageSize = 50;
        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
    }
}