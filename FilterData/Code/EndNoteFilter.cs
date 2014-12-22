using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace FilterData.Code
{
      [Serializable]
    public class EndNoteFilter : Filter
    {
        
        public EndNoteFilter() { }
        /// <summary>
        /// 从C1字段提取机构
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<string> GetInstiuteFields(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return null;

            List<string> field = new List<string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line = string.Empty;
                string prefix = string.Empty;
                bool mark = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 2)
                    {

                        prefix = line.Substring(0, 2);
                        if (prefix == "C1" || (prefix == "  " && mark))
                        {
                            field.Add(line.Substring(2).Trim());
                            mark = true;
                        }
                        else
                        {
                            mark = false;
                        }
                    }
                }
                reader.Close();
            }
            return field;
        }
        /// <summary>
        /// 移除机构字段中的作者信息
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<string> RemoveAuthors(List<string> fields)
        {
            List<string> fieldsWithoutAuthors = new List<string>();
            foreach (var field in fields)
            {
                if (field.IndexOf('[') != -1 && field.IndexOf(']') != -1)
                {
                    string temp = field.Substring(field.IndexOf(']') + 1).Trim();
                    fieldsWithoutAuthors.Add(temp);
                }
                else
                {
                    fieldsWithoutAuthors.Add(field);
                }
            }

            return fieldsWithoutAuthors.Distinct().ToList<string>();
        }
      
    }
}
