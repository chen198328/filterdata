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
using FilterData.Code;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
namespace FilterData
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void 机构挑选ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (var item in this.MdiChildren)
            {
                if (item.GetType() == typeof(FilterInstitute))
                {
                    this.规范表ToolStripMenuItem.Enabled = true;
                    item.Activate();
                    return;
                }
            }
            FilterInstitute filter = new FilterInstitute();
            filter.MdiParent = this;
            filter.WindowState = FormWindowState.Maximized;
            filter.Show();
            this.规范表ToolStripMenuItem.Enabled = true;
        }

        private void 生成文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in this.MdiChildren)
            {
                if (item.GetType() == typeof(ToHtml))
                {
                    item.Activate();
                    return;
                }
            }
            ToHtml tohtml = new ToHtml();
            tohtml.MdiParent = this;
            tohtml.WindowState = FormWindowState.Maximized;
            tohtml.Show();
        }
        /// <summary>
        /// 一级机构读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "一级机构表(*.json)|*.json"; ;
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    List<UniversityItem> listbox = JsonConvert.DeserializeObject<List<UniversityItem>>(reader.ReadToEnd());
                    reader.Close();

                    if (listbox.Count == 0)
                    {
                        MessageBox.Show("一级机构表没有机构");
                        return;
                    }
                    else
                    {
                        FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));
                        foreach (var item in listbox)
                        {
                            var university = form.Univerisities.Find(p => p.Name == item.Name);
                            if (university != null)
                            {
                                university.Checked = true;
                            }
                            else
                            {
                                form.Univerisities.Add(item);
                            }
                        }
                        //绑定
                        List<UniversityItem> items = (from p in form.Univerisities orderby p.Frequence descending select p).ToList<UniversityItem>();
                        form.chbUniversities.Sorted = false;
                        form.BingUniversity(items);
                        form.AddLog("加载一级机构规范数据:" + listbox.Count + "条");
                    }

                    MessageBox.Show("加载一级机构规范数据:" + listbox.Count + "条");
                }


            }
        }
        /// <summary>
        /// 一级机构保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //一级机构保存

            FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));
            if (form == null) return;
            CheckedListBox listbox = form.chbUniversities;
            if (listbox != null && listbox.Items.Count > 0)
            {

                List<UniversityItem> listitems = new List<UniversityItem>();
                for (int index = 0; index < listbox.Items.Count; index++)
                {
                    if (listbox.GetItemChecked(index))
                    {
                        var item = form.Univerisities.Find(p => p.Name == listbox.Items[index].ToString());
                        item.Checked = true;
                        listitems.Add(item);
                    }
                }
                if (listitems.Count == 0)
                {
                    MessageBox.Show("请选择要保存的一级机构名称");
                    return;
                }
                saveFileDialog.Filter = "一级机构表(*.json)|*.json"; ;
                if (DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        string content = JsonConvert.SerializeObject(listitems);
                        writer.Write(content);
                        writer.Flush();
                        writer.Close();
                        form.AddLog("一级机构表保存成功,总共: " + listitems.Count + "条");
                        MessageBox.Show("一级机构表保存成功");
                    }
                }
            }
            else
            {
                MessageBox.Show("请初始化一级机构列表");
            }

        }
        /// <summary>
        /// 获取子窗口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Form GetForm(Type type)
        {
            foreach (var item in this.MdiChildren)
            {
                if (item.GetType() == type)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 二级机构读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 读取ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));

            openFileDialog.Filter = "Text File(*.csv)|*.csv";
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                #region 委托
                Func<DataGridView, DataTable> getDataTableFromGridView = (dgvInstituteDataTable) =>
                {
                    return form.GetDataTableFromGridView(dgvInstituteDataTable);
                };
                Action<DataTable> bingDataGridViewDataSource = (normtable) =>
                {
                    form.BindInstituteDataTable(normtable);
                };
                Action showResult = () =>
                {
                    form.ShowResult();
                };
                Action<string> addLog = (content) =>
                {
                    form.AddLog(content);
                };
                Func<DataTable, DataTable, DataTable> merge = (maintable, updatetable) =>
                {
                    return form.Merge(maintable, updatetable);
                };
                #endregion
                Task task = new Task(() =>
                {
                    DataTable NormTable = NormalizeTable.Import(openFileDialog.FileName);
                    this.BeginInvoke(addLog, "加载规范表数据:" + NormTable.Rows.Count.ToString() + "条");
                    if (NormTable.Rows.Count > 0)
                    {
                        if (form.dgvInstituteDataTable.Rows.Count > 0)
                        {
                            DataTable dgvTable = (DataTable)this.Invoke(getDataTableFromGridView, form.dgvInstituteDataTable);
                            // NormTable.Merge(dgvTable);
                            NormTable = (DataTable)this.Invoke(merge, dgvTable, NormTable);

                        }
                        form.tempInstituteDataTable = NormTable.Copy();
                        this.BeginInvoke(bingDataGridViewDataSource, NormTable);
                    }
                });

                form.isOpen = true;
                form.UseWaitCursor = true;
                task.Start();
                task.ContinueWith(task1 =>
                {
                    this.Invoke(showResult);
                });
            }
        }
        /// <summary>
        /// 二级机构保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //保存 二级规范表

            FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));
            if (form == null) return;
            DataGridView dgvInstituteDataTable = form.dgvInstituteDataTable;

            if (dgvInstituteDataTable == null || dgvInstituteDataTable.Rows.Count == 0)
            {
                MessageBox.Show("请初始化二级规范表");
                return;
            }
            // List<NormalInstituteRecord> list = NormalizeTable.ExportToList(dgvInstituteDataTable);
            List<NormalInstituteRecord> list = NormalizeTable.ExportToList(form.tempInstituteDataTable);
            saveFileDialog.Filter = "二级机构规范表(*.csv)|*.csv";
            if (list.Count == 0)
            {
                MessageBox.Show("没有已经规范的二级机构名称");
                return;
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                NormalizeTable.Export(list, saveFileDialog.FileName);
                form.AddLog("保存二级机构表:" + list.Count.ToString() + "条");
                MessageBox.Show("保存二级机构表:" + list.Count.ToString() + "条");
            }
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 读取ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            openFileDialog.Filter = "缓存(*.bin)|*.bin";
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                using (Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter iformatter = new BinaryFormatter();
                    CacheClass cache = (CacheClass)iformatter.Deserialize(stream);
                    stream.Close();

                    FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));
                    if (form != null)
                    {
                        form.Close();

                    }

                    form = new FilterInstitute();
                    form.MdiParent = this;
                    form.WindowState = FormWindowState.Maximized;
                    form.Show();
                    #region 设置数据
                    form.fileformat = cache.Fileformat;
                    form.memoryFilenames = cache.MemoryFilenames;
                    form.Univerisities = cache.UniverisityList;
                    form.memoryInstituteDataTable = cache.MemoryTable;
                    form.ifilter = cache.ifilter;
                    form.isOpen = cache.isOpen;
                    form.tempInstituteDataTable = cache.InsituteTable;
                    cache.UniverisityList = (from p in cache.UniverisityList orderby p.Checked descending select p).ToList<UniversityItem>();
                    form.BingUniversity(cache.UniverisityList);
                    form.BindInstituteDataTable(cache.InsituteTable);
                    form.SetFilenames(cache.Filenames);
                    form.SetSystemFormat(cache.Type);
                    #endregion

                    规范表ToolStripMenuItem.Enabled = true;


                }
            }

        }
        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));
            if (form == null) return;
            saveFileDialog.Filter = "缓存(*.bin)|*.bin";

            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                CacheClass cache = new CacheClass();
                cache.Fileformat = form.fileformat;
                cache.MemoryFilenames = form.memoryFilenames;

                form.SetUniversityList();
                cache.UniverisityList = form.Univerisities;
                cache.MemoryTable = form.memoryInstituteDataTable;
                cache.ifilter = form.ifilter;
                cache.isOpen = form.isOpen;
                cache.Type = form.GetSystemFormat();
                cache.Filenames = form.GetFilenames();

                DataGridView dgvInstituteDataTable = form.dgvInstituteDataTable;
                //cache.InsituteTable = form.GetDataTableFromGridView(dgvInstituteDataTable);
                cache.InsituteTable = form.tempInstituteDataTable;

                using (Stream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    IFormatter iformatter = new BinaryFormatter();
                    iformatter.Serialize(stream, cache);
                    stream.Close();
                    MessageBox.Show("缓存保存成功");
                }
            }


        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryList form = (HistoryList)GetForm(typeof(HistoryList));
            if (form != null)
            {
                form.Close();
            }
            form = new HistoryList();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryListChange form = (HistoryListChange)GetForm(typeof(HistoryListChange));
            if (form != null)
            {
                form.Close();
            }
            form = new HistoryListChange();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateLog form = (UpdateLog)GetForm(typeof(UpdateLog));
            if (form != null)
            {
                form.Close();
            }
            form = new UpdateLog();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Action ShowUpdateForm = () =>
            {
                UpdateLog form = (UpdateLog)GetForm(typeof(UpdateLog));
                if (form != null)
                {
                    form.Close();
                }
                form = new UpdateLog();
                form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            };
            Func<bool> Validate = () =>
            {
                string key = "firstopen";
                bool result = string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]);
                if (result)
                {
                    try
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        if (config.AppSettings.Settings[key] != null)
                        {
                            config.AppSettings.Settings[key].Value = DateTime.Now.ToString();

                        }
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    catch { }
                }
                return result;
            };
            Task task = new Task(() =>
            {
                bool result = (bool)this.Invoke(Validate); 
                if (result)
                {
                    this.Invoke(ShowUpdateForm);
                }
            });
            task.Start();





        }

        private void SaveAllInstitute(object sender, EventArgs e)
        {
            //保存 二级规范表

            FilterInstitute form = (FilterInstitute)GetForm(typeof(FilterInstitute));
            if (form == null) return;
            DataGridView dgvInstituteDataTable = form.dgvInstituteDataTable;

            if (dgvInstituteDataTable == null || dgvInstituteDataTable.Rows.Count == 0)
            {
                MessageBox.Show("请初始化二级规范表");
                return;
            }
            // List<NormalInstituteRecord> list = NormalizeTable.ExportToList(dgvInstituteDataTable);
            List<NormalInstituteRecord> list = NormalizeTable.ExportToList(form.tempInstituteDataTable, true);
            saveFileDialog.Filter = "二级机构规范表(*.csv)|*.csv";
            if (list.Count == 0)
            {
                MessageBox.Show("没有已经规范的二级机构名称");
                return;
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                NormalizeTable.Export(list, saveFileDialog.FileName);
                form.AddLog("保存二级机构表:" + list.Count.ToString() + "条");
                MessageBox.Show("保存二级机构表:" + list.Count.ToString() + "条");
            }
        }

        private void 缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}
