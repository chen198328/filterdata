using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
namespace FilterData.Code
{
    //public class Filters
    //{
    //    public Filters() { }
    //    public List<string> GetFieldsFromEndNote(StreamReader reader)
    //    {
    //        List<string> field = new List<string>();
    //        string line = string.Empty;
    //        string prefix = string.Empty;
    //        bool mark = false;
    //        while ((line = reader.ReadLine()) != null)
    //        {
    //            if (line.Length > 2)
    //            {

    //                prefix = line.Substring(0, 2);
    //                if (prefix == "C1" || (prefix == "  " && mark))
    //                {
    //                    field.Add(line.Substring(2).Trim());
    //                    mark = true;
    //                }
    //                else
    //                {
    //                    mark = false;
    //                }
    //            }
    //        }
    //        return field;
    //    }
    //    public List<string> GetFieldsFromEI(StreamReader reader)
    //    {
    //        //List<string> field = new List<string>();
    //        //string line = string.Empty;
    //        //string prefix = "Author affiliation".ToLower();
    //        //while ((line = reader.ReadLine()) != null)
    //        //{
    //        //    if (line.ToLower().StartsWith(prefix)) { 
    //        //        string[] institutes=
    //        //    }
    //        //}
    //        return null;
    //    }
    //}
    //public class InstituteString
    //{
    //    public static List<string> GetFields(string filename)
    //    {
    //        List<string> field = new List<string>();
    //        using (StreamReader reader = new StreamReader(filename))
    //        {
    //            string line = string.Empty;
    //            string prefix = string.Empty;
    //            bool mark = false;
    //            while ((line = reader.ReadLine()) != null)
    //            {
    //                if (line.Length > 2)
    //                {

    //                    prefix = line.Substring(0, 2);
    //                    if (prefix == "C1" || (prefix == "  " && mark))
    //                    {
    //                        field.Add(line.Substring(2).Trim());
    //                        mark = true;
    //                    }
    //                    else
    //                    {
    //                        mark = false;
    //                    }
    //                }
    //            }
    //        }
    //        return field;
    //    }
    //    /// <summary>
    //    /// 去重，且根据机构数量排序
    //    /// </summary>
    //    /// <param name="fields"></param>
    //    /// <returns></returns>

    //    public static List<string> GetUniversity(List<string> fields)
    //    {
    //        fields = RemoveAuthors(fields);
    //        Dictionary<string, int> Universities = new Dictionary<string, int>();
    //        foreach (var field in fields)
    //        {
    //            int index = field.IndexOf(',');
    //            string temp = string.Empty;
    //            if (index != -1)
    //            {
    //                temp = field.Substring(0, index).Trim();
    //            }
    //            else { temp = field; }
    //            if (Universities.ContainsKey(temp))
    //            {
    //                Universities[temp] += 1;
    //            }
    //            else
    //            {
    //                Universities[temp] = 1;
    //            }
    //        }
    //        List<string> result = new List<string>();
    //        foreach (var item in Universities.OrderByDescending(k => k.Value))
    //        {
    //            result.Add(item.Key);
    //        }
    //        return result;
    //    }
    //    private static List<string> RemoveAuthors(List<string> fields)
    //    {
    //        List<string> fieldsWithoutAuthors = new List<string>();
    //        foreach (var field in fields)
    //        {
    //            if (field.IndexOf('[') != -1 && field.IndexOf(']') != -1)
    //            {
    //                string temp = field.Substring(field.IndexOf(']') + 1).Trim();
    //                temp = CheckSchool(temp);
    //                fieldsWithoutAuthors.Add(temp);
    //            }
    //            else
    //            {
    //                string temp = CheckSchool(field);
    //                fieldsWithoutAuthors.Add(temp);
    //            }
    //        }

