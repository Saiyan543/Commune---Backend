using Newtonsoft.Json;

namespace Main.Global.Helpers
{
    public static class FunctionalExtensions
    {
        public static T Either<T>(this T source, Func<T, bool> predicate, Func<T, T> operationA, Func<T, T> operationB)
             => predicate(source) ? operationA(source) : operationB(source);

        public static IEnumerable<T> Either<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> predicate, Func<IEnumerable<T>, IEnumerable<T>> operationA, Func<IEnumerable<T>, IEnumerable<T>> operationB)
             => predicate(source) ? operationA(source) : operationB(source);

        public static T? Deserialize<T>(this string source) =>
            JsonConvert.DeserializeObject<T>(source);
        public static string? Serialize<T>(this T source) =>
            JsonConvert.SerializeObject(source);

        public static IEnumerable<T?> Deserialize<T>(this string[] source) =>
            source.Select(x => JsonConvert.DeserializeObject<T>(x));

        public static T Next<T>(this T? source, Func<T?, T> operation)
            => operation(source);

        public static IEnumerable<T>? Next<T>(this IEnumerable<T>? source, Func<IEnumerable<T>?, IEnumerable<T>> operation)
             => operation(source);
        public static void Set<T>(this T? source, Action<T> operation)
            => operation(source);


        public static TOut? Transfer<TIn, TOut>(this TIn? source, Func<TIn?, TOut> mapping) =>
            mapping(source);

        public static IEnumerable<TOut>? Transfer<TIn, TOut>(this IEnumerable<TIn>? source, Func<TIn?, TOut> mapping) =>
            source.Select(x => mapping(x));

        public static IEnumerable<T> ResultOrEmpty<T>(this IEnumerable<T>? source)
            => source is not null ? source : Enumerable.Empty<T>();





        public static IEnumerable<T>? Foreach<T>(this IEnumerable<T> source, Func<T, T> func)
        {
            foreach (var item in source)
            {
                yield return func(item);
            }
        }

        public static async Task<IEnumerable<T>>? Foreach<T>(this IEnumerable<T> source, Func<T, Task<T>> func)
        {
            List<T> s = new();
            foreach (var item in source)
            {
                 s.Add(await func(item));
            }
            return s;
        }
    }
}