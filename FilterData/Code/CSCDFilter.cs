﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
namespace FilterData.Code
{
    /// <summary>
    /// 此filter主要用于ciw格式的cscd，现在系统不再使用
    /// </summary>
    [Serializable]
    public class CSCDFilter : EndNoteFilter, IFilter
    {
        //Regex reg = new Regex(@"([\u4e00-\u9fa5]*?学校)|([\u4e00-\u9fa5]*?大学)|([\u4e00-\u9fa5]*?学院)|([\u4e00-\u9fa5]*?科学院)|([\u4e00-\u9fa5]*?科院)|([\u4e00-\u9fa5]*?公司|([\u4e00-\u9fa5]*?厂)|([\u4e00-\u9fa5]*?集团))");
        Regex reg = new Regex(@"([\u4e00-\u9fa5]|\(|\)|\（|\）)*?(学校|大学|学院|科学院|科院|公司|厂|集团)");//中英文括号
        /// <summary>
        /// 获取机构字段（移除作者信息）
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<string> GetInstituteFields(string filename)
        {
            List<string> institutes = base.GetInstiuteFields(filename);
            return base.RemoveAuthors(institutes);
        }
        /// <summary>
        /// 获取一级机构
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
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
            line = line.Trim(',');
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
                    string institute = field.Replace(parentInstitute, "").Trim(',').Trim();
                    dr[0] = parentInstitute;
                    dr[1] = base.RemoveComma(institute);
                    table.Rows.Add(dr);
                }
            }
            return table;
        }
    }
}
