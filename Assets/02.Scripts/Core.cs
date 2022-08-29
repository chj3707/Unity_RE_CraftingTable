using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Core
{
    public static void swap<T>(ref T lvalue, ref T rvalue)
    {
        var temp = lvalue;
        lvalue = rvalue;
        rvalue = temp;
    }
}
