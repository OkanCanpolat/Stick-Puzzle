using System;
using System.Collections.Generic;

public static class ListExtension
{
    public static void AddIfNotContains<T>(this List<T> list, T insert)
    {
        if (!list.Contains(insert))
        {
            list.Add(insert);
        }
    }

    public static void MergeUniques<T>(this List<T> source, List<T> target)
    {
        foreach (var item in target)
        {
            source.AddIfNotContains(item);
        }
    }
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rnd = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            list.Swap(i, rnd.Next(i + 1));
        }
    }
    public static void Shuffle<T>(this IList<T> list, Random rnd)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            list.Swap(i, rnd.Next(i + 1));
        }
    }
    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}
