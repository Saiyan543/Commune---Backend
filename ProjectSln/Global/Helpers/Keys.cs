using System.Text;

namespace Main.Global.Helpers
{
    public static class Keys
    {
        public static string OrdinalKey(params string[] keys)
        {
            StringBuilder sb = new();
            Array.Sort(keys, StringComparer.Ordinal);
            foreach (var key in keys)
                sb.Append(key);
            return sb.ToString();
        }
    }
}