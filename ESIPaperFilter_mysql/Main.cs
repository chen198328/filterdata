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
using MySql.Data.MySqlClient;
using System.IO;
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
                //row["SO"] = paperlist[index].SO;
                //row["J9"] = paperlist[index].J9;
                //row["SN"] = paperlist[index].SN;
                row["PY"] = paperlist[index].PY;
                row["Z9"] = paperlist[index].Z9;
                row["TC"] = paperlist[index].TC;
                row["Category"] = paperlist[index].Category;
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
            //table.Columns.Add(new DataColumn("SO", typeof(string)));
            //table.Columns.Add(new DataColumn("J9", typeof(string)));
            //table.Columns.Add(new DataColumn("SN", typeof(string)));
            table.Columns.Add(new DataColumn("ESI", typeof(string)));
            table.Columns.Add(new DataColumn("Category", typeof(string)));
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
            if (cbxExportAll.Checked)
            {
                year = month = 0;
            }

            string category = cbxCategorys.SelectedItem as string;
            Task task = new Task(() =>
            {
                UseWaitCursor = true;

                this.BeginInvoke(updateESIISSN, "正在从数据库中加载数据");
                NameValueCollection namewlauecollection = QueryISSNandCategory(year, month);
                paperlist.ForEach(p =>
                {
                    p.Category = namewlauecollection[p.SN];
                    if (string.IsNullOrEmpty(p.Category))
                    {
                        p.Category = namewlauecollection[p.SO];
                    }
                    if (string.IsNullOrEmpty(p.Category))
                    {
                        p.Category = namewlauecollection[p.J9];
                    }
                    if (!string.IsNullOrEmpty(p.Category))
                    {
                        p.IsMark = true;
                    }
                    else
                    {
                        p.IsMark = false;
                    }
                });
                DataTable table = GetESIPaperTable(paperlist);
                this.Invoke(bindData, table);
                int count = paperlist.FindAll(p => p.IsMark == true).Count;
                this.BeginInvoke(updateESIISSN, "最终匹配数据：" + count.ToString());
                UseWaitCursor = false;
            });

            task.Start();
            task.ContinueWith(t =>
            {
                MessageBox.Show("匹配完成");
            });
        }
        private void btnImport_Click1(object sender, EventArgs e)
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
                sql = "select distinct ISSN,EISSN,fulltitle,title29,title20,Category from MasteJournalList";
            }
            else
            {
                sql = string.Format("select distinct ISSN,EISSN,fulltitle,title29,title20,Category from MasteJournalList where year={0} and month={1}", year, month);
            }
            IDataReader ireader = MySqlHelper.ExecuteReader(Util.connectionString, sql);
            NameValueCollection namevaluecollection = new NameValueCollection();
            while (ireader.Read())
            {
                string category = ireader[5].ToString();
                for (int index = 0; index < 5; index++)
                {
                    string field = ireader[index].ToString();
                    if (!string.IsNullOrEmpty(field) && !namevaluecollection.AllKeys.Contains(field))
                    {
                        namevaluecollection.Set(field, category);
                    }
                }
            }
            return namevaluecollection;


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
            string categoryname = cbxCategorys.SelectedItem.ToString();
            List<Paper> _paperlist = paperlist.FindAll(x => x.Category != null);
                _paperlist=_paperlist.FindAll(x=> x.Category.ToLower() == categoryname.ToLower());
            if (chbExport.Checked)
            {
                //分文件导出
                UseWaitCursor = true;
                List<string> filenames = (from p in _paperlist where p.IsMark select p.Filename).Distinct().ToList();
                filenames.ForEach(f =>
                {
                    List<Paper> papers = (from p in _paperlist where p.IsMark && p.Filename == f select p).ToList<Paper>();
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
                List<Paper> papers = _paperlist.FindAll(p => p.IsMark);
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
            string sql = "select distinct category from MasteJournalList";
            IDataReader ireader = MySqlHelper.ExecuteReader(Util.connectionString, sql);
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

        private void cbxExportAll_CheckedChanged(object sender, EventArgs e)
        {

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
