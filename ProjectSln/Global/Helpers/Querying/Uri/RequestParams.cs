namespace Main.Global.Helpers.Querying.Uri
{
    public record class RequestParams : RequestBase
    {
        public RequestParams()
        {
            Booleans = Booleans;
            Comparisons = Comparisons;
        }

        public string Booleans { get; set; } = string.Empty;

        public ValueFilter[] Comparisons { get; set; } = Array.Empty<ValueFilter>();

        public string[] SplitBools() => Booleans != string.Empty ? Booleans.Split(" ") : Array.Empty<string>();

    }

    //?pagenumber=1&pagesize=10&Booleans%5B0%5D.Fields=a%20b%20c&Comparisons%5B0%5D.Field=name&Comparisons%5B0%5D.Value=aladdin&Comparisons%5B0%5D.Operator=eq&Comparisons%5B1%5D.Field=age&Comparisons%5B1%5D.Value=18&Comparison%5B1%5D.Operator=eq
}