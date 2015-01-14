using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterData.Code
{
    public class Converter
    {
        public List<ConvertPaper> cache = new List<ConvertPaper>();
        public Converter() { }
        public virtual string ToHtml(List<string> years)
        {
            StringBuilder content = new StringBuilder();
            if (years == null || years.Count == 0)
            {
                cache = (from p in cache orderby p.TotalCites descending select p).ToList<ConvertPaper>();
            }
            else
            {
                cache = (from p in cache where years.Contains(p.Year) orderby p.TotalCites descending select p).ToList<ConvertPaper>();
            }
            int index = 1;
            foreach (var p in cache)
            {
                content.AppendLine("<tr>");
                //序号
                content.AppendLine("<td>" + (index++).ToString() + "</td>");
                //标题
                content.AppendLine("<td>");
                content.AppendLine("<p><b>标题: </b>" + p.Title + "</p>");
                //作者
                if (!string.IsNullOrEmpty(p.Author))
                {
                    if (p.AuthorCount < 20)
                    {
                        content.AppendLine("<p><b>作者: </b>" + p.Author + "</p>");
                    }
                    else
                    {
                        content.AppendLine("<div class=\"main\">");
                        content.AppendLine("<p class=\"mainf\"><b>作者：</b>");
                        content.AppendLine(p.Author);
                        content.AppendLine("</p>");
                        content.AppendLine("</div>");
                        content.AppendLine("<div class=\"intro\">");

                        content.AppendLine("<span class=\"key\">展开(" + p.AuthorCount + ")</span>");
                        content.AppendLine("<div style=\"clear:both; height:0; overflow:hidden;\"></div>");
                        content.AppendLine("</div>");
                    }
                }
                //关键词
                if (!string.IsNullOrEmpty(p.Keywords))
                {
                    content.AppendLine("<p><b>关键词: </b>" + p.Keywords + "</p>");
                }
                int institutecount = 0;
                if (p.Institutes.Count <= 2)
                {
                    //机构
                    content.AppendLine("<p><b>机构: </b>");
                    string space = string.Empty;
                    p.Institutes.ForEach(i =>
                    {
                        institutecount++;
                        if (institutecount == 2)
                        {
                            space = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                        }
                        content.AppendLine(space + i.Trim() + "<br/>");

                    });
                    content.AppendLine("</p>");
                }
                else if (p.Institutes.Count > 1)
                {
                    content.AppendLine("<div class=\"main\">");
                    content.AppendLine("<p class=\"mainf\"><b>机构: </b>");
                    int pos = 0;
                    p.Institutes.ForEach(i =>
                    {
                        string space = string.Empty;
                        if (pos > 0)
                        {
                            space = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                        }
                        pos++;
                        content.AppendLine(space + i.Trim() + "<br/>");
                    });
                    content.AppendLine("</p>");
                    content.AppendLine("</div>");
                    content.AppendLine("<div class=\"intro\">");
                    content.AppendLine("<span class=\"key\">展开(" + p.Institutes.Count + ")</span>");
                    content.AppendLine("<div style=\"clear:both; height:0; overflow:hidden;\"></div>");
                    content.AppendLine("</div>");
                }

                //来源
                if (!string.IsNullOrEmpty(p.Source))
                {
                    content.AppendLine("<p><b>来源: </b>" + p.Source);
                    if (!string.IsNullOrEmpty(p.Year))
                    {
                        content.AppendFormat(" 年: " + p.Year);
                    }
                    if (!string.IsNullOrEmpty(p.Volume))
                    {
                        content.AppendFormat(" 卷: " + p.Volume);
                    }
                    if (!string.IsNullOrEmpty(p.Issue))
                    {
                        content.AppendFormat(" 期: " + p.Issue);
                    }
                    if (!string.IsNullOrEmpty(p.ArticleNumber))
                    {
                        content.AppendFormat(" 文章编号: " + p.ArticleNumber);
                    }
                    if (!string.IsNullOrEmpty(p.BeginPage) || !string.IsNullOrEmpty(p.BeginPage))
                    {
                        content.AppendFormat(" 页码: " + p.BeginPage + "-" + p.EndPage);
                    }
                    content.AppendLine("</p>");
                }
                if (!string.IsNullOrEmpty(p.ConferenceTitle))
                {
                    content.AppendLine("<p><b>会议: </b>" + p.ConferenceTitle + "</p>");
                    content.AppendLine("<p><b>地点: </b>" + p.ConferenceAddress + "</p>");
                    content.AppendLine("<p><b>时间: </b>" + p.ConferenceDate + "</p>");
                }
                //被引频次
                //content.AppendLine("<p><b>被引频次: </b>" + p.Cites.ToString() + "</p>");
                if (p.isCite)
                {
                    content.AppendLine("<p><b>被引频次: </b>" + p.TotalCites.ToString() + "</p>");
                }
                if (!string.IsNullOrEmpty(p.AccessionNumber))
                {
                    content.AppendLine("<p><b>入藏号: </b>" + p.AccessionNumber + "</p>");
                }
                //DOI
                if (!string.IsNullOrEmpty(p.Doi))
                {
                    content.AppendLine("<p><b>DOI: </b><a href=\"http://doi.org/" + p.Doi + "\"  target=\"_blank\">" + p.Doi + "</a></p>");
                }
                content.AppendLine("</td>");
                content.AppendLine("</tr>");

            }
            return content.ToString();
        }
        public void Clear() { cache.Clear(); }
        public int Count() { return cache.Count; }
    }
}
