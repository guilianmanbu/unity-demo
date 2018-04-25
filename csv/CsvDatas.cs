using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


public class CsvDatas
{
    private static Dictionary<Type, IDictionary> dic = new Dictionary<Type, IDictionary>();
    public static Dictionary<Type, IDictionary> Dic { get { return dic; } }

    private static Dictionary<Type, IList> listDic = new Dictionary<Type, IList>();     // 内部缓存列表形式

    public static string csvFolder = @"E:\UnityWorkspace\Common\config-table";

    /// <summary>
    /// 获取配置字典，key=id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Dictionary<int,T> GetDic<T>()
    {
        IDictionary _dic;
        if (dic.TryGetValue(typeof(T), out _dic))
            return _dic as Dictionary<int, T>;
        else
            return new Dictionary<int, T>();
    }

    /// <summary>
    /// 获取配置单项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T GetData<T>(int id) where T : CsvBase
    {
        T t;
        if (GetDic<T>().TryGetValue(id,out t))
            return t;
        else
            return null;
    }

    /// <summary>
    /// 获取配置列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> GetList<T>()
    {
        IList _list;
        if (!listDic.TryGetValue(typeof(T), out _list))
        {
            _list = GetDic<T>().Values.ToList<T>();
            listDic.Add(typeof(T), _list);
        }
        return _list as List<T>;
    }

    /// <summary>
    /// 是否存在配置项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsExistsOfID<T>(int id) where T : CsvBase
    {
        return GetDic<T>().ContainsKey(id);
    }

    /// <summary>
    /// 清空
    /// </summary>
    public static void Dispose()
    {
        dic = new Dictionary<Type, IDictionary>();
        listDic = new Dictionary<Type, IList>();
    }

    /// <summary>
    /// 加载所有csv文件到内存为obj实例
    /// </summary>
    public static void LoadAllCsv()
    {
        if (dic.Count > 0) return;

        #region auto
		LoadCsv<Achievement>("achievement.csv");
		LoadCsv<AlbumList>("album_list.csv");
#endregion

        BuildAllCsv();
    }

    /// <summary>
    /// 处理关联引用
    /// </summary>
    private static void BuildAllCsv()
    {
        foreach (KeyValuePair<Type,IDictionary> innerDicEntry in dic)
        {
            foreach (DictionaryEntry itemEntry in innerDicEntry.Value) 
            {
                (itemEntry.Value as CsvBase).Build();
            }
        }
    }

    /// <summary>
    /// 加载csv为obj实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="csvFileName"></param>
    private static void LoadCsv<T>(string csvFileName) where T : CsvBase
    {
        Debuger.Log("Parse file... : " + csvFileName);

        Type _type = typeof(T);

        // 创建字典
        IDictionary innerDic = null;
        if(!dic.TryGetValue(_type,out innerDic))
        {
            innerDic = new Dictionary<int, T>();
            dic.Add(_type,innerDic);
        }
        else
        {
            Debuger.Log("csv repeate ! type = " + _type.Name + " csv file = " + csvFileName);
            return;
        }

        FileInfo csvFile = new FileInfo(csvFolder + "\\" + csvFileName);

        // 读取
        List<string> csvList = new List<string>();
        using(StreamReader sr = new StreamReader(csvFile.Open(FileMode.Open,FileAccess.Read,FileShare.ReadWrite)))
        {
            while (sr.Peek() > 0)
                csvList.Add(sr.ReadLine());
        }

        // 匹配字段顺序
        string[] keyList = csvList[2].Split(',');
        FieldInfo[] fieldInfos = new FieldInfo[keyList.Length];
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            fieldInfos[i] = _type.GetField(keyList[i]);
        }

        // 生成实例
        for (int i = 3; i < csvList.Count; i++)
        {
            string[] fileValues = csvList[i].Split(',');

            T obj = Activator.CreateInstance<T>();
            obj.ID = int.Parse(fileValues[0]);

            for (int j = 1; j < fieldInfos.Length; j++)
            {
                SetField(fieldInfos[j], obj, fileValues[j]);
            }

            if (innerDic.Contains(obj.ID))
                Debuger.Log("配置表ID重复！ csv文件：" + csvFileName + " ID : " + obj.ID);

            innerDic.Add(obj.ID, obj);
        }
    }

    // 给字段赋值
    private static void SetField(FieldInfo fieldInfo,object obj,string value)
    {
        switch (fieldInfo.FieldType.Name)
        {
            case "Int32":
                int v;
                if (Int32.TryParse(value, out v))
                    fieldInfo.SetValue(obj, v);
                else
                    fieldInfo.SetValue(obj, 0);
                break;
            case "String":
                if (!string.IsNullOrEmpty(value))
                    fieldInfo.SetValue(obj, value);                
                break;
            case "Boolean":
                int b;
                if (Int32.TryParse(value, out b))
                    fieldInfo.SetValue(obj, b > 0);
                else
                    fieldInfo.SetValue(obj, false);
                break;
            case "Single":
                float f;
                if (float.TryParse(value, out f))
                    fieldInfo.SetValue(obj, f);
                else
                    fieldInfo.SetValue(obj, 0f);
                break;
            case "Int32[]":
                if (string.IsNullOrEmpty(value))
                    fieldInfo.SetValue(obj, new int[0]);
                else
                {
                    string[] strarr = value.Split('$');
                    int[] intarr = new int[strarr.Length];
                    for (int i = 0; i < strarr.Length; i++)
                    {
                        int vv;
                        if (Int32.TryParse(strarr[i], out vv))
                            intarr[i] = vv;
                        else
                            intarr[i] = 0;
                    }
                    fieldInfo.SetValue(obj, intarr);
                }
                break;
            case "String[]":
                if (string.IsNullOrEmpty(value))
                    fieldInfo.SetValue(obj, new string[0]);
                else
                {
                    string[] strarr = value.Split('$');
                    fieldInfo.SetValue(obj, strarr);
                }
                break;
            case "Boolean[]":
                if (string.IsNullOrEmpty(value))
                    fieldInfo.SetValue(obj, new bool[0]);
                else
                {
                    string[] strarr = value.Split('$');
                    bool[] barr = new bool[strarr.Length];
                    for (int i = 0; i < strarr.Length; i++)
                    {
                        int bb;
                        if (Int32.TryParse(strarr[i], out bb))
                            barr[i] = bb > 0;
                        else
                            barr[i] = false;
                    }
                    fieldInfo.SetValue(obj, barr);
                }
                break;
            case "Single[]":
                if (string.IsNullOrEmpty(value))
                    fieldInfo.SetValue(obj, new float[0]);
                else
                {
                    string[] strarr = value.Split('$');
                    float[] farr = new float[strarr.Length];
                    for (int i = 0; i < strarr.Length; i++)
                    {
                        float ff;
                        if (float.TryParse(strarr[i], out ff))
                            farr[i] = ff;
                        else
                            farr[i] = 0f;
                    }
                    fieldInfo.SetValue(obj, farr);
                }
                break;
            default:break;
        }
    }
}

