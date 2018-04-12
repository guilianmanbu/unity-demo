using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class ScriptsSignature:UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");  // 创建一个脚本后，unity为其自动生成.meta文件，然后根据这个.meta文件的路径去得到对应的.cs脚本
        if (path.EndsWith(".cs"))
        {
            StringBuilder sb = new StringBuilder();

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

            StreamReader sr = new StreamReader(fs);
            bool signed = false;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!signed && !line.StartsWith("using "))
                {
                    signed = true;
                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine("/// Author : xiao niu");
                    sb.AppendLine("/// CreateTime : " + DateTime.Now.ToString());
                    sb.AppendLine("/// </summary>");
                    sb.AppendLine();
                }

                sb.AppendLine(line);
            }

            fs.Seek(0, SeekOrigin.Begin);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Close();
            sr.Close();
            fs.Close();
        }
    }
}

