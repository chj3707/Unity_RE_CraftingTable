using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    public static void Swap<T>(ref T p_a, ref T p_b)
    {
        var temp = p_a;
        p_a = p_b;
        p_b = temp;
    }
}
