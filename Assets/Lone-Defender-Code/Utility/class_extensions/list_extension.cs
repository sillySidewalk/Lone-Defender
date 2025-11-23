using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class My_list_extensions
{
    public static string List_to_string<T>(this List<T> l) =>
        string.Join(", ", l);

    // because Queues aren't visible in inspector, and because this shouldn't impact too much
    public static T Dequeue<T>(this List<T> l)
    {
        T return_val = l[0];
        l.RemoveAt(0);
        return return_val;
    }
}
