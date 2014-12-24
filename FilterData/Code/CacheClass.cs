using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FilterData.Code
{
    /// <summary>
    /// 缓存类，保存数据
    /// </summary>
    [Serializable]
    public class CacheClass
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 输入文件名
        /// </summary>
        public string Filenames { set; get; }
        public string[] MemoryFilenames { set; get; }
        /// <summary>
        /// 保存原始的二级数据表
        /// </summary>
        public DataTable MemoryTable { set; get; }
        /// <summary>
        /// 一级机构列表
        /// </summary>
        public List<UniversityItem> UniverisityList { set; get; }
        ///二级机构表
        public DataTable InsituteTable { set; get; }
        public bool isOpen { set; get; }
        public IFilter ifilter { set; get; }
        public string Fileformat { set; get; }
        /// <summary>
        /// 一级机构过滤
        /// </summary>
        public string FilterInstiute { set; get; }
        public CacheClass() { }
    }
}
