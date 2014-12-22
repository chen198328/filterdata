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

namespace FilterData
{
    public partial class UpdateLog : Form
    {
        public UpdateLog()
        {
            InitializeComponent();
        }

        private void 升级日志_Load(object sender, EventArgs e)
        {
            string filename = "log.txt";
            if (!File.Exists(filename))
            {
                richTextBox1.Text = "没有找到系统升级的日志文件";
                return;
            }
            using (StreamReader reader = new StreamReader(filename,Encoding.Default))
            {
                richTextBox1.Text = reader.ReadToEnd();
            }
        }
    }
}
