using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilterData.Code;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
namespace FilterData
{
    public partial class FilterInstitute : Form
    {
        public string[] memoryFilenames;
        /// <summary>
        /// 原始二级机构表,用户机构一级机构遴选
        /// </summary>
        public DataTable memoryInstituteDataTable = InstituteDataTable.Create();
        /// <summary>
        /// 二级机构表临时存储，用于一二级机构筛选，去重前后需要与datagridview的数据保持一致
        /// </summary>
        public DataTable tempInstituteDataTable = InstituteDataTable.Create();
        public bool isOpen = false;
        public IFilter ifilter = new WosFilter();
        public string fileformat = string.Empty;
        /// <summary>
        /// 用户保存一级机构数据
        /// </summary>
        public List<UniversityItem> Univerisities = new List<UniversityItem>();
        public FilterInstitute()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "EndNote(*.ciw)|*.ciw|Text File(*.txt)|*.txt";
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                OpenFileDialog digalog = (OpenFileDialog)sender;
                StringBuilder filenames = new StringBuilder();
                SetSystemSelect(digalog.FileNames[0].ToLower());
                memoryFilenames = digalog.FileNames;
                foreach (var file in digalog.FileNames)
                {
                    filenames.Append(file + ";");
                }
                txtFileNames.Text = filenames.ToString().TrimEnd(';');
                filenames.Clear();
                AddLog("打开" + digalog.FileNames.Length + "个文件");
            }

        }
        private void SetSystemSelect(string text)
        {
            if (text.EndsWith(".ciw"))
            {
                if (text.Contains("cpci"))
                {
                    cbbSystem.SelectedItem = "Wos(sci,ssci,cpci)";
                    return;
                }
                if (text.Contains("scie"))
                {
                    cbbSystem.SelectedItem = "Wos(sci,ssci,cpci)";
                    return;
                }
                if (text.Contains("ssci"))
                {
                    cbbSystem.SelectedItem = "Wos(sci,ssci,cpci)";
                    return;
                }
                if (text.Contains("medline"))
                {
                    cbbSystem.SelectedItem = "Medline";
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



        }
        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="content"></param>
        public void AddLog(string content)
        {
            rtbLog.AppendText(DateTime.Now.ToString() + ":" + content + "\r\n");
            rtbLog.ScrollToCaret();
        }
        public void BingUniversity(List<UniversityItem> universities)
        {
            chbUniversities.Items.Clear();
            for (int index = 0; index < universities.Count; index++)
            {
                chbUniversities.Items.Add(universities[index].Name);
                chbUniversities.SetItemChecked(index, universities[index].Checked);
            }
            AddLog("加载一级机构列表：" + universities.Count + "条");
        }
        public void BindInstituteDataTable(DataTable table, string rowfilter = null)
        {
            DataView view = table.DefaultView;
            if (!string.IsNullOrEmpty(rowfilter))
            {
                view.RowFilter = rowfilter;
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = view;
            bindingNavigator1.BindingSource = bs;
            dgvInstituteDataTable.DataSource = bs;
            AddLog("加载二级机构列表：" + table.Rows.Count + "条");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvInstituteDataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// 一级机构遴选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            List<string> selectitems = new List<string>();
            for (int index = 0; index < chbUniversities.Items.Count; index++)
            {
                if (chbUniversities.GetItemChecked(index))
                {
                    selectitems.Add(chbUniversities.Items[index].ToString().Trim());
                }
            }
            DataTable selectTable = ifilter.SelectInstitutes(memoryInstituteDataTable, selectitems);
            tempInstituteDataTable = selectTable;
            BindInstituteDataTable(selectTable);
        }

        private void btnRemoveDistinct_Click(object sender, EventArgs e)
        {
            if (dgvInstituteDataTable.Rows.Count == 0)
            {
                MessageBox.Show("数据为空,无效操作");
                return;
            }
            string remove = txtRemove.Text.Trim();
            if (!string.IsNullOrEmpty(remove))
            {
                AddRemoveHistory(remove);
            }

            List<string> removehistory = new List<string>();
            if (chbRemoveHistory.Checked)
            {
                removehistory = GetRemoveHistory();
            }
            else
            {

                removehistory = new List<string>() { };
                if (!string.IsNullOrEmpty(remove))
                {
                    removehistory.Add(remove);
                }
            }
            //bool isRemove = remove.Length > 0 ? true : false;
            DataTable instituteData = GetDataTableFromGridView(dgvInstituteDataTable);
            List<string> tempList = new List<string>();
            foreach (DataRow row in instituteData.Rows)
            {
                string temp = row[1].ToString().Trim();
                if (removehistory.Count != 0)
                {
                    removehistory.ForEach(p =>
                    {
                        temp = temp.Replace(p, "");
                    });
                }
                tempList.Add(row[0] + "|" + temp + "|" + row[2] + "|" + row[3] + "|" + row[4]);

            }
            //tempList.Distinct().ToList<string>();

            tempList = tempList.Distinct(new Compare()).ToList<string>();
            instituteData.Rows.Clear();

            foreach (var item in tempList)
            {
                DataRow dr = instituteData.NewRow();
                string[] splits = item.Split('|');
                dr[0] = splits[0];
                dr[1] = splits[1];
                dr[2] = splits[2];
                dr[3] = splits[3];
                dr[4] = splits[4];
                instituteData.Rows.Add(dr);
            }
            tempList.Clear();
            tempInstituteDataTable = instituteData;
            BindInstituteDataTable(instituteData);

        }

        /// <summary>
        /// 获取删除的历史
        /// </summary>
        /// <returns></returns>
        private List<string> GetRemoveHistory()
        {
            string filename = System.Configuration.ConfigurationManager.AppSettings["historyofremove"];
            if (!File.Exists(filename))
            {
                return new List<string>();
            }
            using (StreamReader reader = new StreamReader(filename))
            {
                string content = reader.ReadToEnd();
                List<string> removehistory = JsonConvert.DeserializeObject<List<string>>(content);
                return removehistory;
            }
        }
        private void AddRemoveHistory(string remove)
        {
            List<string> removehistory;

            string filename = System.Configuration.ConfigurationManager.AppSettings["historyofremove"];
            if (!File.Exists(filename))
            {
                removehistory = new List<string>();
                filename = "removehistory.json";
                //本地新建removehistory文件 

                string key = "historyofremove";
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                {
                    config.AppSettings.Settings[key].Value = filename;

                }
                else
                {
                    config.AppSettings.Settings.Add(key, filename);
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string content = reader.ReadToEnd();
                    removehistory = JsonConvert.DeserializeObject<List<string>>(content);
                    if (removehistory == null)
                    {
                        removehistory = new List<string>();
                    }
                    reader.Close();
                }
            }
            removehistory.Add(remove.Trim());
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(JsonConvert.SerializeObject(removehistory.Distinct<string>()));
                writer.Flush();
                writer.Close();
            }

        }
        public DataTable GetDataTableFromGridView(DataGridView dataview)
        {
            DataTable instituteData = memoryInstituteDataTable.Clone();

            foreach (DataGridViewRow row in dataview.Rows)
            {

                if (row.Cells[0].Value != null)
                {
                    DataRow dr = instituteData.NewRow();
                    dr[0] = row.Cells[0].Value;
                    dr[1] = row.Cells[1].Value;
                    dr[2] = row.Cells[2].Value;
                    dr[3] = row.Cells[3].Value;
                    dr[4] = row.Cells[4].Value;
                    instituteData.Rows.Add(dr);
                }
            }
            return instituteData;
        }
        private string GetOutputFilename(string prefixname, string filename)
        {
            FileInfo fileinfo = new FileInfo(filename);
            return prefixname + fileinfo.Name;
        }
        #region
        //private void btnSaveNormTable_Click_1(object sender, EventArgs e)
        //{
        //    if (dgvInstituteDataTable == null || dgvInstituteDataTable.Rows.Count == 0)
        //    {
        //        MessageBox.Show("没有要保存的规范数据");
        //        return;
        //    }
        //    saveFileDialog1.Filter = "Text File(*.csv)|*.csv";
        //    DialogResult dialogresult = saveFileDialog1.ShowDialog();

        //    if (dialogresult == DialogResult.OK)
        //    {
        //        Tuple<string, int> result = NormalizeTable.Import(dgvInstituteDataTable);
        //        using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8))
        //        {
        //            writer.Write(result.Item1);
        //            writer.Flush();
        //            writer.Close();
        //        }
        //        AddLog("保存规范表数据:" + result.Item2.ToString());
        //        MessageBox.Show("规范表保存成功！");
        //    }
        //}
        #endregion
        private void btnReadNormTable_Click(object sender, EventArgs e)
        {
            openFileDialog_NormTable.Filter = "Text File(*.csv)|*.csv";
            DialogResult dialogresult = openFileDialog_NormTable.ShowDialog();
            if (dialogresult == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog_NormTable.FileName))
            {
                #region 委托
                Func<DataGridView, DataTable> getDataTableFromGridView = (dgvInstituteDataTable) =>
                {
                    return GetDataTableFromGridView(dgvInstituteDataTable);
                };
                Action<DataTable> bingDataGridViewDataSource = (normtable) =>
                {
                    BindInstituteDataTable(normtable);
                };
                Action showResult = () =>
                {
                    ShowResult();
                };
                Action<string> addLog = (content) =>
                {
                    AddLog(content);
                };
                Func<DataTable, DataTable, DataTable> merge = (maintable, updatetable) =>
                {
                    return Merge(maintable, updatetable);
                };
                #endregion
                Task task = new Task(() =>
                {
                    DataTable NormTable = NormalizeTable.Import(openFileDialog_NormTable.FileName);
                    this.BeginInvoke(addLog, "加载规范表数据:" + NormTable.Rows.Count.ToString() + "条");
                    if (NormTable.Rows.Count > 0)
                    {
                        if (dgvInstituteDataTable.Rows.Count > 0)
                        {
                            DataTable dgvTable = (DataTable)this.Invoke(getDataTableFromGridView, dgvInstituteDataTable);
                            // NormTable.Merge(dgvTable);
                            NormTable = (DataTable)this.Invoke(merge, dgvTable, NormTable);

                        }
                        this.BeginInvoke(bingDataGridViewDataSource, NormTable);
                    }
                });

                isOpen = true;
                UseWaitCursor = true;
                task.Start();
                task.ContinueWith(task1 =>
                {
                    this.Invoke(showResult);
                });

            }
        }
        /// <summary>
        /// 对比更新
        /// </summary>
        /// <param name="maintable"></param>
        /// <param name="updatetable"></param>
        /// <returns></returns>
        public DataTable Merge(DataTable maintable, DataTable updatetable)
        {

            for (int index = 0; index < maintable.Rows.Count; index++)
            {
                maintable.Rows[index]["HashCode"] = (maintable.Rows[index]["一级机构"].ToString() + maintable.Rows[index]["二级机构"].ToString()).ToLower().GetHashCode();
            }
            for (int index = 0; index < maintable.Rows.Count; index++)
            {
                updatetable.Rows[index]["HashCode"] = (updatetable.Rows[index]["一级机构"].ToString() + updatetable.Rows[index]["二级机构"].ToString()).ToLower().GetHashCode();
            }

            List<int> positions = new List<int>();
            for (int i = 0; i < updatetable.Rows.Count; i++)
            {
                DataRow item = updatetable.Rows[i];
                string query = string.Format("HashCode='{0}'", item["HashCode"]);
                DataRow[] rows = maintable.Select(query);
                if (rows.Length == 0)
                    continue;
                positions.Add(i);
                foreach (DataRow row in rows)
                {
                    string[] temp = item["学科规范"].ToString().Split(';');
                    string tempmain = row["学科规范"].ToString();
                    for (int index = 0; index < temp.Length; index++)
                    {
                        if (!tempmain.Contains(temp[index]))
                        {
                            if (tempmain.Length > 0)
                                tempmain += ";" + temp[index];
                            else
                            {
                                tempmain = temp[index];
                            }
                        }
                    }
                    row["学科规范"] = tempmain;

                    temp = item["学校规范"].ToString().Split(';');
                    tempmain = row["学校规范"].ToString();
                    for (int index = 0; index < temp.Length; index++)
                    {
                        if (!tempmain.Contains(temp[index]))
                        {
                            if (tempmain.Length > 0)
                                tempmain += ";" + temp[index];
                            else
                            {
                                tempmain = temp[index];
                            }
                        }
                    }
                    row["学校规范"] = tempmain;

                    row["学校规范"] = item["学校规范"].ToString();
                }
                //updatetable.Rows.Remove(item);

            }
            for (int index = positions.Count - 1; index >= 0; index--)
            {
                updatetable.Rows.RemoveAt(index);
            }
            if (updatetable.Rows.Count > 0)
            {
                maintable.Merge(updatetable);
            }
            return maintable;
        }
        public void ShowResult(string message = null)
        {
            if (UseWaitCursor)
                UseWaitCursor = false;
            if (isOpen)
            {
                isOpen = false;
                dgvInstituteDataTable.UseWaitCursor = false;
                if (message == null)
                    MessageBox.Show("数据加载完成");
                else
                {
                    MessageBox.Show(message);
                }
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtFileNames.Text.Trim().Length == 0 || (memoryFilenames == null || memoryFilenames.Length == 0))
            {
                MessageBox.Show("请选择文件");
                return;
            }
            if (cbbSystem.SelectedItem == null)
            {
                MessageBox.Show("请选择文件格式选项");
                return;
            }
            #region 委托
            Func<List<UniversityItem>, bool> bingUniversities = (universities) =>
            {
                BingUniversity(universities);
                return true;
            };
            Func<string, bool> addLog = (content) =>
            {
                AddLog(content);
                return true;
            };
            Func<DataTable, bool> bindInstiuteDataTable = (instituteDataTable) =>
            {
                BindInstituteDataTable(instituteDataTable);
                return true;
            };
            Action<bool> changeStatus = (status) =>
            {
                btnImport.Enabled = status;
            };
            Action showResult = () =>
            {
                ShowResult();
            };
            Action setOpen = () =>
            {
                isOpen = true;
            };
            Action fucosSearchUniv = () =>
            {
                txtSearchUniv.Focus();
            };
            #endregion

            Task task = new Task(() =>
            {
                this.Invoke(setOpen);
                this.Invoke(changeStatus, false);
                UseWaitCursor = true;
                List<string> Fields = new List<string>();
                //List<string> Universities = new List<string>();
                // Dictionary<string, int> universities = new Dictionary<string, int>();
                //Dictionary<string, double> _universities = new Dictionary<string, double>();

                foreach (var file in memoryFilenames)
                {
                    Fields.AddRange(ifilter.GetInstituteFields(file));
                }
                // universities = ifilter.GetParentInstitutes(Fields);
                memoryInstituteDataTable = ifilter.GetInstitutes(Fields);
                tempInstituteDataTable = memoryInstituteDataTable.Copy();

                //string maxInstitute = universities.OrderByDescending(k => k.Value).First<KeyValuePair<string, int>>().Key;
                //Univerisities.Clear();
                //foreach (var item in universities)
                //{
                //    UniversityItem uitem = new UniversityItem();
                //    uitem.Name = item.Key;
                //    uitem.Frequence = item.Value;
                //    uitem.Similarity = maxInstitute.CalculateSimilarity(item.Key);
                //    Univerisities.Add(uitem);
                //}
                Univerisities = GenerateUniversities(Fields);
                this.BeginInvoke(bingUniversities, Univerisities);
                this.BeginInvoke(bindInstiuteDataTable, memoryInstituteDataTable);

            });
            task.Start();
            task.ContinueWith(tasks =>
            {
                this.BeginInvoke(changeStatus, true);
                this.BeginInvoke(fucosSearchUniv);
                if (!task.IsFaulted)
                {

                    this.BeginInvoke(showResult);
                }
                else
                {
                    this.BeginInvoke(showResult, "格式错误");
                }
            });
        }
        List<UniversityItem> GenerateUniversities(List<string> addresslist)
        {
            Dictionary<string, int> universities = new Dictionary<string, int>();
            //用于去重使用
            Dictionary<string, int> distinct = new Dictionary<string, int>();
            universities = ifilter.GetParentInstitutes(addresslist);
            string maxInstitute = universities.OrderByDescending(k => k.Value).First<KeyValuePair<string, int>>().Key;
            List<UniversityItem> univs = new List<UniversityItem>();

            foreach (var item in universities)
            {
                UniversityItem uitem = new UniversityItem();
                uitem.Name = item.Key;
                uitem.Frequence = item.Value;
                uitem.Similarity = maxInstitute.CalculateSimilarity(item.Key);
                univs.Add(uitem);
            }
            return univs;

        }



        private void cbbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string system = cbbSystem.SelectedItem.ToString();
            switch (system)
            {
                case "Wos(sci,ssci,cpci)":
                    ifilter = new WosFilter();
                    fileformat = "endnote";
                    break;
                case "Medline":
                    ifilter = new MedlineFilter();
                    fileformat = "endnote";
                    break;
                case "EI":
                    ifilter = new EIFilter();
                    fileformat = "ei";
                    break;
                case "CSCD":
                    ifilter = new CSCD2Filter();
                    fileformat = "cscd";
                    break;
                case "CSSCI":
                    ifilter = new CSSCIFilter();
                    fileformat = "cssci";
                    break;
            }
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (dgvInstituteDataTable.Rows.Count == 0)
            {
                MessageBox.Show("数据为空,无效操作");
                return;
            }

            DialogResult dialogresult = fbdOutputData.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {

                #region 初始化InstituteMatch
                InstiuteMatch institute = new InstiuteMatch();
                string subjectfilter = txtSubjectFilter.Text.Trim();
                foreach (DataGridViewRow row in dgvInstituteDataTable.Rows)
                {
                    string value = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : "";
                    if (value.Length > 0)
                    {
                        //根据subjectfilter来选择需要导出的学科
                        string[] temps = value.ToString().Split(';');
                        foreach (var item in temps)
                        {
                            if (item == subjectfilter)
                            {
                                institute.Add((string)row.Cells[0].Value, (string)row.Cells[1].Value);
                                break;
                            }
                        }

                    }
                }
                #endregion
                PaperList paperlist = new PaperList(institute);
                foreach (var file in memoryFilenames)
                {
                    paperlist.Clear();
                    paperlist.ReadFile(file, fileformat);
                    paperlist.MatchPaperList();
                    if (paperlist.paperlist.Count == 0)
                    {
                        AddLog("无匹配数据:" + file);
                    }
                    else
                    {
                        //将输出的数据量也保存在文件名中
                        string filename = fbdOutputData.SelectedPath + "\\" + GetOutputFilename("_" + paperlist.paperlist.Count.ToString() + "_", file);
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filename))
                        {
                            writer.Write(paperlist.ToString());
                            writer.Flush();
                            writer.Close();
                            AddLog("数据导出[" + paperlist.paperlist.Count.ToString() + "]:" + filename);
                        }
                    }
                }
                MessageBox.Show("数据保存成功");
            }

        }





        #region 一级机构排序
        private void btnFrequence_Click(object sender, EventArgs e)
        {
            SetUniversityList();
            List<UniversityItem> items = (from p in Univerisities orderby p.Frequence descending select p).ToList<UniversityItem>();
            chbUniversities.Sorted = false;
            BingUniversity(items);
        }
        private void btnStringOrder_Click(object sender, EventArgs e)
        {
            chbUniversities.Sorted = true;
        }
        private void btnSimilarity_Click_2(object sender, EventArgs e)
        {
            SetUniversityList();
            List<UniversityItem> items = (from p in Univerisities orderby p.Similarity descending select p).ToList<UniversityItem>();
            chbUniversities.Sorted = false;
            BingUniversity(items);
        }
        private void btnChecked_Click(object sender, EventArgs e)
        {
            SetUniversityList();
            List<UniversityItem> items = (from p in Univerisities orderby p.Checked descending select p).ToList<UniversityItem>();
            chbUniversities.Sorted = false;
            BingUniversity(items);
        }
        #endregion
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string content = txtSearchUniv.Text.Trim().ToLower();
            for (int index = 0; index < chbUniversities.Items.Count; index++)
            {
                if (chbUniversities.GetItemChecked(index))
                {
                    var item = Univerisities.Find(p => p.Name == chbUniversities.Items[index].ToString());
                    item.Checked = true;
                }
            }
            List<UniversityItem> items = (from p in Univerisities where p.Name.ToLower().Contains(content) orderby p.Frequence descending select p).ToList<UniversityItem>();
            chbUniversities.Sorted = false;
            BingUniversity(items);
        }

        private void FilterInstitute_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main main = (Main)this.ParentForm;
            main.规范表ToolStripMenuItem.Enabled = false;
        }
        #region 会被Main调用的方法
        public string GetSystemFormat()
        {
            return cbbSystem.SelectedItem != null ? cbbSystem.SelectedItem.ToString() : "";
        }
        public void SetSystemFormat(string format)
        {
            cbbSystem.SelectedItem = format;
        }
        public string GetFilenames()
        {
            return txtFileNames.Text.Trim();
        }
        public void SetFilenames(string filenames)
        {
            txtFileNames.Text = filenames;
        }
        /// <summary>
        /// 使得Universities的被选与chbUniversities保持一致
        /// </summary>
        public void SetUniversityList()
        {
            for (int index = 0; index < chbUniversities.Items.Count; index++)
            {
                if (chbUniversities.GetItemChecked(index))
                {
                    var item = Univerisities.Find(p => p.Name == chbUniversities.Items[index].ToString());
                    item.Checked = true;
                }
            }
        }
        #endregion

        #region 一二级机构筛选
        /// <summary>
        /// 更新临时表
        /// </summary>
        /// <param name="univ"></param>
        /// <param name="insti"></param>
        public void UpdateTempInstituteDataTable(string univ, string insti)
        {

            if (string.IsNullOrEmpty(univ) && !string.IsNullOrEmpty(insti))
            {
                //只有二级机构
                for (int index = tempInstituteDataTable.Rows.Count - 1; index >= 0; index--)
                {
                    if (tempInstituteDataTable.Rows[index][0].ToString().Contains(univ))
                    {
                        tempInstituteDataTable.Rows.RemoveAt(index);
                    }
                }

            }
            else if (!string.IsNullOrEmpty(univ) && string.IsNullOrEmpty(insti))
            {
                //只有一级机构
                //只有二级机构
                for (int index = tempInstituteDataTable.Rows.Count - 1; index >= 0; index--)
                {
                    if (tempInstituteDataTable.Rows[index][0].ToString().Contains(insti))
                    {
                        tempInstituteDataTable.Rows.RemoveAt(index);
                    }
                }
            }
            else
            {
                //两个都有
                for (int index = tempInstituteDataTable.Rows.Count - 1; index >= 0; index--)
                {
                    if (tempInstituteDataTable.Rows[index][0].ToString().Contains(insti) && tempInstituteDataTable.Rows[index][0].ToString().Contains(univ))
                    {
                        tempInstituteDataTable.Rows.RemoveAt(index);
                    }
                }
            }
            DataTable table = GetDataTableFromGridView(dgvInstituteDataTable);
            tempInstituteDataTable.Merge(table);

        }
        #endregion
        /// <summary>
        /// 修改数据 同步更新到tempInstituteTable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvInstituteDataTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                object id = dgvInstituteDataTable.Rows[e.RowIndex].Cells[5].Value;
                DataRow row = tempInstituteDataTable.Rows.Find(id);
                if (row != null)
                {
                    row.ItemArray[e.ColumnIndex] = dgvInstituteDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                }
            }
        }

        private void txtUnivFilter_TextChanged(object sender, EventArgs e)
        {
            UnivInstiTextChanged();
        }

        private void txtInstiFilter_TextChanged(object sender, EventArgs e)
        {
            UnivInstiTextChanged();
        }
        public void UnivInstiTextChanged()
        {
            string insti = txtInstiFilter.Text.Trim();
            string parentinsti = txtParentInstiFilter.Text.Trim();

            string rowfilter = string.Empty;
            if (parentinsti.Length == 0)
            {
                rowfilter = string.Format("二级机构 like '%{0}%'", insti);
            }
            else if (insti.Length == 0)
            {
                rowfilter = string.Format("一级机构 like '%{0}%'", parentinsti);
            }
            else
            {
                rowfilter = string.Format("一级机构 like '%{0}%' And 二级机构 like '%{1}%'", parentinsti, insti);
            }
            BindInstituteDataTable(tempInstituteDataTable, rowfilter);

        }

        private void dgvInstituteDataTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvInstituteDataTable.UseWaitCursor = false;
        }

        private void btnFilterInstitute_Click(object sender, EventArgs e)
        {
            string filter = txtFilterInstitute.Text.Trim();
            if (filter.Length == 0)
            {
                MessageBox.Show("请输入要筛选的关键词");
                return;
            }
            #region 委托
            Func<List<UniversityItem>, bool> bingUniversities = (universities) =>
            {
                BingUniversity(universities);
                return true;
            };
            Func<DataTable, bool> bindInstiuteDataTable = (instituteDataTable) =>
            {
                BindInstituteDataTable(instituteDataTable);
                return true;
            };
            #endregion

            List<List<string>> filters = InitalFilter(filter);

            List<string> Fields = new List<string>();

            foreach (var file in memoryFilenames)
            {
                Fields.AddRange(ifilter.GetInstituteFields(file));
            }
            Fields = (from p in Fields where FilterInsitute(p, filters) select p).ToList<string>();

            Univerisities = GenerateUniversities(Fields);
            this.BeginInvoke(bingUniversities, Univerisities);


            memoryInstituteDataTable = ifilter.GetInstitutes(Fields);
            tempInstituteDataTable = memoryInstituteDataTable.Copy();
            Univerisities = GenerateUniversities(Fields);
            this.BeginInvoke(bindInstiuteDataTable, memoryInstituteDataTable);



        }
        private List<List<string>> InitalFilter(string filter)
        {
            string[] filters = filter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<List<string>> _filters = new List<List<string>>();
            foreach (var item in filters)
            {
                if (item.Contains(";"))
                {
                    string[] items = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> _items = new List<string>();
                    for (int index = 0; index < items.Length; index++)
                    {
                        _items.Add(items[index].Trim());
                    }
                    _filters.Add(_items);
                }
                else
                {
                    _filters.Add(new List<string>() { item.Trim() });
                }
            }
            return _filters;
        }
        private bool FilterInsitute(string institute, List<List<string>> filters)
        {
            if (filters == null)
                return false;
            foreach (var p in filters)
            {
                bool result = true;
                for (int i = 0; i < p.Count; i++)
                {
                    result = result && institute.Contains(p[i]);
                }
                if (result)
                {
                    return true;
                }
            }
            return false;
        }
    }
    [Serializable]
    public class UniversityItem
    {
        public string Name { set; get; }
        public double Similarity { set; get; }
        public int Frequence { set; get; }
        public bool Checked { set; get; }
    }
    public class Compare : IEqualityComparer<string>
    {

        public bool Equals(string x, string y)
        {
            return x.ToLower() == y.ToLower();
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }
}

