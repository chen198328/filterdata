using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ESIPaperFilter.Code
{
    public class Paper
    {
        public string Filename { set; get; }
        public string Title { set; get; }
        public string J9 { set; get; }
        public string SO { set; get; }
        public string SN { set; get; }
        public int PY { set; get; }
        public int TC { set; get; }
        public int Z9 { set; get; }
        public bool IsMark { set; get; }
        public List<string> Lines { set; get; }
        public Paper()
        {
            Lines = new List<string>();
        }
        public static List<Paper> Read(string path)
        {
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                string line = string.Empty;
                List<Paper> paperlist = new List<Paper>();
                Paper paper = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string prefix = line.Substring(0, 2);
                    if (prefix == "FN" || prefix == "VR" || prefix == "EF")
                    {
                        continue;

                    }
                    else if (string.IsNullOrWhiteSpace(prefix) && string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    if (prefix == "PT")
                    {
                        paper = new Paper();
                        paper.Filename = Path.GetFileName(path);
                    }
                    if (paper != null)
                    {
                        paper.Lines.Add(line);
                    }
                    if (prefix == "SN")
                        paper.SN = line.Substring(2).Trim();
                    if (prefix == "TI")
                    {
                        paper.Title = line.Substring(2).Trim();
                    }
                    if (prefix == "SO")
                    {
                        paper.SO = line.Substring(2).Trim();
                    }
                    if (prefix == "PY")
                    {
                        int year = 0;
                        int.TryParse(line.Substring(2).Trim(), out year);
                        paper.PY = year;
                    }
                    if (prefix == "Z9")
                    {
                        int z9 = 0;
                        int.TryParse(line.Substring(2).Trim(), out z9);
                        paper.Z9 = z9;
                    }
                    if (prefix == "TC")
                    {
                        int tc = 0;
                        int.TryParse(line.Substring(2).Trim(), out tc);
                        paper.TC = tc;
                    }
                    if (prefix == "J9")
                    {
                        paper.J9 = line.Substring(2).Trim();
                    }
                    if (prefix == "ER")
                        paperlist.Add(paper);
                }
                return paperlist;
            }
        }
    }
}
