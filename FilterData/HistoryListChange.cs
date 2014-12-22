using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
namespace FilterData
{
    public partial class HistoryListChange : Form
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        private string Filename { set; get; }
        public HistoryListChange()
        {
            InitializeComponent();
        }

        private void HistoryListChange_Load(object sender, EventArgs e)
        {
            Filename = ConfigurationManager.AppSettings["historyofremove"];
            if (!File.Exists(Filename))
            {
                DialogResult result = MessageBox.Show("系统找不到历史删除的文件,如果有现成的文件，可以在设置文件路径");
                return;
            }
            ReadandBindData(Filename);
            txtFilename.Text = Filename;

        }
        /// <summary>
        /// 读取和绑定数据
        /// </summary>
        /// <param name="filename"></param>
        void ReadandBindData(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string content = reader.ReadToEnd();
                List<string> list = JsonConvert.DeserializeObject<List<string>>(content);

                bindingSource1.DataSource = GetDataTable(list);
                bindingNavigator1.BindingSource = bindingSource1;
                dataGridView1.DataSource = bindingSource1;
            }

        }
        DataTable GetDataTable(List<string> list)
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("字符串", typeof(string));
            table.Columns.Add(column);
            for (int index = 0; index < list.Count; index++)
            {
                DataRow row = table.NewRow();
                row[0] = list[index];
                table.Rows.Add(row);
            }
            return table;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateData();
        }
        void UpdateData()
        {
            using (StreamWriter writer = new StreamWriter(Filename, false))
            {
                List<string> historylist = new List<string>();
                for (int index = 0; index < dataGridView1.Rows.Count; index++)
                {
                    if (dataGridView1.Rows[index].Cells[0].Value.ToString().Trim().Length > 0)
                    {
                        historylist.Add(dataGridView1.Rows[index].Cells[0].Value.ToString());
                    }
                }
                string content = JsonConvert.SerializeObject(historylist);
                writer.Write(content);
                writer.Flush();
                writer.Close();
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateData();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "历史删除文件(*.json)|*.json";
            openFileDialog1.FileName = "removehistory";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                Filename = openFileDialog1.FileName;
                txtFilename.Text = openFileDialog1.FileName;
                ReadandBindData(openFileDialog1.FileName);
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (txtFilename.Text.Trim().Length > 0)
            {
              string key="historyofremove";
              Configuration config=ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
              if (config.AppSettings.Settings[key] != null)
              {
                  config.AppSettings.Settings[key].Value = txtFilename.Text.Trim();

              }
              else
              {
                  config.AppSettings.Settings.Add(key, txtFilename.Text.Trim());
              }
              config.Save(ConfigurationSaveMode.Modified);
              ConfigurationManager.RefreshSection("appSettings");
              ReadandBindData(txtFilename.Text.Trim());
              MessageBox.Show("设置成功");
            }
            else
            {
                MessageBox.Show("请先打开文件路径");
            }
        }
    }
}
