using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/12/2018 11:44:36 AM
/// </summary>



public class TxResource
{
    private static TxResource instance;
    public static TxResource Instance { get { return instance ?? (instance = new TxResource()); } }

    private Dictionary<string, object> _cachedAssets = new Dictionary<string, object>();

    public GameObject LoadGameObject(string path)
    {
        object obj;
        if(!_cachedAssets.TryGetValue(path,out obj))
        {
            obj = Resources.Load<GameObject>(path);
            if(obj==null)
            {
                txDebug.LogError("load failed : " + path);
                return null;
            }
            else
            {
                _cachedAssets.Add(path, obj);
            }
        }

        GameObject go = Object.Instantiate<GameObject>(obj as GameObject);
        go.name = path;
        return go;
    }
}

