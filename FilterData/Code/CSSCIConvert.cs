using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FilterData.Code
{
    public class CSSCIConvert : Converter, IConvert
    {

        public void Read(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line = string.Empty;
                ConvertPaper paper = null;
                string field = string.Empty;
                string prefix = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("【文件序号】"))
                    {
                        paper = new ConvertPaper();
                        paper.AccessionNumber = line.Substring(6).Trim();
                    }
                    if (line.Contains("【来源篇名】"))
                    {
                        paper.Title = line.Substring(6).Trim();
                    }
                    if (line.Contains("【来源作者】"))
                    {
                        paper.Author = line.Substring(6).Trim();
                    }
                    if (line.Contains("【关 键 词】"))
                    {
                        paper.Keywords = line.Substring("【关 键 词】".Length).Trim();
                    }
                    if (line.Contains("【机构名称】"))
                    {
                        paper.Institutes.Add(line.Substring(6).Trim());
                    }
                    if (line.Contains("【期    刊】"))
                    {
                        paper.Source = line.Substring("【期    刊】".Length).Trim();
                    }
                    if (line.Contains("【年代卷期】"))
                    {
                        paper.Volume = line.Substring("【年代卷期】".Length).Trim();
                        paper.Year = line.Substring("【年代卷期】".Length,4).Trim();
                    }
                    if (line.Contains("----"))
                    {
                        cache.Add(paper);
                        paper = null;
                    }

                }
                reader.Close();
            }
        }
        public override string ToHtml(List<string> years)
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
                    content.AppendLine("<p><b>作者: </b>" + p.Author + "</p>");
                }
                if (!string.IsNullOrEmpty(p.Keywords))
                {
                    content.AppendLine("<p><b>关键词: </b>" + p.Keywords + "</p>");
                }
                if (p.Institutes.Count > 0)
                {
                    //机构
                    content.AppendLine("<p><b>机构: </b>");
                    p.Institutes.ForEach(i =>
                    {
                        content.AppendLine(i.Trim());
                    });
                    content.AppendLine("</p>");
                }

                //来源
                if (!string.IsNullOrEmpty(p.Source))
                {
                    content.AppendLine("<p><b>来源: </b>" + p.Source);
                    if (!string.IsNullOrEmpty(p.Volume))
                    {
                        content.AppendFormat(p.Volume);
                    }
                    content.AppendLine("</p>");
                }
                //DOI
                if (!string.IsNullOrEmpty(p.AccessionNumber))
                {
                    content.AppendLine("<p><b>入藏号: </b><a target=\"_blank\" href=\"http://cssci.nju.edu.cn/ly_search_list.html?id=" + p.AccessionNumber + "\">" + p.AccessionNumber + "</a></p>");
                }
                content.AppendLine("</td>");
                content.AppendLine("</tr>");

            }
            return content.ToString();
        }
    }
}
