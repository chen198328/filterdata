using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
namespace FilterData.Code
{
    public class MedlineFilter : Filter, IFilter
    {
        public List<string> GetInstituteFields(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return null;

            List<string> field = new List<string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("C1"))
                    {
                        line = line.Substring(3);
                        string[] lines = line.Split(';');
                        foreach (var item in lines)
                        {
                            if (item.ToLower().Contains("china"))
                            {
                                field.Add(item.Trim());
                            }
                        }
                    }
                }
                reader.Close();
            }
            return field;
        }

        public Dictionary<string, int> GetParentInstitutes(List<string> fields)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            Dictionary<string, string> distinct = new Dictionary<string, string>();
            foreach (var field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    string key=GetParentInstitute(field);
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
            }
            return result;
        }
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
    }
}
