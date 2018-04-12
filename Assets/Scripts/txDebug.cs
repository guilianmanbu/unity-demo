using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 11:44:36 AM
/// </summary>


public class txDebug
{
    public static void Log(object obj)
    {
        Debug.Log(obj);
    }

    public static void LogWarnning(object obj)
    {
        Debug.LogWarning(obj);
    }

    public static void LogError(object obj)
    {
        Debug.LogError(obj);
    }
}

