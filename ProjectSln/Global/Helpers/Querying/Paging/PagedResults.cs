namespace Main.Global.Helpers.Querying.Paging
{
    public sealed class PagedResults<T> : List<T>
    {
        public PageData MetaData { get; set; }

        public PagedResults(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new PageData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }

        public static PagedResults<T> Page(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var size = pageSize < 50 & pageSize > 0 ? pageSize : count;
            var items = source
                .Skip((pageNumber - 1) * size)
                .Take(pageSize).ToList();

            return new PagedResults<T>(items, count, pageNumber, size);
        }

        public PagedResults<T> PrePaged<T>(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var size = pageSize < 50 & pageSize > 0 ? pageSize : count;

            return new PagedResults<T>(source, count, pageNumber > 1 ? pageNumber : 1, size);
        }
    }
}