using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApp1
{
    static class CommonConfig
    {
        /// <summary>
        /// 用于存储映射
        /// </summary>
        public static Dictionary<String, String> fileHead = new Dictionary<string, string> ();

        /// <summary>
        /// 重置所有映射
        /// </summary>
        public static void RefreshAll()
        {
            fileHead.Clear();
            fileHead.Add("FFD8","jpg");
            fileHead.Add("8950","png");
            fileHead.Add("4749","gif");
        }

        public static String AddToFileHead(String key, String value)
        {
            //格式校验
            if (key.Length != 4)
            {
                return "文件头的长度应该是4个";
            }
            char[] ch = key.ToCharArray();
            foreach (char c in ch)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
                {
                    continue;
                }
                else
                {
                    return "文件头应只存在十六进制的表示即0->F";
                }
            }
            if (fileHead.ContainsKey(key.ToUpper()))
            {
                fileHead.Remove(key.ToUpper());
                fileHead.Add(key.ToUpper(), value);
                return "覆盖成功";
            }
            fileHead.Add(key.ToUpper(), value);
            return "添加成功";
        }

        public static String ShowKV()
        {
            String result = "已存在的映射：";
            foreach (var item in fileHead)
            {
                result += " [" + item.Key + "-" + item.Value + "] ";
            }
            return result;
        }
    }
}
