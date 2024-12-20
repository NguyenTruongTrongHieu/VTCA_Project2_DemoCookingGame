namespace Studio.OverOne.Addr.Extensions
{
    internal static class ArrayExtensions
    {
        public static T[] Append<T>(this T[] array, params T[] items)
        {
            if (array == null) {
                return items;
            }
            T[] result = new T[array.Length + items.Length];
            array.CopyTo(result, 0);
            items.CopyTo(result, array.Length);
            return result;
        }
    }
}