    //        return fieldsWithoutAuthors.Distinct().ToList<string>();
    //    }
    //    /// <summary>
    //    /// 创建一个空表格
    //    /// </summary>
    //    /// <returns></returns>
    //    public static DataTable CreateInsituteTable()
    //    {
    //        #region 创建表
    //        DataTable InstituteTable = new DataTable();
    //        DataColumn University = new DataColumn("一级机构", typeof(string));
    //        DataColumn School = new DataColumn("二级机构", typeof(string));
    //        DataColumn Subject = new DataColumn("学科规范", typeof(string));
    //        DataColumn Standard = new DataColumn("学院规范", typeof(string));
    //        DataColumn UniversityCh = new DataColumn("学校规范", typeof(string));
    //        InstituteTable.Columns.Add(University);
    //        InstituteTable.Columns.Add(School);
    //        InstituteTable.Columns.Add(Subject);
    //        InstituteTable.Columns.Add(Standard);
    //        InstituteTable.Columns.Add(UniversityCh);
    //        #endregion
    //        return InstituteTable;
    //    }
    //    public static DataTable GetDataTable(List<string> fields)
    //    {
    //        fields = RemoveAuthors(fields);

    //        DataTable InstituteTable = CreateInsituteTable();

    //        DataRow dr = InstituteTable.NewRow();
    //        foreach (var field in fields)
    //        {
    //            if (!string.IsNullOrEmpty(field))
    //            {

    //                int index = field.IndexOf(',');
    //                if (index != -1)
    //                {
    //                    dr["一级机构"] = field.Substring(0, index);
    //                    dr["二级机构"] = field.Substring(index + 1);
    //                }
    //                else
    //                {
    //                    dr["一级机构"] = field;
    //                }
    //                InstituteTable.Rows.Add(dr.ItemArray);
    //            }
    //        }
    //        return InstituteTable;
    //    }
    //    static Regex regPostCode = new Regex("[0-9]+");
    //    /// <summary>
    //    /// 将字符串截断到包含数字的位置
    //    /// </summary>
    //    /// <param name="line"></param>
    //    /// <returns></returns>
    //    static string CheckSchool(string line)
    //    {
    //        int start = line.IndexOf(',');
    //        if (start != -1)
    //        {
    //            start = line.IndexOf(',', start);
    //            if (start != -1)
    //            {
    //                Match match = regPostCode.Match(line, start);
    //                if (match.Success)
    //                {
    //                    int end = line.IndexOf(match.Value, start);
    //                    end = line.LastIndexOf(',', end);
    //                    line = line.Substring(0, end);
    //                    return line;
    //                }
    //                else
    //                {
    //                    return line;
    //                }
    //            }
    //            else
    //            {
    //                return line;
    //            }

    //        }
    //        else
    //        {
    //            return line;
    //        }

