using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterData.Code
{
    public interface IConvert
    {
        /// <summary>
        /// 清楚缓存中数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 缓存中的记录数据
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// 数据添加到缓存中
        /// </summary>
        /// <param name="filename"></param>
        void Read(string filename);
        /// <summary>
        /// 根据模板生成html
        /// </summary>
        /// <returns></returns>
        string ToHtml(List<string> years);
    }
}
