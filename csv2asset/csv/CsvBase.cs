using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/26/2018 4:40:33 PM
/// </summary>


public abstract class TableBase : ScriptableObject
{
    public abstract void Build();
}

public class TableGeneric<D> : TableBase where D : CsvBase
{
    public D[] list;
    public Dictionary<int, D> dic;

    public override void Build()
    {
        dic = new Dictionary<int, D>();
        for (int i = 0; i < list.Length; i++)
        {
            dic.Add(list[i].ID, list[i]);
        }
    }
}

[Serializable]
public class CsvBase
{
    public int ID;
}
