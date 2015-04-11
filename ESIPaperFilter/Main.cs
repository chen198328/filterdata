using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESIPaperFilter.Code;
using System.Data.SqlClient;
using System.IO;
using DotNet.Utilities;
using System.Collections.Specialized;
namespace ESIPaperFilter
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        List<Paper> paperlist = new List<Paper>();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "文本(*.txt)|*.txt|EndNote(*.ciw)|*.ciw"; ;
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            cbbFilenames.Items.Clear();
            for (int index = 0; index < openFileDialog.FileNames.Length; index++)
            {
                cbbFilenames.Items.Add(openFileDialog.FileNames[index]);
            }
            if (cbbFilenames.Items.Count > 0)
            {
                cbbFilenames.SelectedIndex = 0;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileNames.Length == 0)
            {
                MessageBox.Show("请选择要读取的文件");
                return;
            }

            Action<DataTable> bind = (table) =>
            {
                dataGridView.DataSource = table;
            };
            Action<string> updatePaperCount = (message) =>
            {
                lblTotalCount.Text = message;
            };
            Task task = new Task(() =>
            {

                string[] filenames = openFileDialog.FileNames;
                paperlist = GetPapers(filenames);
                DataTable table = GetESIPaperTable(paperlist);
                this.BeginInvoke(bind, table);
                this.BeginInvoke(updatePaperCount, "数据总数：" + paperlist.Count);
            });
            task.Start();
            UseWaitCursor = true;
            task.ContinueWith(t =>
            {
                UseWaitCursor = false;
            });
        }
        private List<Paper> GetPapers(string[] filenames)
        {
            List<Paper> paperlist = new List<Paper>();
            for (int index = 0; index < filenames.Length; index++)
            {
                paperlist.AddRange(Paper.Read(filenames[index]));
            }
            return paperlist;
        }
        private DataTable GetESIPaperTable(List<Paper> paperlist)
        {
            DataTable table = GetTableStructure();
            for (int index = 0; index < paperlist.Count; index++)
            {
                DataRow row = table.NewRow();
                row["Title"] = paperlist[index].Title;
                row["SO"] = paperlist[index].SO;
                row["J9"] = paperlist[index].J9;
                row["SN"] = paperlist[index].SN;
                row["PY"] = paperlist[index].PY;
                row["Z9"] = paperlist[index].Z9;
                row["TC"] = paperlist[index].TC;
                if (paperlist[index].IsMark)
                {
                    row["ESI"] = "Y";
                }
                else
                {
                    row["ESI"] = "N";
                }
                table.Rows.Add(row);

            }
            return table;
        }
        private DataTable GetTableStructure()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Title", typeof(string)));
            table.Columns.Add(new DataColumn("SO", typeof(string)));
            table.Columns.Add(new DataColumn("J9", typeof(string)));
            table.Columns.Add(new DataColumn("SN", typeof(string)));
            table.Columns.Add(new DataColumn("ESI", typeof(string)));
            table.Columns.Add(new DataColumn("TC", typeof(int)));
            table.Columns.Add(new DataColumn("Z9", typeof(int)));
            table.Columns.Add(new DataColumn("PY", typeof(int)));


            return table;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 1)
            {
                MessageBox.Show("数据窗口中没有数据");
                return;
            }
            Action<string> updateESIISSN = (message) =>
            {
                lblmatch.Text = message;

            };
            Action<DataTable> bindData = (table) =>
            {
                dataGridView.DataSource = table;
            };
            int year = (int)nudYear.Value;
            int month = (int)nudMonth.Value;


            NameValueCollection namewlauecollection = QueryISSNandCategory(year, month);

            string category = cbxCategorys.SelectedItem as string;
            Task task = new Task(() =>
            {
                this.BeginInvoke(updateESIISSN, "正在从数据库中加载数据");
                List<Paper> distinctpapers = (from p in paperlist select new Paper() { SN = p.SN, SO = p.SO, J9 = p.J9 }).ToList<Paper>().Distinct(new PaperComparer()).ToList<Paper>();
                StringBuilder sqls = new StringBuilder();

                distinctpapers.ForEach(p =>
                {
                    string sql = string.Empty;

                    sql = string.Format("select ISSN,EISSN,FullTitle,Title20,Title29 from [MasteJournalList] where ([issn]='{0}' or [eissn]='{0}' or [fulltitle]='{1}' or [title29]='{2}' or  [title20]='{2}'", p.SN, p.SO, p.J9);
                    if (!cbxExportAll.Checked)
                    {
                        sql += " and [year]=" + year + " and [month]=" + month;
                    }
                    sql += ")";
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        switch (category)
                        {
                            case "全部":
                                break;
                            default:
                                sql += " and category='" + category + "';";
                                break;
                        }
                    }
                    sqls.AppendLine(sql);
                });
                IDataReader ireader = SqlHelper.ExecuteReader(SqlHelper.connectionString, CommandType.Text, sqls.ToString());
                paperlist.ForEach(p =>
                {
                    p.IsMark = false;
                });
                int rowcount = 0;
                do
                {
                    if (ireader.Read())
                    {
                        string issn = ireader["issn"] as string;
                        string eissn = ireader["eissn"] as string;
                        string fulltitle = ireader["fulltitle"] as string;
                        string title20 = ireader["title20"] as string;
                        string title29 = ireader["title29"] as string;
                        List<Paper> papers = paperlist.FindAll(p => p.SN == issn | p.SN == eissn | p.SO == fulltitle | p.J9 == title20 | p.J9 == title29);
                        papers.ForEach(p =>
                        {
                            p.IsMark = true;
                        });
                        this.BeginInvoke(updateESIISSN, string.Format("ISSN:{0} match  进度{1}%", issn, rowcount++ * 100 / distinctpapers.Count));
                    }
                } while (ireader.NextResult());
                ireader.Close();
                DataTable table = GetESIPaperTable(paperlist);
                this.BeginInvoke(bindData, table);
                int count = paperlist.FindAll(p => p.IsMark == true).Count;
                this.BeginInvoke(updateESIISSN, "最终匹配数据：" + count.ToString());

            });
            UseWaitCursor = true;
            task.Start();
            task.ContinueWith(t =>
            {
                UseWaitCursor = false;
                MessageBox.Show("匹配完成");
            });
        }
        private NameValueCollection QueryISSNandCategory(int year = 0, int month = 0)
        {
            string sql = string.Empty;
            if (year == 0 || month == 0)
            {
                sql = "select ISSN,EISSN,Category from MasteJournalList";
            }
            else
            {
                sql = string.Format("select ISSN,EISSN,Category from MasteJournalList where year={0} and month={1}", year, month);
            }
            IDataReader ireader = SqlHelper.ExecuteReader(SqlHelper.connectionString, CommandType.Text, sql);
            NameValueCollection namevaluecollection = new NameValueCollection();
            while (ireader.Read())
            {
                string issn = ireader[0] as string;
                string eissn = ireader[1] as string;
                string category = ireader[2] as string;
                if (!string.IsNullOrWhiteSpace(issn))
                {
                    namevaluecollection.Set(issn, category);
                }
                if (!string.IsNullOrWhiteSpace(eissn))
                {
                    namevaluecollection.Set(eissn, category);
                }
            }
            return namevaluecollection;


        }
        private string Excute(string sql, SqlConnection connection)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                string result = cmd.ExecuteScalar() as string;
                return result;
            }
            catch (SqlException)
            {
                return null;
            }
        }

        private void btnOpenSaveDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            txtSaveDirectory.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (txtSaveDirectory.Text.Length == 0)
            {
                MessageBox.Show("请选择文件要保存的文件件");
                return;
            }
            if (chbExport.Checked)
            {
                //分文件导出
                UseWaitCursor = true;
                List<string> filenames = (from p in paperlist where p.IsMark select p.Filename).Distinct().ToList();
                filenames.ForEach(f =>
                {
                    List<Paper> papers = (from p in paperlist where p.IsMark && p.Filename == f select p).ToList<Paper>();
                    string filename = Path.Combine(txtSaveDirectory.Text, f.Insert(f.IndexOf('.'), "_" + papers.Count));
                    SaveFiles(filename, papers);
                });
                UseWaitCursor = false;
                MessageBox.Show("保存成功");
            }
            else
            {
                //一个文件导出
                UseWaitCursor = true;
                List<Paper> papers = paperlist.FindAll(p => p.IsMark);
                string filename = Path.Combine(txtSaveDirectory.Text, "esipaper_" + papers.Count.ToString() + ".ciw");

                SaveFiles(filename, papers);
                UseWaitCursor = true;

                UseWaitCursor = false;
                MessageBox.Show("保存成功");
            }
        }
        private void SaveFiles(string filename, List<Paper> paperlist)
        {
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                writer.WriteLine("FN Thomson Reuters Web of Science™");
                writer.WriteLine("VR 1.0");
                paperlist.ForEach(p =>
                {
                    if (p.IsMark)
                    {
                        p.Lines.ForEach(l =>
                        {
                            writer.WriteLine(l);
                        });
                        writer.WriteLine();
                    }
                });
                writer.WriteLine("EF");
                writer.Flush();
                writer.Close();
            }
        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportData importdata = new ImportData();
            importdata.ShowDialog();
        }

        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewData viewdata = new ViewData();
            viewdata.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            cbxCategorys.Items.Clear();
            string sql = "select distinct category from [MasteJournalList]";
            IDataReader ireader = SqlHelper.ExecuteReader(SqlHelper.connectionString, CommandType.Text, sql);
            cbxCategorys.Items.Add("全部");
            while (ireader.Read())
            {
                if (ireader[0] != null)
                {
                    string category = ireader[0].ToString();
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        cbxCategorys.Items.Add(ireader[0].ToString());
                    }
                }

            }
            cbxCategorys.SelectedItem = "全部";
        }
    }
    class PaperComparer : EqualityComparer<Paper>
    {

        public override bool Equals(Paper x, Paper y)
        {
            if (x.SN == y.SN)
                return true;
            else if (x.SO == y.SO)
                return true;
            else if (x.J9 == y.J9)
                return true;
            else
                return false;
        }

        public override int GetHashCode(Paper obj)
        {
            return (obj.SO + obj.SO + obj.J9).GetHashCode();
        }
    }
}
