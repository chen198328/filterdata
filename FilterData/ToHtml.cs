using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilterData.Code;
using System.IO;
namespace FilterData
{
    public partial class ToHtml : Form
    {
        string[] memoryFilenames;
        string html = string.Empty;
        string html_code = string.Empty;
        IConvert iconvert = null;
        public ToHtml()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                OpenFileDialog digalog = (OpenFileDialog)sender;
                StringBuilder filenames = new StringBuilder();
                memoryFilenames = digalog.FileNames;
                SetSystemSelect(digalog.FileNames[0].ToLower());
                foreach (var file in digalog.FileNames)
                {
                    filenames.Append(file + ";");
                }
                txtFileNames.Text = filenames.ToString().TrimEnd(';');
                filenames.Clear();
            }

        }
        private void SetSystemSelect(string text)
        {

            if (text.EndsWith(".ciw"))
            {
                if (text.Contains("cpci"))
                {
                    cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                    return;
                }
                if (text.Contains("scie"))
                {
                    cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                    return;
                }
                if (text.Contains("ssci"))
                {
                    cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                    return;
                }
                if (text.Contains("medline"))
                {
                    cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                    return;
                }
            }
            else if (text.EndsWith(".txt"))
            {
                if (text.Contains("cscd"))
                {
                    cbbSystem.SelectedItem = "CSCD";
                    return;
                }
                if (text.Contains("cssci"))
                {
                    cbbSystem.SelectedItem = "CSSCI";
                    return;
                }

                if (text.Contains("ei"))
                {
                    cbbSystem.SelectedItem = "EI";
                    return;
                }
            }
            else
            {
                cbbSystem.SelectedItem = "";
            }

            if (text.Contains("cscd"))
            {
                cbbSystem.SelectedItem = "CSCD";
                return;
            }
            if (text.Contains("medline"))
            {
                cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                return;
            }
            if (text.Contains("cssci"))
            {
                cbbSystem.SelectedItem = "CSSCI";
                return;
            }
            if (text.Contains("scie"))
            {
                cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                return;
            }
            if (text.Contains("ei"))
            {
                cbbSystem.SelectedItem = "EI";
                return;
            }
            if (text.Contains("cpci"))
            {
                cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                return;
            }
            if (text.Contains("ssci"))
            {
                cbbSystem.SelectedItem = "EndNote(sci-e,ssci,cpci,medline)";
                return;
            }

        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "EndNote(*.ciw)|*.ciw|Text File(*.txt)|*.txt";
            openFileDialog1.ShowDialog();
            txtTitle.Focus();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (memoryFilenames == null || memoryFilenames.Length == 0)
            {
                MessageBox.Show("请选择需要转换文件");
                return;
            }
            if (cbbSystem.SelectedItem == null)
            {
                MessageBox.Show("请选择需要转换文件的格式");
                return;
            }
            if (!string.IsNullOrEmpty(txtDatabase.Text.Trim()) && !string.IsNullOrEmpty(txtTitle.Text.Trim()) && !string.IsNullOrEmpty(txtSearchDate.Text.Trim()) && !string.IsNullOrEmpty(txtTimespan.Text.Trim()) && !string.IsNullOrEmpty(txtSearchCondition.Text.Trim()))
            {

            }
            else
            {
                MessageBox.Show("请输入标题或备注内容");
                return;
            }

            iconvert.Clear();
            foreach (var file in memoryFilenames)
            {
                iconvert.Read(file);
            }

            using (StreamReader reader = new StreamReader("template.html", Encoding.UTF8))
            {
                html = reader.ReadToEnd();
                html_code = GetTemplateCode();
                string title = txtTitle.Text.Trim();
                //string content = iconvert.ToHtml();
                string searchdate = txtSearchDate.Text.Trim();
                string timespan = txtTimespan.Text.Trim();
                string searchcondition = txtSearchCondition.Text.Trim();
                string database = txtDatabase.Text.Trim();
                html = html.Replace("${title}$", title);
                html_code = html_code.Replace("${title}$", title);
                List<string> years = new List<string>();
                if (string.IsNullOrEmpty(txtYear.Text.Trim())) { }
                else if (txtYear.Text.Trim().Contains(';'))
                {
                    foreach (var year in txtYear.Text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        years.Add(year.Trim());
                    }
                }
                else
                {
                    years.Add(txtYear.Text.Trim());
                }
                #region 全部格式
                html = html.Replace("${content}$", iconvert.ToHtml(years));
                html = html.Replace("${searchdate}$", searchdate);
                html = html.Replace("${searchTimeSpan}$", timespan);
                html = html.Replace("${searchCount}$", iconvert.Count().ToString());
                html = html.Replace("${searchCondition}$", searchcondition);
                html = html.Replace("${database}$", database);
                #endregion
                #region code格式
                html_code = html_code.Replace("${content}$", iconvert.ToHtml(years));
                html_code = html_code.Replace("${searchdate}$", searchdate);
                html_code = html_code.Replace("${searchTimeSpan}$", timespan);
                html_code = html_code.Replace("${searchCount}$", iconvert.Count().ToString());
                html_code = html_code.Replace("${searchCondition}$", searchcondition);
                html_code = html_code.Replace("${database}$", database);
                #endregion
                //content=string.Format(html, txtTitle.Text.Trim(), iconvert.ToHtml());
                webBrowser1.DocumentText = html;
                this.Text = title;
                txtRecordCount.Text = iconvert.Count().ToString();
            }
        }
        private string GetTemplateCode()
        {
            using (StreamReader reader = new StreamReader("template_code.html", Encoding.UTF8))
            {
                string content = reader.ReadToEnd();
                reader.Close();
                return content;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(html))
            {
                MessageBox.Show("请先预览生成的网页");
                return;
            }
            else
            {
                saveFileDialog1.Filter = "Html(*.html)|*.html";
                DialogResult dialogresult = saveFileDialog1.ShowDialog();

                if (dialogresult == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8))
                    {
                        writer.Write(html);
                        writer.Flush();
                        writer.Close();
                    }
                    string filename = saveFileDialog1.FileName.Replace(".html", ".code.txt");
                    using (StreamWriter writercode = new StreamWriter(filename, false, Encoding.UTF8))
                    {
                        writercode.Write(html_code);
                        writercode.Flush();
                        writercode.Close();
                    }
                    MessageBox.Show("保存成功！");
                }
            }
        }

        private void cbbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string system = cbbSystem.SelectedItem.ToString();
            switch (system)
            {
                case "EndNote(sci-e,ssci,cpci,medline)":
                    iconvert = new EndNoteConvert();
                    break;
                case "EI":
                    iconvert = new EIConvert();
                    break;
                case "CSSCI":
                    iconvert = new CSSCIConvert();
                    break;
                case "CSCD":
                    iconvert = new CSCDConverter();
                    break;
                default:
                    break;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }
    }
}
