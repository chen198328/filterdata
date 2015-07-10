using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DotNet.Utilities;
namespace ESIPaperFilter
{
    public partial class ViewData : Form
    {
        public ViewData()
        {
            InitializeComponent();
        }

        private List<string> GetList(string sql)
        {
            IDataReader ireader = SqlHelper.ExecuteReader(SqlHelper.connectionString, CommandType.Text, sql);
            List<string> items = new List<string>();
            while (ireader.Read())
            {
                if (ireader[0] != null)
                {
                    string item = ireader[0].ToString();
                    if (!string.IsNullOrWhiteSpace(item)) { items.Add(item); }
                }

            }
            ireader.Close();
            return items;
        }
        private void ViewData_Load(object sender, EventArgs e)
        {
            List<string> years = GetList("select distinct year from MasteJournalList");
            cbbYear.Items.Clear();
            years.ForEach(p =>
            {
                cbbYear.Items.Add(p);
            });
            if (cbbYear.Items.Count > 0)
                cbbYear.SelectedIndex = 0;
        }

        private void cbbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string year = cbbYear.SelectedItem.ToString();
            string sql = "select distinct month from MasteJournalList where year=" + year;
            List<string> months = GetList(sql);
            cbbMonth.Items.Clear();
            months.ForEach(p =>
            {
                cbbMonth.Items.Add(p);
            });
            if (cbbMonth.Items.Count > 0)
                cbbMonth.SelectedIndex = 0;
        }

        private void cbbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string year = cbbYear.SelectedItem.ToString();
            string month = cbbMonth.SelectedItem.ToString();
            string sql = string.Format("select distinct category from MasteJournalList where year={0} and month={1}", year, month);
            List<string> categorys = GetList(sql);
            cbbCategoryList.Items.Clear();
            cbbCategoryList.Items.Add("全部");
            categorys.ForEach(p =>
            {
                cbbCategoryList.Items.Add(p);
            });
            if (cbbCategoryList.Items.Count > 0)
                cbbCategoryList.SelectedIndex = 0;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string year = cbbYear.SelectedItem as string;
            string month = cbbMonth.SelectedItem as string;
            string category = cbbCategoryList.SelectedItem as string;
            string sql = string.Format("select id,fulltitle,title20,title29,issn,eissn from MasteJournalList where year={0} and month='{1}' ", year, month);
            if (category != "全部")
            {
                sql += " and [category]='" + category + "'";
            }
            DataTable table = SqlHelper.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            dataGridView1.DataSource = table;
            lblTotalCount.Text = table.Rows.Count.ToString();


        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string columnname = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
            string sql = string.Format("update [MasteJournalList] set {0}='{1}' where id={2}", columnname, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), id);
            SqlHelper.ExecteNonQuery(CommandType.Text, sql);

        }
    }
}
