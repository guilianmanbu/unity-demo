using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

public class Csv2Obj_tool
{
    /// <summary>
    /// 是否已存在该obj文件
    /// </summary>
    /// <param name="objFileName"></param>
    /// <returns></returns>
    private static FileInfo hasObjFile(string objFileName,ref FileInfo[] objFileList)
    {
        for (int i = 0; i < objFileList.Length; i++)
        {
            if (objFileList[i].Name.Equals(objFileName))
                return objFileList[i];
        }
        return null;
    }

    /// <summary>
    /// csv文件名转为cs类名
    /// </summary>
    /// <param name="csvName"></param>
    /// <returns></returns>
    private static string CsvName2ObjName(string csvName)
    {
        string[] parts = csvName.Replace(".csv", "").Split('_');
        string objname = string.Empty;
        for (int i = 0; i < parts.Length; i++)
        {
            objname += parts[i].Substring(0, 1).ToUpper() + parts[i].Substring(1);
        }
        return objname;
    }

    /// <summary>
    /// 从csv文件生成obj文件
    /// </summary>
    public static void Start()
    {
        string csvFolder = CsvDatas.csvFolder;

        string objFolder = @"E:\UnityWorkspace\ClientTest\ClientTest\csv\obj";

        string csvDatasFilePath = @"E:\UnityWorkspace\ClientTest\ClientTest\csv\CsvDatas.cs";

        DirectoryInfo csvDir = new DirectoryInfo(csvFolder);
        if (!csvDir.Exists) { Debuger.Log("Not Exist:" + csvFolder); return; }

        DirectoryInfo objDir = new DirectoryInfo(objFolder);
        if (!objDir.Exists) { Debuger.Log("Not Exist:" + objFolder); return; }

        FileInfo csvDatasFile = new FileInfo(csvDatasFilePath);
        if (!csvDatasFile.Exists) { Debuger.Log("Not Exist:" + csvDatasFilePath); return; }

        FileInfo[] oldObjs = objDir.GetFiles("*.cs",SearchOption.TopDirectoryOnly);

        FileInfo[] csvFiles = csvDir.GetFiles("*.csv", SearchOption.TopDirectoryOnly);

        StringBuilder objContentReplaceStr = new StringBuilder();
        StringBuilder csvDatasContentReplaceStr = new StringBuilder();

        for (int i = 0; i < csvFiles.Length; i++)
        {
            FileInfo csvFile = csvFiles[i];

            // 读取csv
            List<string> content = new List<string>();
            using (StreamReader sr=csvFile.OpenText())
            {
                while (sr.Peek() > 0)
                {
                    content.Add(sr.ReadLine());
                }
            }

            string[] remList = content[0].Split(',');
            string[] typeList = content[1].Split(',');
            string[] keyList = content[2].Split(',');

            // 生成字段
            objContentReplaceStr.Clear();
            for (int j = 1; j < keyList.Length; j++)
            {
                objContentReplaceStr.AppendLine("\t/// <summary>");
                objContentReplaceStr.AppendLine("\t/// " + remList[j]);
                objContentReplaceStr.AppendLine("\t/// </summary>");
                objContentReplaceStr.AppendLine(string.Format("\tpublic {0} {1};\n", typeList[j], keyList[j]));
            }

            // 类名
            string objName = CsvName2ObjName(csvFile.Name);

            csvDatasContentReplaceStr.AppendLine(string.Format("\t\tLoadCsv<{0}>(\"{1}\");", objName, csvFile.Name));

            // 写入cs文件
            FileInfo objFile = hasObjFile(objName + ".cs", ref oldObjs);
            if (objFile == null)   
            {
                // 创建新的
                objFile = new FileInfo(objFolder + "\\" + objName + ".cs");
                using(StreamWriter sw = objFile.CreateText())
                {
                    sw.WriteLine("public class " + objName + " : CsvBase");
                    sw.WriteLine("{");
                    sw.WriteLine("// replace start");
                    sw.Write(objContentReplaceStr.ToString());
                    sw.WriteLine("// replace end");
                    sw.WriteLine("}");
                }
            }
            else
            {
                // 已存在
                string objcontent = string.Empty;
                using (StreamReader sr = objFile.OpenText())
                    objcontent = sr.ReadToEnd();

                Regex regex = new Regex(@"// replace start([\s\S]*)// replace end");
                objcontent = regex.Replace(objcontent, string.Format("// replace start\n{0}// replace end", objContentReplaceStr.ToString()));

                using (StreamWriter sw = new StreamWriter(objFile.OpenWrite()))
                    sw.Write(objcontent);
            }

            Debuger.Log("create cs file ok :  " + objName);
        }

        // 读取csvdatas
        string csvdatascontent = string.Empty;
        using (StreamReader sr = csvDatasFile.OpenText())
        {
            csvdatascontent = sr.ReadToEnd();
        }

        // 替换
        Regex reg = new Regex(@"#region auto[\s\S]*#endregion");
        csvdatascontent = reg.Replace(csvdatascontent, string.Format("#region auto\n{0}#endregion",csvDatasContentReplaceStr.ToString()));

        // 写入 csvdatas
        using(StreamWriter sw=new StreamWriter(csvDatasFile.OpenWrite()))
        {
            sw.Write(csvdatascontent);
        }

        Debuger.Log("modify file ok :  " + csvDatasFile.Name);
        Debuger.Log("done");
    }
}
