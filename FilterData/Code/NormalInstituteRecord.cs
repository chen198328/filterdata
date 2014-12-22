using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterData.Code
{
    /// <summary>
    /// 二级规范表记录,主要用于其系统
    /// </summary>
    public class NormalInstituteRecord
    {
        public NormalInstituteRecord() { }
        /// <summary>
        /// 一级机构名
        /// </summary>
        public string 一级机构 { set; get; }
        /// <summary>
        /// 二级机构名
        /// </summary>
        public string 二级机构 { set; get; }
        /// <summary>
        /// 规范学科
        /// </summary>
        public string 学科规范 { set; get; }
        /// <summary>
        ///规范学院
        /// </summary>
        public string 学院规范 { set; get; }
        /// <summary>
        /// 规范学校
        /// </summary>
        public string 学校规范 { set; get; }
    }
}
