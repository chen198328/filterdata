using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterData.Code
{
    public class StringUtil
    {
        /// <summary>
        /// 字符串比较，忽略大小写,相等匹配
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool EqualIgnoreCase(List<string> list, string item)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (list[index].ToLower() == item.ToLower())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 字符串比较，忽略大小写,宝航匹配 前者包含后者
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Contains(string line, List<string> items, bool ignoarecase = true)
        {
            if (ignoarecase)
            {
                for (int index = 0; index < items.Count; index++)
                {
                    string p = items[index].Trim();
                    if (p.StartsWith("-"))
                    {
                        p = p.Substring(1);
                        if (!line.ToLower().Contains(p))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (line.ToLower().Contains(p))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int index = 0; index < items.Count; index++)
                {
                    string p = items[index].Trim();
                    if (p.StartsWith("-"))
                    {
                        p = p.Substring(1);
                        if (!line.Contains(p))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (line.Contains(p))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }
}
