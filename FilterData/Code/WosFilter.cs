using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;
namespace FilterData.Code
{
    [Serializable]
    public class WosFilter : EndNoteFilter, IFilter
    {
        /// <summary>
        /// 提取机构字段，去掉作者信息
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<string> GetInstituteFields(string filename)
        {
            List<string> fields = base.GetInstiuteFields(filename);
            return base.RemoveAuthors(fields);
        }

        public Dictionary<string, int> GetParentInstitutes(List<string> fields)
        {
            Dictionary<string, int> Universities = new Dictionary<string, int>();
            Dictionary<string, string> distinct = new Dictionary<string, string>();
            foreach (var field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    int index = field.IndexOf(',');
                    string key = string.Empty;
                    if (index != -1)
                    {
                        key = field.Substring(0, index).Trim();
                    }
                    else { key = field; }

                    //此处用于处理一级机构去重问题
                    if (distinct.ContainsKey(key.ToLower()))
                    {
                        key = distinct[key.ToLower()];
                    }
                    else
                    {
                        distinct[key.ToLower()] = key;
                    }
                    if (Universities.ContainsKey(key))
                    {
                        Universities[key] += 1;
                    }
                    else
                    {
                        Universities[key] = 1;
                    }
                }
            }
            return Universities;
        }

        public System.Data.DataTable GetInstitutes(List<string> fields)
        {
            DataTable result = InstituteDataTable.Create();

            foreach (var item in fields)
            {
                DataRow row = result.NewRow();
                string field = CheckSchool(item);
                if (!string.IsNullOrEmpty(item))
                {
                    int index = field.IndexOf(',');
                    if (index != -1)
                    {
                        row["一级机构"] = field.Substring(0, index);
                        row["二级机构"] = field.Substring(index + 1);
                    }
                    else
                    {
                        row["一级机构"] = field;
                        row["二级机构"] = string.Empty;
                    }
                    result.Rows.Add(row);
                }
            }
            return result;
        }

        public override System.Data.DataTable SelectInstitutes(System.Data.DataTable institutes, List<string> parentInstitues)
        {
            DataTable result = institutes.Clone();
            foreach (DataRow row in institutes.Rows)
            {
                string parentInstitute = row[0].ToString();
                if (StringUtil.EqualIgnoreCase(parentInstitues, parentInstitute))
                {
                    result.Rows.Add(row.ItemArray);
                }
            }
            return result;
        }
        static Regex regPostCode = new Regex("[0-9]+");
        /// <summary>
        /// 将字符串截断到包含数字的位置
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static string CheckSchool(string line)
        {
            int start = line.IndexOf(',');
            if (start != -1)
            {
                start = line.IndexOf(',', start);
                if (start != -1)
                {
                    Match match = regPostCode.Match(line, start);
                    if (match.Success)
                    {
                        int end = line.IndexOf(match.Value, start);
                        end = line.LastIndexOf(',', end);
                        line = line.Substring(0, end);
                        return line;
                    }
                    else
                    {
                        return line;
                    }
                }
                else
                {
                    return line;
                }

            }
            else
            {
                return line;
            }

        }
    }
}
