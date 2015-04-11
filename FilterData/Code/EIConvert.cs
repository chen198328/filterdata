using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FilterData.Code
{
    public class EIConvert : Converter, IConvert
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
                    if (line.Contains("Accession number:"))
                    {
                        paper = new ConvertPaper();
                        paper.AccessionNumber = line.Substring("Accession number:".Length).Trim();
                    }
                    if (line.Contains("Title:"))
                    {
                        paper.Title = line.Substring("Title:".Length).Trim();
                    }
                    if (line.Contains("Authors:"))
                    {
                        paper.Author = line.Substring("Authors:".Length).Trim();
                        if (!string.IsNullOrEmpty(paper.Author))
                        {
                            string[] authors = paper.Author.Split(';');
                            if (authors != null)
                            {
                                paper.AuthorCount = authors.Length;
                            }
                        }
                    }
                    if (line.Contains("Author affiliation:"))
                    {
                        string institutes = line.Substring("Author affiliation:".Length).Trim();
                        string[] _institutes = institutes.Split(';');
                        List<string> instituteslist = new List<string>();
                        if (_institutes != null && _institutes.Length > 0)
                        {
                            for (int index = 0; index < _institutes.Length; index++)
                            {
                                if (_institutes[index].Trim().Length > 0)
                                {
                                    string temp = _institutes[index].Trim();
                                    paper.Institutes.Add(temp.Trim());

                                }
                            }

                        }
                    }

                    if (line.Contains("Source title:"))
                    {
                        paper.Source = line.Substring("Source title:".Length).Trim();
                    }
                    if (line.Contains("Volume:"))
                    {
                        paper.Volume = line.Substring("Volume:".Length).Trim();
                    }
                    if (line.Contains("Issue:"))
                    {
                        paper.Issue = line.Substring("Issue:".Length).Trim();
                    }
                    if (line.Contains("Pages:"))
                    {
                        paper.BeginPage = line.Substring("Pages:".Length).Trim();
                    }
                    if (line.Contains("Publication year:"))
                    {
                        paper.Year = line.Substring("Publication year:".Length).Trim();
                    }
                    if (line.Contains("DOI:"))
                    {
                        paper.Doi = line.Substring("DOI:".Length).Trim();
                    }
                    if (line.Contains("Compilation and indexing terms"))
                    {
                        cache.Add(paper);
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
                //if (!string.IsNullOrEmpty(p.Author))
                //{
                //    content.AppendLine("<p><b>作者: </b>" + p.Author + "</p>");
                //}

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
                if (!string.IsNullOrEmpty(p.Keywords))
                {
                    content.AppendLine("<p><b>关键词: </b>" + p.Keywords + "</p>");
                }
                //if (p.Institutes.Count > 0)
                //{
                //    //机构
                //    content.AppendLine("<p><b>机构: </b>");
                //    p.Institutes.ForEach(i =>
                //    {
                //        content.AppendLine(i.Trim());
                //    });
                //    content.AppendLine("</p>");
                //}
                int institutecount = 0;
                if (p.Institutes.Count <= 2 && p.Institutes.Count > 0)
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
                else
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
                    if (!string.IsNullOrEmpty(p.BeginPage))
                    {
                        content.AppendFormat(" 页码: " + p.BeginPage);
                    }
                    content.AppendLine("</p>");
                }
                //DOI
                if (!string.IsNullOrEmpty(p.Doi))
                {
                    content.AppendLine("<p><b>DOI:   </b><a target=\"_blank\" href=\"http://doi.org/" + p.Doi + "\">" + p.Doi + "</a></p>");
                }
                if (!string.IsNullOrEmpty(p.AccessionNumber))
                {
                    content.AppendLine("<p><b>入藏号: </b>" + p.AccessionNumber + "</p>");
                }

                content.AppendLine("</td>");
                content.AppendLine("</tr>");

            }
            return content.ToString();
        }
    }
}