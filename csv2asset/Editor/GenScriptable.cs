using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Author : xiao niu
/// CreateTime : 4/26/2018 5:44:08 PM
/// </summary>

class GenScriptable
{
    public static string objFolder = @"E:\UnityWorkspace\Test\Assets\Scripts\csv\obj";
    public static string csvFolder= @"E:\UnityWorkspace\Common\config-table";
    public static string assetsFolder = "Assets/Resources/" + ConfigManager.tableAssetsFolder;
    public static string configManagerPath = @"E:\UnityWorkspace\Test\Assets\Scripts\csv\ConfigManager.cs";
    public static string genScriptablePath = @"E:\UnityWorkspace\Test\Assets\Editor\GenScriptable.cs";

    [MenuItem("Tools/配置表/更新表结构",priority =1)]
    public static void Csv2Obj()
    {
        DirectoryInfo csvDir = new DirectoryInfo(csvFolder);
        if (!csvDir.Exists) { Debug.Log("Not Exist:" + csvFolder); return; }

        DirectoryInfo objDir = new DirectoryInfo(objFolder);
        if (!objDir.Exists) { Debug.Log("Not Exist:" + objFolder); return; }


        //FileInfo[] oldObjs = objDir.GetFiles("*.cs", SearchOption.TopDirectoryOnly);
        //for (int i = 0; i < oldObjs.Length; i++)
        //    oldObjs[i].Delete();

        FileInfo[] csvFiles = csvDir.GetFiles("*.csv", SearchOption.TopDirectoryOnly);

        StringBuilder configManagerContentReplaceStr = new StringBuilder();
        StringBuilder genScriptableContentReplaceStr = new StringBuilder();

        for (int i = 0; i < csvFiles.Length; i++)
        {
            FileInfo csvFile = csvFiles[i];

            // 类名
            string objName = CsvName2ObjName(csvFile.Name);

            // 表名
            string tableName = objName + "Table";

            // 资源名
            string assetName = tableName;

            configManagerContentReplaceStr.AppendLine( string.Format("\t\tLoadTable<{0}, {1}>(\"{2}\");", tableName, objName, assetName));
            genScriptableContentReplaceStr.AppendLine(string.Format("\t\ttemp = CreateAsset<{0}, {1}>(\"{2}\", \"{3}\");", tableName, objName, csvFile.Name, assetName + ".asset"));

            // 读取csv
            List<string> content = new List<string>();
            using (StreamReader sr = csvFile.OpenText())
            {
                while (sr.Peek() > 0 && content.Count<3)
                {
                    content.Add(sr.ReadLine());
                }
            }

            string[] remList = content[0].Split(',');
            string[] typeList = content[1].Split(',');
            string[] keyList = content[2].Split(',');

            // 生成字段
            StringBuilder objContentReplaceStr = new StringBuilder();
            for (int j = 1; j < keyList.Length; j++)
            {
                objContentReplaceStr.AppendLine("\t/// <summary>");
                objContentReplaceStr.AppendLine("\t/// " + remList[j]);
                objContentReplaceStr.AppendLine("\t/// </summary>");
                objContentReplaceStr.AppendLine(string.Format("\tpublic {0} {1};\n", typeList[j], keyList[j]));
            }

            // 创建新的cs文件
            FileInfo objFile = new FileInfo(objFolder + "\\" + objName + ".cs");
            using (StreamWriter sw = new StreamWriter(objFile.Open(FileMode.OpenOrCreate, FileAccess.Write)))
            {
                // obj类
                sw.WriteLine("using System;\n");
                sw.WriteLine("[Serializable]");
                sw.WriteLine("public class " + objName + " : CsvBase");
                sw.WriteLine("{");
                sw.Write(objContentReplaceStr.ToString());
                sw.WriteLine("}");
                // table类
                sw.WriteLine("public partial class " + tableName + " : TableGeneric<" + objName + "> { }");
            }

            Debug.Log("create cs file ok :  " + objName);
        }

        // 修改 configManager
        ModifyFile(configManagerPath, configManagerContentReplaceStr.ToString());

        // 修改 genScriptableFile
        ModifyFile(genScriptablePath, genScriptableContentReplaceStr.ToString());

        Debug.Log("OK");
    }

    [MenuItem("Tools/配置表/更新表数据",priority =2)]
    public static void CreateScriAsset()
    {
        ScriptableObject temp = null;

        #region auto
#endregion auto

        //AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = temp;
    }

    

    private static string CsvName2ObjName(string csvName)
    {
        string[] parts = csvName.Replace(".csv", "").Split('_');
        string objname = string.Empty;
        for (int i = 0; i < parts.Length; i++)
        {
            objname += parts[i].Substring(0, 1).ToUpper() + parts[i].Substring(1);
        }
        return objname + "Obj";
    }

    private static void ModifyFile(string filePath,string replacement)
    {
        // 读取 content
        string content = File.ReadAllText(filePath);

        // 替换
        Regex reg = new Regex("#region " + @"auto[\s\S]*#endregion"+" auto");
        content = reg.Replace(content, string.Format("#region " + "auto\n{0}#endregion" + " auto", replacement, 1));

        // 写入 cnfigManager
        File.WriteAllText(filePath, content, Encoding.UTF8);

        string filename = filePath.Substring(filePath.LastIndexOf('\\'));
        Debug.Log("modify file ok :  " + filename);
    }

    private static TableBase CreateAsset<T,D>(string csvFile,string assetName) 
        where T : TableGeneric<D> where D : CsvBase,new()
    {
        FileInfo csvFileInfo = new FileInfo(csvFolder + "\\" + csvFile);

        // 读取
        List<string> csvList = new List<string>();
        using (StreamReader sr = new StreamReader(csvFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
        {
            while (sr.Peek() > 0)
                csvList.Add(sr.ReadLine());
        }

        Type objType = typeof(D);

        // 匹配字段顺序
        string[] keyList = csvList[2].Split(',');
        FieldInfo[] fieldInfos = new FieldInfo[keyList.Length];
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            fieldInfos[i] = objType.GetField(keyList[i]);
        }

        // 创建数据实例
        Dictionary<int,D> datas = new Dictionary<int, D>();
        for (int i = 3; i < csvList.Count; i++)
        {
            D data = GetInstance<D>(ref fieldInfos, csvList[i]);

            if (datas.Keys.Contains(data.ID))
                Debug.LogWarning("配置表ID重复！ csv文件：" + csvFile + " ID : " + data.ID);
            else
                datas.Add(data.ID,data);
        }

        // 创建表资源
        T t = ScriptableObject.CreateInstance<T>();
        t.list = datas.Values.ToArray();
        AssetDatabase.CreateAsset(t, assetsFolder + assetName);

        return t;
    }

    private static D GetInstance<D>(ref FieldInfo[] fieldInfos, string content) where D : CsvBase,new()
    {
        string[] fieldValues = content.Split(',');

        D data = new D();
        data.ID = int.Parse((fieldValues[0]));

        for (int i = 1; i < fieldValues.Length; i++)
        {
            SetField(fieldInfos[i], data, fieldValues[i]);
        }

        return data;
    }

    // 给字段赋值
    private static void SetField(FieldInfo fieldInfo, object obj, string value)
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
            default: break;
        }
    }
}

//  the end