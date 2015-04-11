using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolkit.Csv;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ESIPaperFilter.Code;
using DotNet.Utilities;
namespace ESIPaperFilter
{
    public partial class ImportData : Form
    {
        public ImportData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "CSV(*.csv)|*.csv"; ;
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            txtFilename.Text = openFileDialog.FileName;
        }

        private void lblmessage_Click(object sender, EventArgs e)
        {
            string filename = txtFilename.Text.Trim();
            if (filename.Length == 0)
            {
                MessageBox.Show("请输入文件名");
                return;
            }
            UseWaitCursor = true;
            try
            {
                string[][] items = CsvParser.ParseFile(',', filename);
                DataTable table = new DataTable();
                if (items.Length > 0)
                {
                    //表头
                    string[] heads = items[0];
                    for (int index = 0; index < heads.Length; index++)
                    {
                        table.Columns.Add(new DataColumn(heads[index], typeof(string)));
                    }
                    if (items.Length > 1)
                    {
                        for (int index = 1; index < items.Length; index++)
                        {
                            DataRow row = table.NewRow();
                            for (int colindex = 0; colindex < items[index].Length; colindex++)
                            {
                                if (items[index][colindex].ToLower() != "null")
                                {
                                    string item = items[index][colindex];

                                    if ((items[0][colindex].ToLower().Contains("issn") || items[0][colindex].ToLower().Contains("eissn")) && !IsISSN(item.Trim()))
                                    {
                                        // item = item.Insert(4, "-");
                                        item = "_" + item;
                                    }
                                    row[colindex] = item;
                                }

                            }
                            table.Rows.Add(row);
                        }
                    }
                    dataGridView1.DataSource = table;
                    lblMessage.Text = (items.Length - 1).ToString() + "记录";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取错误：" + ex.Message.ToString());
            }
            UseWaitCursor = false;
        }
        Regex reg = new Regex(@"[0-9]{4}-[0-9]{3,4}x{0,1}");
        private bool IsISSN(string issn)
        {
            return reg.IsMatch(issn.ToLower());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("数据表格中没有数据，请读取");
                return;
            }
            UseWaitCursor = true;
            int year = (int)nudYear.Value;
            int month = (int)nudMonth.Value;
            string sql = "select count(*) from [mastejournallist] where year=" + year + " and month=" + month;
            int count = (int)SqlHelper.ExecuteScalar(CommandType.Text, sql);
            if (count > 0)
            {
                DialogResult result = MessageBox.Show(string.Format("已经存在{0}年{1}月的数据，无法导入数据。是否删除已有数据后再导入？", year, month));
                if (result == DialogResult.OK)
                {
                    //删除数据
                    //EntityList<MasteJournalList> journalist = MasteJournalList.FindAll(new string[] { "year", "month" }, new object[] { year, month });
                    //journalist.Delete();
                    string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ESIPaper"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format("delete from [MasteJournalList] where [year]={0} and [month]={1}", year, month);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    return;
                }
            }
            List<Journal> journallist = new List<Journal>();
            Dictionary<string, int> head = new Dictionary<string, int>();
            for (int index = 0; index < dataGridView1.Columns.Count; index++)
            {
                head[dataGridView1.Columns[index].Name.ToLower()] = index;
            }
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                Journal journal = new Journal();
                journal.fulltitle = dataGridView1.Rows[index].Cells[head["full title"]].Value as string;
                journal.title29 = dataGridView1.Rows[index].Cells[head["title29"]].Value as string;
                journal.issn = dataGridView1.Rows[index].Cells[head["issn"]].Value as string;
                journal.eissn = dataGridView1.Rows[index].Cells[head["eissn"]].Value as string;
                journal.title20 = dataGridView1.Rows[index].Cells[head["title20"]].Value as string;
                journal.category = dataGridView1.Rows[index].Cells[head["category name"]].Value as string;
                journal.year = year;
                journal.month = month;
                journallist.Add(journal);
            }
            Save(journallist);
            UseWaitCursor = false;
            MessageBox.Show("导入成功:" + journallist.Count.ToString());
        }
        /// <summary>
        /// SqlBulkCopy数据
        /// </summary>
        /// <param name="journallist"></param>
        private void Save(List<Journal> journallist)
        {
            string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ESIPaper"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlBulkCopy copy = new SqlBulkCopy(connection);
                copy.DestinationTableName = "MasteJournalList";
                copy.ColumnMappings.Add("fulltitle", "fulltitle");
                copy.ColumnMappings.Add("title29", "title29");
                copy.ColumnMappings.Add("title20", "title20");
                copy.ColumnMappings.Add("issn", "issn");
                copy.ColumnMappings.Add("eissn", "eissn");
                copy.ColumnMappings.Add("year", "year");
                copy.ColumnMappings.Add("month", "month");
                copy.ColumnMappings.Add("category", "category");
                copy.WriteToServer(GetDataTable(journallist));
            }
        }
        private DataTable GetDataTable(List<Journal> journallist)
        {
            DataTable table = GetDataTableStructure();
            journallist.ForEach(p =>
            {
                DataRow row = table.NewRow();
                row["fulltitle"] = p.fulltitle;
                row["title20"] = p.title20;
                row["title29"] = p.title29;
                row["issn"] = p.issn;
                row["eissn"] = p.eissn;
                row["category"] = p.category;
                row["year"] = p.year;
                row["month"] = p.month;
                table.Rows.Add(row);
            });
            return table;
        }
        private DataTable GetDataTableStructure()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("fulltitle", typeof(string)));
            table.Columns.Add(new DataColumn("title29", typeof(string)));
            table.Columns.Add(new DataColumn("title20", typeof(string)));
            table.Columns.Add(new DataColumn("issn", typeof(string)));
            table.Columns.Add(new DataColumn("eissn", typeof(string)));
            table.Columns.Add(new DataColumn("category", typeof(string)));
            table.Columns.Add(new DataColumn("year", typeof(string)));
            table.Columns.Add(new DataColumn("month", typeof(string)));

            return table;
        }
    }
}
