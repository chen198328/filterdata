using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;
using CsvHelper;
namespace FilterData.Code
{
    /// <summary>
    /// 规范表
    /// </summary>
    public class NormalizeTable
    {
        /// <summary>
        /// 从DataGridView中导出规范表  
        /// </summary>
        /// <param name="dgvInstituteDataTable"></param>
        /// <returns>第一项为规范表内容，第二项为记录数</returns>
        public static Tuple<string, int> Export1(DataGridView dgvInstituteDataTable)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("\"一级机构\",\"二级机构\",\"学科规范\",\"学院规范\",\"学校规范\"");
            int count = 0;
            foreach (DataGridViewRow row in dgvInstituteDataTable.Rows)
            {
                if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().Trim().Length > 0)
                {
                    string line = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value, row.Cells[4].Value);
                    content.AppendLine(line);
                    count++;
                }
            }
            return new Tuple<string, int>(content.ToString(), count);
        }
        public static List<NormalInstituteRecord> ExportToList(DataGridView dgvInstituteDataTable)
        {
            List<NormalInstituteRecord> list = new List<NormalInstituteRecord>();
            foreach (DataGridViewRow row in dgvInstituteDataTable.Rows)
            {
                if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().Trim().Length > 0)
                {
                    NormalInstituteRecord record = new NormalInstituteRecord();
                    record.一级机构 = row.Cells[0].Value != null ? row.Cells[0].Value.ToString() : null;
                    record.二级机构 = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : null;
                    record.学科规范 = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : null;
                    record.学院规范 = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : null;
                    record.学校规范 = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : null;
                    list.Add(record);
                }
            }
            return list;
        }
        public static List<NormalInstituteRecord> ExportToList(DataTable table, bool all = false)
        {
            List<NormalInstituteRecord> list = new List<NormalInstituteRecord>();
            if (all)
            {
                foreach (DataRow row in table.Rows)
                {
                    NormalInstituteRecord record = new NormalInstituteRecord();
                    record.一级机构 = row["一级机构"] != null ? row["一级机构"].ToString() : null;
                    record.二级机构 = row["二级机构"] != null ? row["二级机构"].ToString() : null;
                    record.学科规范 = row["学科规范"] != null ? row["学科规范"].ToString() : null;
                    record.学院规范 = row["学院规范"] != null ? row["学院规范"].ToString() : null;
                    record.学校规范 = row["学校规范"] != null ? row["学校规范"].ToString() : null;
                    list.Add(record);
                }
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row["学科规范"] != null && row["学科规范"].ToString().Trim().Length > 0)
                    {
                        NormalInstituteRecord record = new NormalInstituteRecord();
                        record.一级机构 = row["一级机构"] != null ? row["一级机构"].ToString() : null;
                        record.二级机构 = row["二级机构"] != null ? row["二级机构"].ToString() : null;
                        record.学科规范 = row["学科规范"] != null ? row["学科规范"].ToString() : null;
                        record.学院规范 = row["学院规范"] != null ? row["学院规范"].ToString() : null;
                        record.学校规范 = row["学校规范"] != null ? row["学校规范"].ToString() : null;
                        list.Add(record);
                    }
                }
            }
            return list;
        }
        public static int Export(List<NormalInstituteRecord> list, string filename)
        {
            if (list == null || list.Count == 0) return 0;
            try
            {

                using (StreamWriter writer = new StreamWriter(filename, false, Encoding.Default))
                {
                    var csv = new CsvWriter(writer);
                    csv.WriteRecords(list);
                    return list.Count;
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 从csv文件中读取规范表
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static DataTable Import1(string filename)
        {

            DataTable NormTable = InstituteDataTable.Create();
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split(new string[] { "\",\"", null }, StringSplitOptions.None);
                    if (fields.Length > 4)
                    {
                        DataRow dr = NormTable.NewRow();
                        string university = fields[0].TrimStart('"');
                        dr[0] = university;
                        dr[1] = fields[1];
                        dr[2] = fields[2];
                        dr[3] = fields[3];
                        dr[4] = fields[4].TrimEnd('"'); ;
                        NormTable.Rows.Add(dr);
                    }
                }
                reader.Close();
            }
            return NormTable;
        }
        public static List<NormalInstituteRecord> ImportToList(string filename)
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.Default))
            {
                var csv = new CsvReader(reader);
                List<NormalInstituteRecord> list = csv.GetRecords<NormalInstituteRecord>().ToList<NormalInstituteRecord>();
                return list;
            }
        }
        public static DataTable Import(string filename)
        {
            DataTable table = InstituteDataTable.Create();
            List<NormalInstituteRecord> list = ImportToList(filename);

            if (list == null || list.Count == 0)
                return table;
            foreach (var item in list)
            {
                DataRow dr = table.NewRow();
                dr[0] = item.一级机构;
                dr[1] = item.二级机构;
                dr[2] = item.学科规范;
                dr[3] = item.学院规范;
                dr[4] = item.学校规范;
                table.Rows.Add(dr);
            }
            return table;
        }
    }
}
