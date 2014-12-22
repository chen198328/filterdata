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
    public partial class HistoryList : Form
    {
        public HistoryList()
        {
            InitializeComponent();
        }

        private void HistoryList_Load(object sender, EventArgs e)
        {
            string filename = ConfigurationManager.AppSettings["historyofremove"];
            if (!File.Exists(filename))
            {
                DialogResult result = MessageBox.Show("不存在历史删除的文件");
                return;
            }
            using (StreamReader reader = new StreamReader(filename))
            {
                string content = reader.ReadToEnd();
                List<string> list = JsonConvert.DeserializeObject<List<string>>(content);

                dataGridView1.DataSource = GetDataTable(list);
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

    }
}
