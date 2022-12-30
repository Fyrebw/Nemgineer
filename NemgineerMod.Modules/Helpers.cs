using System;
using System.Collections.Generic;

namespace NemgineerMod.Modules
{
    internal static class Helpers
    {
        public static T[] Append<T>(ref T[] array, List<T> list)
        {
            int length = array.Length;
            int count = list.Count;
            Array.Resize<T>(ref array, length + count);
            list.CopyTo(array, length);
            return array;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (Func<T[], T[]>)(r => Helpers.Append<T>(ref r, list));
    }
}
