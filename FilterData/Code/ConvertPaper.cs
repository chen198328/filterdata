using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterData.Code
{
    public class ConvertPaper
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 作者,兼容Medline的AU
        /// </summary>
        public string Author_ { set; get; }
        public int Author_Count { set; get; }
        public int AuthorCount { set; get; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { set; get; }
        /// <summary>
        /// 机构
        /// </summary>
        public List<string> Institutes { set; get; }
        /// <summary>
        /// 来源，包括期刊、年、卷、期
        /// </summary>
        public string Source { set; get; }
        /// <summary>
        /// 年
        /// </summary>
        public string Year { set; get; }
        /// <summary>
        /// 卷
        /// </summary>
        public string Volume { set; get; }
        /// <summary>
        /// 期
        /// </summary>
        public string Issue { set; get; }
        /// <summary>
        /// 文章号
        /// </summary>
        public string ArticleNumber { set; get; }
        public bool isCite { set; get; }
        /// <summary>
        /// 所有数据库的被引频次，不同数据库不同被引频次
        /// </summary>
        public int Cites { set; get; }
        /// <summary>
        /// 核心集被引频次
        /// </summary>
        public int TotalCites { set; get; }
        /// <summary>
        /// DOI
        /// </summary>
        public string Doi { set; get; }
        /// <summary>
        /// 开始页
        /// </summary>
        public string BeginPage { set; get; }
        /// <summary>
        /// 结束页
        /// </summary>
        public string EndPage { set; get; }
        public string AccessionNumber { set; get; }
        public string Keywords { set; get; }
        /// <summary>
        /// 用于兼容Medline
        /// </summary>
        public string Keywords_ { set; get; }
        /// <summary>
        /// 会议时间
        /// </summary>
        public string ConferenceTitle { set; get; }
        /// <summary>
        /// 会议地点
        /// </summary>
        public string ConferenceAddress { set; get; }
        /// <summary>
        /// 会议时间
        /// </summary>
        public string ConferenceDate { set; get; }
        public ConvertPaper()
        {
            Institutes = new List<string>();
            isCite = false;
        }

    }
}
