using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace FilterData.Code
{
    public class CSCDConverter : Converter, IConvert
    {
        Regex regYear = new Regex("[0-9]{4,4}");

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
                    if (line.Contains("题名："))
                    {
                        paper = new ConvertPaper();
                        paper.Title = line.Substring("题名：".Length).Trim();
                    }
                    if (line.Contains("作者："))
                    {
                        paper.Author = line.Substring("作者：".Length).Trim();
                    }
                    if (line.Contains("单位："))
                    {
                        paper.Institutes = line.Substring("单位：".Length).Trim().Split(';').ToList<string>();
                    }
                    if (line.Contains("关键词："))
                    {
                        paper.Keywords = line.Substring("关键词：".Length).Trim();
                    }
                    if (line.Contains("来源："))
                    {
                        paper.Source = line.Substring("来源：".Length).Trim();
                        string[] splits = paper.Source.Split(',');
                        paper.Year = splits[1].Trim();
                        Match match = regYear.Match(paper.Year);
                        if (match.Success)
                        {
                            paper.Year = match.Value;
                        }

                    }
                    if (line.Contains("被引频次："))
                    {
                        int cites = 0;
                        int.TryParse(line.Substring("被引频次：".Length).Trim(), out cites);
                        paper.TotalCites = cites;
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
                        content.AppendLine(i.Trim() + ";");
                    });
                    content.AppendLine("</p>");
                }

                //来源
                if (!string.IsNullOrEmpty(p.Source))
                {
                    content.AppendLine("<p><b>来源: </b>" + p.Source + "</p>");
                }
                content.AppendLine("<p><b>被引频次: </b>" + p.TotalCites + "</p>");
                content.AppendLine("</td>");
                content.AppendLine("</tr>");

            }
            return content.ToString();
        }
    }
}
