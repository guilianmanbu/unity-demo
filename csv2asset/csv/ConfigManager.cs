using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ConfigManager
{

    public const string tableAssetsFolder = "tables/";

    public static Dictionary<Type, TableBase> tableDic = new Dictionary<Type, TableBase>();

    public static void LoadAllTable()
    {
        if (tableDic.Count > 0)
            return;

        #region auto
#endregion auto

        // Build All
        foreach (var item in tableDic)
            item.Value.Build();
    }

    private static void LoadTable<Ttable, Tdata>(string fileName)
        where Ttable : TableBase
        where Tdata : CsvBase
    {
        Ttable t = Resources.Load<Ttable>(tableAssetsFolder + fileName);

        tableDic.Add(typeof(Tdata), t);
    }

    public static Ttable GetTable<Ttable,Tdata>() 
        where Ttable : TableGeneric<Tdata>
        where Tdata:CsvBase
    {
        return tableDic[typeof(Tdata)] as Ttable;
    }

    public static D[] GetArray<D>() where D : CsvBase
    {
        return (tableDic[typeof(D)] as TableGeneric<D>).list;
    }

    public static Dictionary<int, D> GetDic<D>() where D : CsvBase
    {
        return (tableDic[typeof(D)] as TableGeneric<D>).dic;
    }

    public static D GetData<D>(int id) where D : CsvBase
    {
        return GetDic<D>()[id];
    }

    public static bool IsExistsOfID<D>(int id) where D : CsvBase
    {
        return GetDic<D>().ContainsKey(id);
    }

    public static void Dispose()
    {
        tableDic = new Dictionary<Type, TableBase>();
        Resources.UnloadUnusedAssets();
    }
}