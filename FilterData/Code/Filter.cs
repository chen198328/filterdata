using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System;
namespace FilterData.Code
{
    [Serializable]
    public class Filter
    {
        Regex regChinse = new Regex("[\u4e00-\u9fa5]");
        public Filter() { }
        public virtual System.Data.DataTable SelectInstitutes(System.Data.DataTable institutes, List<string> parentInstitues)
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
        /// <summary>
        /// 创建二级机构表
        /// </summary>
        /// <returns></returns>
        public DataTable CreateInstituteDataTable()
        {
            return InstituteDataTable.Create();
        }
        public bool isChinese(string line)
        {
            return regChinse.IsMatch(line);
        }
        public bool IsContainsParentInstitute(string line)
        {
            line = line.ToLower();
            if (line.Contains("univ"))
                return true;
            if (line.Contains("institute"))
                return true;
            if (line.Contains("institute"))
                return true;
            if (line.Contains("college"))
                return true;
            if (line.Contains("department"))
                return true;
            if (line.Contains("dept"))
                return true;
            if (line.Contains("lab"))
                return true;
            if (line.Contains("hospital"))
                return true;
            if (line.Contains("construction"))
                return true;
            return false;
        }
        protected string RemoveComma(string line)
        {
            if (line.StartsWith(","))
            {
                line = line.Substring(1);
            }
            if (line.StartsWith("，"))
            {
                line = line.Substring(1);
            }
            if (line.EndsWith(","))
            {
                line = line.Substring(0, line.Length - 1);
            }
            if (line.EndsWith("，"))
            {
                line = line.Substring(0, line.Length - 1);
            }
            return line;
        }
    }
}
