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
        /// 字符串比较，忽略大小写
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(List<string> list, string item)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (list[index].ToLower() == item.ToLower())
                    return true;
            }
            return false;
        }
    }
}
