using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
namespace FilterData.Code
{
    [Serializable]
    public class CSCD2Filter : Filter, IFilter
    {
        Regex reg = new Regex(@"([\u4e00-\u9fa5]*?学校)|([\u4e00-\u9fa5]*?大学)|([\u4e00-\u9fa5]*?学院)|([\u4e00-\u9fa5]*?科学院)|([\u4e00-\u9fa5]*?科院)|([\u4e00-\u9fa5]*?公司|([\u4e00-\u9fa5]*?厂)|([\u4e00-\u9fa5]*?集团))");

        public List<string> GetInstituteFields(string filename)
        {
            List<string> field = new List<string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line = string.Empty;
                string prefix = "单位：";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(prefix))
                    {
                        line = line.Substring(prefix.Length).Trim();
                        field.AddRange(GetField(line));
                    }
                }
                reader.Close();
            }
            return field;
        }
        public List<string> GetField(string line)
        {
            string[] lines = line.Split(';');
            List<string> result = new List<string>();
            for (int index = 0; index < lines.Length; index++)
            {
                result.Add(lines[index].Trim());
            }
            return result;
        }

        public Dictionary<string, int> GetParentInstitutes(List<string> fields)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            Dictionary<string, string> distinct = new Dictionary<string, string>();
            foreach (var line in fields)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string key = GetParentInsitute(line);
                    //此处用于处理一级机构去重问题
                    if (distinct.ContainsKey(key.ToLower()))
                    {
                        key = distinct[key.ToLower()];
                    }
                    else
                    {
                        distinct[key.ToLower()] = key;
                    }
                    if (result.ContainsKey(key))
                    {
                        result[key] += 1;
                    }
                    else
                    {
                        result[key] = 1;
                    }
                }
            } return result;
        }
        public string GetParentInsitute(string line)
        {
            #region
            if (base.isChinese(line))
            {
                //匹配
                Match match = reg.Match(line);
                if (match.Success)
                {
                    line = match.Value;
                }
            }
            else
            {
                //英语，直接提取最后一个字段
                int index = line.LastIndexOf(',');
                if (index != -1)
                {
                    line = line.Substring(index + 1).Trim();
                }
            }
            #endregion
            return line;
        }

        public System.Data.DataTable GetInstitutes(List<string> fields)
        {
            DataTable table = InstituteDataTable.Create();

            foreach (var field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    DataRow dr = table.NewRow();
                    string parentInstitute = GetParentInsitute(field);
                    string institute = field.Replace(parentInstitute, "").Trim();
                    dr[0] = parentInstitute;
                    dr[1] = base.RemoveComma(institute).Trim();
                    table.Rows.Add(dr);
                }
            }
            return table;
        }
    }
}
