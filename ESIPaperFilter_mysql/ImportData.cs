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
using System.Text.RegularExpressions;
using ESIPaperFilter.Code;
using MySql.Data.MySqlClient;
using System.IO;
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
            string sql = "select count(*) from MasteJournalList where year=" + year + " and month=" + month;
            int count = int.Parse(MySqlHelper.ExecuteScalar(Util.connectionString, sql).ToString());
            if (count > 0)
            {
                DialogResult result = MessageBox.Show(string.Format("已经存在{0}年{1}月的数据，无法导入数据。是否删除已有数据后再导入？", year, month));
                if (result == DialogResult.OK)
                {
                    //删除数据
                    string _sql = string.Format("delete from MasteJournalList where year={0} and month={1}", year, month);
                    MySqlHelper.ExecuteScalar(Util.connectionString, _sql);
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
            string filename = Save2File(journallist);
            using (MySqlConnection connection = new MySqlConnection(Util.connectionString))
            {
                connection.Open();
                //StringBuilder sqls = new StringBuilder();
                //foreach (var journal in journallist)
                //{
                //    sqls.AppendLine(string.Format("insert into MasteJournalList (fulltitle,title29,title20,issn,eissn,category,year,month) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",{6},{7});", journal.fulltitle, journal.title29, journal.title20, journal.issn, journal.eissn, journal.year, journal.month, journal.category));
                //}
                //MySqlHelper.ExecuteNonQuery(Util.connectionString, sqls.ToString(), null);
                MySqlBulkLoader copy = new MySqlBulkLoader(connection);
                copy.TableName = "MasteJournalList";
                copy.Columns.AddRange(new List<string>() { "fulltitle", "title29", "title20", "issn", "eissn", "year", "month", "category" });

                copy.LineTerminator = "\r\n";
                copy.FieldTerminator = "~";
                copy.FieldQuotationCharacter = '"';
                copy.FileName = filename;

                copy.Load();
                File.Delete(filename);
            }
        }
        private string Save2File(List<Journal> journals)
        {
            string filename = Guid.NewGuid().ToString() + ".txt";
            StringBuilder content = new StringBuilder();
            foreach (var journal in journals)
            {
                content.AppendFormat("\"{0}\"~\"{1}\"~\"{2}\"~\"{3}\"~\"{4}\"~{5}~{6}~\"{7}\"\r\n", journal.fulltitle, journal.title29, journal.title20, journal.issn, journal.eissn, journal.year, journal.month, journal.category);
            }
            File.WriteAllText(filename, content.ToString());
            return filename;

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
