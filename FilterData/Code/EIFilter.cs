using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
namespace FilterData.Code
{
    [Serializable]
    public class EIFilter : Filter, IFilter
    {
        /// <summary>
        /// 提取机构
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<string> GetInstituteFields(string filename)
        {
            string prefix = "Author affiliation";
            if (string.IsNullOrEmpty(filename)) return null;

            List<string> field = new List<string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(prefix))
                    {
                        line = line.Substring(prefix.Length + 1).Trim();
                        field.AddRange(GetField(line));
                    }
                }
                reader.Close();
            }
            return field;
        }
        private List<string> GetField(string line)
        {
            string[] lines = line.Split(';');
            int pos = 0;
            List<string> result = new List<string>();
            for (int index = 0; index < lines.Length; index++)
            {
                pos = lines[index].IndexOf(')');
                if (pos == -1)
                {
                    continue;
                }
                else
                {
                    result.Add(lines[index].Substring(pos + 1).Trim());
                }
            }
            return result;
        }
        public Dictionary<string, int> GetParentInstitutes(List<string> fields)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    string parentInstitute = GetParentInstitute(field);
                    if (result.ContainsKey(parentInstitute))
                    {
                        result[parentInstitute] += 1;
                    }
                    else
                    {
                        result[parentInstitute] = 1;
                    }
                }
            }
            return result;

        }
        /// <summary>
        /// 获取一级机构
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string GetParentInstitute(string line)
        {

            string[] lines = line.Split(',');
            if (lines.Length < 2) { return line; }
            else
            {
                int pos = 0;
                for (int index = 1; index < lines.Length; index++)
                {
                    string temp = lines[index].ToLower();
                    if (base.IsContainsParentInstitute(temp))
                    {
                        continue;
                    }
                    else
                    {
                        pos = index - 1;
                        break;
                    }
                }
                if (lines[pos].ToLower().Trim().StartsWith("ltd"))
                {
                    return lines[pos - 1] + ", " + lines[pos];
                }
                else
                {
                    return lines[pos].Trim();
                }
            }
        }
        /// <summary>
        /// 获取二级机构
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public System.Data.DataTable GetInstitutes(List<string> fields)
        {
            DataTable result = base.CreateInstituteDataTable();

            foreach (var item in fields)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    DataRow row = result.NewRow();
                    string parentInstitute = GetParentInstitute(item);
                    int index = item.IndexOf(parentInstitute);
                    if (index > 0)
                    {
                        string institutes = item.Substring(0, index).Trim();
                        if (institutes.EndsWith(","))
                        {
                            institutes = institutes.Substring(0, institutes.Length - 1);
                        }
                        row[0] = parentInstitute;
                        row[1] = institutes.Trim();
                    }
                    else
                    {
                        row[0] = parentInstitute;
                    }
                    result.Rows.Add(row);
                }
            }
            return result;
        }

        //public System.Data.DataTable SelectInstitutes(System.Data.DataTable institutes, List<string> parentInstitues)
        //{
        //    return base.SelectInstitutes(institutes, parentInstitues);
        //}
    }
}
