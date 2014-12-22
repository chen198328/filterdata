using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FilterData.Code
{
    public class EndNoteConvert : Converter, IConvert
    {
        /// <summary>
        /// 读取数据进入缓存
        /// </summary>
        /// <param name="filename"></param>
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
                    if (line.Length == 0) continue;
                    prefix = line.Substring(0, 2);
                    if (prefix == "  ")
                        prefix = field;
                    switch (prefix)
                    {
                        case "PT":
                            paper = new ConvertPaper();
                            break;
                        case "TI":
                            field = "TI";
                            paper.Title += line.Substring(3) + " ";
                            break;
                        case "AU":
                            field = "AU";
                            paper.Author_ += "; " + line.Substring(3).Trim();
                            paper.Author_Count++;
                            break;
                        case "AF":
                            field = "AF";
                            paper.Author += "; " + line.Substring(3);
                            paper.AuthorCount++;
                            break;
                        case "DE":
                            field = "DE";
                            paper.Keywords += " " + line.Substring(3);
                            break;
                        case "ID":
                            field = "ID";
                            paper.Keywords_ += " " + line.Substring(3);
                            break;
                        case "SO":
                            field = "SO";
                            paper.Source = line.Substring(3).Trim();
                            break;
                        case "C1":
                            field = "C1";
                            paper.Institutes.Add(line.Substring(3));
                            break;
                        case "Z9":
                            field = "Z9";
                            int cites = 0;
                            int.TryParse(line.Substring(3).Trim(), out cites);
                            paper.Cites = cites;
                            break;
                        case "TC":
                            field = "TC";
                            int totalcites = 0;
                            int.TryParse(line.Substring(3).Trim(), out totalcites);
                            paper.TotalCites = totalcites;
                            paper.isCite = true;
                            break;
                        case "PY":
                            field = "PY";
                            paper.Year = line.Substring(3).Trim();
                            break;
                        case "VL":
                            field = "VL";
                            paper.Volume = line.Substring(3).Trim();
                            break;
                        case "IS":
                            field = "IS";
                            paper.Issue = line.Substring(3).Trim();
                            break;
                        case "AR":
                            field = "AR";
                            paper.ArticleNumber = line.Substring(3).Trim();
                            break;
                        case "DI":
                            field = "DI";
                            paper.Doi = line.Substring(3).Trim();
                            break;
                        case "BP":
                            field = "BP";
                            paper.BeginPage = line.Substring(3).Trim();
                            break;
                        case "EP":
                            field = "EP";
                            paper.EndPage = line.Substring(3).Trim();
                            break;
                        case "CT":
                            field = "CT";
                            paper.ConferenceTitle += line.Substring(3).Trim() + " ";
                            break;
                        case "CL":
                            field = "CL";
                            paper.ConferenceAddress += line.Substring(3).Trim() + " ";
                            break;
                        case "CY":
                            field = "CY";
                            paper.ConferenceDate += line.Substring(3).Trim() + " ";
                            break;
                        case "UT":
                            field = "UT";
                            paper.AccessionNumber += line.Substring(3).Trim() + " ";
                            break;
                        case "ER":
                            if (!string.IsNullOrEmpty(paper.Author))
                            {
                                paper.Author = paper.Author.Substring(1);
                            }
                            else if (!string.IsNullOrEmpty(paper.Author_))
                            {
                                paper.Author = paper.Author_.Substring(1);
                                paper.AuthorCount = paper.Author_Count;
                            }
                            if (!string.IsNullOrEmpty(paper.Keywords))
                            {
                                paper.Keywords = paper.Keywords.Substring(1);
                            }
                            else if (!string.IsNullOrEmpty(paper.Keywords_))
                            {
                                paper.Keywords = paper.Keywords_.Substring(1);
                            }
                            cache.Add(paper);
                            break;
                        default:
                            field = "";
                            break;
                    }
                }
                reader.Close();
            }
        }

    }
}