    //    }
    //}
    /// <summary>
    /// 机构匹配
    /// </summary>
    public class InstiuteMatch
    {
        public List<UniversityInsituties> UniversityInstituteList = new List<UniversityInsituties>();
        public InstiuteMatch() { }
        /// <summary>
        /// 添加机构,将二级机构转为小写
        /// </summary>
        /// <param name="university"></param>
        /// <param name="institute"></param>
        public void Add(string university, string institute)
        {
            institute = institute.ToLower();
            var match = UniversityInstituteList.FindLast(x => x.Name == university);
            if (match != null)
            {
                match.Insitutes.Add(institute);
                match.Insitutes = match.Insitutes.Distinct(new Compare()).ToList<string>();
                //长度优先
                match.Insitutes.Sort(new CompareLength());
            }
            else
            {
                UniversityInstituteList.Add(new UniversityInsituties() { Name = university, Insitutes = new List<string>() { institute } });
            }

        }
        /// <summary>
        /// 匹配地址字段内容
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool Match(string line)
        {
            int start = 0;
            int end = 0;
            while ((start = line.IndexOf('[')) != -1 && (end = line.IndexOf(']')) != -1)
            {
                line = line.Remove(start, end - start + 1);
            }
            // line = reg.Replace(line, "");
            string[] institutes = line.Split(new string[] { ";", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //for (int index = 0; index < institutes.Length; index++)
            //{
            //    int pos = line.IndexOf(']');
            //    if (pos != -1)
            //        institutes[index] = institutes[index].Substring(pos + 1);
            //}
            foreach (var univ in UniversityInstituteList)
            {
                foreach (var institute in institutes)
                {
                    //先找匹配的学校
                    if (institute.Contains(univ.Name))
                    {
                        foreach (var insti in univ.Insitutes)
                        {
                            if (institute.ToLower().Contains(insti.Trim()))
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
    public class CompareLength : IComparer<string>
    {

        public int Compare(string x, string y)
        {
            if (x.Length > y.Length)
                return 1;
            else if (x.Length < y.Length)
                return -1;
            else return 0;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UniversityInsituties
    {
        /// <summary>
        /// 一级机构
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 二级机构,一律转为小写
        /// </summary>
        public List<string> Insitutes = new List<string>();
        public UniversityInsituties()
        {

        }
    }

    public class PaperList
    {
        /// <summary>
        /// 标识行内容
        /// </summary>
        public string CopyRight { set; get; }
        public List<Paper> paperlist = new List<Paper>();
        /// <summary>
        /// 待匹配的内容
        /// </summary>
        public InstiuteMatch Match { set; get; }
        public IReader ireader;
        public PaperList(InstiuteMatch match)
        {
            Match = match;
        }
        /// <summary>
        /// 清除PaperList
        /// </summary>
        public void Clear()
        {
            paperlist.Clear();
        }
        /// <summary>
        /// 读取文档
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="fileformat">文件格式</param>
        public void ReadFile(string filename, string fileformat = null)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                if (string.IsNullOrEmpty(fileformat))
                    fileformat = GetFileFomat(filename);
                ireader = GetFileReader(fileformat);
                CopyRight = ireader.ReadCopyRight(reader);
                List<Paper> templist = ireader.Read(reader);
                paperlist.AddRange(templist);

                reader.Close();
            }
        }
        /// <summary>
        /// 文件名中包含相应的格式信息
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>默认返回EndNote格式</returns>
        private string GetFileFomat(string filename)
        {
            if (filename.EndsWith(".ciw"))
                return "endnote";
            if (filename.Contains("cscd"))
            {
                return "cscd";
            }
            if (filename.Contains("medline"))
            {
                return "endnote";
            }
            if (filename.Contains("scie"))
            {
                return "endnote";
            }
            if (filename.Contains("cpci"))
            {
                return "endnote";
            }
            if (filename.Contains("ssci"))
            {
                return "endnote";
            }
            if (filename.Contains("ei"))
            {
                return "ei";
            }
            if (filename.Contains("cssci"))
            {
                return "cssci";
            }
            return "endnote";
        }
        private IReader GetFileReader(string fileformat)
        {
            switch (fileformat)
            {
                case "ei":
                    return new EIReader();
                case "endnote":
                    return new EndNoteReader();
                case "cscd":
                    return new CSCDReader();
                case "cssci":
                default:
                    return new CSSCIReader();
            }
        }
        /// <summary>
        /// 结果匹配
        /// </summary>
        public void MatchPaperList()
        {
            paperlist = ireader.MathPaperList(this.paperlist, this.Match.UniversityInstituteList);
        }
        public override string ToString()
        {
            StringBuilder fulltext = new StringBuilder();
            fulltext.Append(CopyRight);
            foreach (var paper in paperlist)
            {
                fulltext.Append(paper.FullText);
            }
            return fulltext.ToString();
        }
    }
    /// <summary>
    /// 记录，包括机构字段内容和全记录内容
    /// </summary>
    public class Paper
    {
        /// <summary>
        /// 机构字段，多条机构使用分号;隔开
        /// </summary>
        public List<string> Institutes { set; get; }
        /// <summary>
        /// 完整的文章内容
        /// </summary>
        public string FullText { set; get; }
    }
    /// <summary>
    /// EndNote格式的数据读取
    /// </summary>
    public class Reader
    {
        public Reader() { }
    }
    public class EndNoteReader : Reader, IReader
    {
        public EndNoteReader() { }

        public List<Paper> Read(StreamReader reader)
        {
            List<Paper> paperlist = new List<Paper>();
            string line = string.Empty;
            bool mark = false;
            StringBuilder content = new StringBuilder();
            List<string> institutes = new List<string>();
            while ((line = reader.ReadLine()) != null)
            {
                content.AppendLine(line);
                if (line.StartsWith("C1") || (line.StartsWith("  ") && mark))
                {
                    mark = true;
                    int start = line.IndexOf(']');
                    institutes.Add(line.Substring(start + 1));
                }
                else
                {
                    mark = false;
                }
                if (line.StartsWith("ER"))
                {
                    Paper paper = new Paper();
                    paper.Institutes = new List<string>();
                    institutes.ForEach(p =>
                    {
                        paper.Institutes.Add(p);
                    });
                    paper.FullText = content.ToString();
                    paperlist.Add(paper);

                    institutes.Clear();
                    content.Clear();
                }
            }
            return paperlist;

        }


        public string ReadCopyRight(StreamReader reader)
        {
            return string.Format("{0}\r\n{1}\r\n", reader.ReadLine(), reader.ReadLine());
        }


        public List<Paper> MathPaperList(List<Paper> paperlist, List<UniversityInsituties> univinstiList)
        {

            //var list = from p in paperlist where MatchPaper(p.Institutes, univinstiList) select p;
            // return list.ToList<Paper>();
            List<Paper> paperlist1 = new List<Paper>();
            for (int index = 0; index < paperlist.Count; index++)
            {
                if (MatchPaper(paperlist[index].Institutes, univinstiList))
                    paperlist1.Add(paperlist[index]);
            }
            return paperlist1;
        }
        Regex regChinse = new Regex("[\u4e00-\u9fa5]");
        private bool MatchPaper(List<string> institutes, List<UniversityInsituties> univinstiList)
        {
            foreach (var institute in institutes)
            {
                foreach (var univinsti in univinstiList)
                {
                    //中英文分开
                    if (regChinse.IsMatch(institute))
                    {
                        //Chinese
                        //if (institute.ToLower().Contains(univinsti.Name.ToLower()))
                        //{
                        //    foreach (var insti in univinsti.Insitutes)
                        //    {
                        //        if (institute.ToLower().Contains(insti.ToLower()))
                        //            return true;
                        //    }
                        //}
                        foreach (var insti in univinsti.Insitutes)
                        {
                            string temp = univinsti.Name + insti;
                            if (institute.Contains(temp))
                                return true;
                        }
                    }
                    else
                    {
                        //English
                        if (institute.ToLower().EndsWith(univinsti.Name.ToLower()) || institute.ToLower().Contains(univinsti.Name.ToLower() + ","))
                        {
                            foreach (var insti in univinsti.Insitutes)
                            {
                                if (institute.ToLower().Contains(insti.ToLower()))
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// CSSCI格式的数据读取
    /// </summary>
    public class CSSCIReader : Reader, IReader
    {
        public string ReadCopyRight(StreamReader reader)
        {
            return string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n", reader.ReadLine(), reader.ReadLine(), reader.ReadLine(), reader.ReadLine());
        }
        public List<Paper> Read(StreamReader reader)
        {
            List<Paper> paperlist = new List<Paper>();
            string line = string.Empty;
            string institute = string.Empty;
            StringBuilder content = new StringBuilder();
            string prefix = "【机构名称】";
            while ((line = reader.ReadLine()) != null)
            {
                content.AppendLine(line);
                if (line.StartsWith(prefix))
                {
                    institute = line.Replace(prefix, "");
                }
                if (line.StartsWith("---"))
                {
                    paperlist.Add(new Paper() { Institutes = institute.Split('/').ToList<string>(), FullText = content.ToString() });
                    content.Clear();
                    institute = string.Empty;
                }
            }
            return paperlist;
        }


        public List<Paper> MathPaperList(List<Paper> paperlist, List<UniversityInsituties> univinstiList)
        {
            var list = from p in paperlist where Match(p.Institutes, univinstiList) select p;
            return list.ToList<Paper>();
        }
        private bool Match(List<string> institutes, List<UniversityInsituties> univinstiList)
        {
            foreach (var institute in institutes)
            {
                foreach (var univinsti in univinstiList)
                {
                    //if (institute.ToLower().Contains(univinsti.Name.ToLower()))
                    //{
                    //    foreach (var insti in univinsti.Insitutes)
                    //    {
                    //        if (institute.ToLower().Contains(insti.ToLower()))
                    //            return true;
                    //    }
                    //}

                    foreach (var insti in univinsti.Insitutes)
                    {
                        string temp = univinsti.Name + insti;
                        if (institute.Contains(temp))
                            return true;
                    }
                }
            }
            return false;
        }
    }
    public class CSCDReader : Reader, IReader
    {

        public List<Paper> Read(StreamReader reader)
        {
            List<Paper> paperlist = new List<Paper>();
            string line = string.Empty;
            string institute = string.Empty;
            StringBuilder content = new StringBuilder();
            string prefix = "单位：";
            while ((line = reader.ReadLine()) != null)
            {
                content.AppendLine(line);
                if (line.StartsWith(prefix))
                {
                    institute = line.Replace(prefix, "");
                }
                if (line.StartsWith("被引频次："))
                {
                    paperlist.Add(new Paper() { Institutes = institute.Split(';').ToList<string>(), FullText = content.ToString() });
                    content.Clear();
                    institute = string.Empty;
                }
            }
            return paperlist;
        }

        public string ReadCopyRight(StreamReader reader)
        {
            return null;
        }

        public List<Paper> MathPaperList(List<Paper> paperlist, List<UniversityInsituties> univinstiList)
        {
            var list = from p in paperlist where Match(p.Institutes, univinstiList) select p;
            return list.ToList<Paper>();
        }
        private bool Match(List<string> institutes, List<UniversityInsituties> univinstiList)
        {
            foreach (var institute in institutes)
            {
                foreach (var univinsti in univinstiList)
                {
                    //if (institute.ToLower().Contains(univinsti.Name.ToLower()))
                    //{
                    //    foreach (var insti in univinsti.Insitutes)
                    //    {
                    //        if (institute.ToLower().Contains(insti.ToLower()))
                    //            return true;
                    //    }
                    //}
                    foreach (var insti in univinsti.Insitutes)
                    {
                        string temp = univinsti.Name + insti;
                        if (institute.Contains(temp))
                            return true;
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// EI格式的数据读取
    /// </summary>
    public class EIReader : Reader, IReader
    {

        public List<Paper> Read(StreamReader reader)
        {
            List<Paper> paperlist = new List<Paper>();
            string line = string.Empty;
            string institute = string.Empty;
            string prefix = "Author affiliation:";
            StringBuilder content = new StringBuilder();
            while ((line = reader.ReadLine()) != null)
            {
                content.AppendLine(line);
                if (line.StartsWith(prefix))
                {
                    institute = line.Replace(prefix, "");
                }
                if (line.Length == 0 && content.Length > 50)
                {
                    //int start = 0;
                    //int end = 0;
                    //while ((start = institute.IndexOf('(')) != -1 && (end = institute.IndexOf(')')) != -1)
                    //{
                    //    institute = institute.Remove(start, end - start + 1);
                    //}
                    Regex regx = new Regex(@"\([0-9]+\)");
                    institute = regx.Replace(institute, "");
                    paperlist.Add(new Paper() { Institutes = institute.Split(';').ToList<string>(), FullText = content.ToString() });
                    content.Clear();
                    institute = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(institute))
            {
                //int start = 0;
                //int end = 0;
                //while ((start = institute.IndexOf('(')) != -1 && (end = institute.IndexOf(')')) != -1)
                //{
                //    institute = institute.Remove(start, end - start + 1);
                //}
                Regex regx = new Regex(@"\([0-9]+\)");
                institute = regx.Replace(institute, "");
                paperlist.Add(new Paper() { Institutes = institute.Split(';').ToList<string>(), FullText = content.ToString() });
                content.Clear();
                institute = string.Empty;
            }
            return paperlist;
        }


        public string ReadCopyRight(StreamReader reader)
        {
            return null;
        }





        public List<Paper> MathPaperList(List<Paper> paperlist, List<UniversityInsituties> univinstiList)
        {
            var list = from p in paperlist where Match(p.Institutes, univinstiList) select p;
            return list.ToList<Paper>();
        }
        private bool Match(List<string> institutes, List<UniversityInsituties> univinstiList)
        {
            //全部小写处理
            foreach (var institute in institutes)
            {
                foreach (var univinsti in univinstiList)
                {
                    if (institute.ToLower().EndsWith(univinsti.Name.ToLower()) || institute.ToLower().Contains(univinsti.Name.ToLower() + ","))
                    {
                        foreach (var insti in univinsti.Insitutes)
                        {
                            if (institute.ToLower().Contains(insti.ToLower()))
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
    public interface IReader
    {
        List<Paper> Read(StreamReader reader);
        string ReadCopyRight(StreamReader reader);
        List<Paper> MathPaperList(List<Paper> paperlist, List<UniversityInsituties> univinstiList);
    }
}
