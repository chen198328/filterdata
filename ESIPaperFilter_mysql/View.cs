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
namespace ESIPaperFilter
{
    public partial class View : Form
    {
        public List<Paper> PaperList = new List<Paper>();
        public View()
        {
            InitializeComponent();
        }

        private void View_Load(object sender, EventArgs e)
        {
            var years = (from p in PaperList select p.PY).Distinct<int>();
            cbxYears.Items.Clear();
            cbxYears.Items.Add("全部");
            foreach (var year in years)
            {
                cbxYears.Items.Add(year);
            }
            cbxYears.SelectedIndex = 0;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            List<Paper> paperlist = PaperList;
            if (cbxYears.SelectedItem.ToString() != "全部")
            {
                int year = 0;
                int.TryParse(cbxYears.SelectedItem.ToString(), out year);

                paperlist = paperlist.FindAll(x => x.PY == year);
            }

            int cite = (int)nudCite.Value;
            paperlist = paperlist.FindAll(x => x.TC >= cite);

            var categorys = paperlist.GroupBy(x => x.Category);
            List<Item> items = new List<Item>();
            foreach (var category in categorys)
            {
                Item item = new Item();
                item.Count = category.Count();
                item.Name = category.Key;
                item.Sum = category.Sum(x => x.TC);
                items.Add(item);
            }
            dataGridView1.DataSource = items;
        }
        public class Item
        {
            public string Name { set; get; }
            public int Count { set; get; }
            public int Sum { set; get; }
        }
    }
}
