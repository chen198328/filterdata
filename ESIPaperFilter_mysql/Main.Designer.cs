namespace ESIPaperFilter
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRead = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMonth = new System.Windows.Forms.NumericUpDown();
            this.nudYear = new System.Windows.Forms.NumericUpDown();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnMatch = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cbbFilenames = new System.Windows.Forms.ComboBox();
            this.chbExport = new System.Windows.Forms.CheckBox();
            this.lblmatch = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.eSIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSaveDirectory = new System.Windows.Forms.TextBox();
            this.btnOpenSaveDirectory = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxExportAll = new System.Windows.Forms.CheckBox();
            this.cbxCategorys = new System.Windows.Forms.ComboBox();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(918, 32);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 5;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(831, 33);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "月份";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "年份";
            // 
            // nudMonth
            // 
            this.nudMonth.Location = new System.Drawing.Point(312, 64);
            this.nudMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMonth.Name = "nudMonth";
            this.nudMonth.Size = new System.Drawing.Size(120, 21);
            this.nudMonth.TabIndex = 9;
            this.nudMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudYear
            // 
            this.nudYear.Location = new System.Drawing.Point(145, 64);
            this.nudYear.Maximum = new decimal(new int[] {
            2020,
            0,
            0,
            0});
            this.nudYear.Minimum = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            this.nudYear.Name = "nudYear";
            this.nudYear.Size = new System.Drawing.Size(120, 21);
            this.nudYear.TabIndex = 8;
            this.nudYear.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(14, 93);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(981, 459);
            this.dataGridView.TabIndex = 12;
            // 
            // btnMatch
            // 
            this.btnMatch.Location = new System.Drawing.Point(623, 62);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(75, 23);
            this.btnMatch.TabIndex = 13;
            this.btnMatch.Text = "匹配";
            this.btnMatch.UseVisualStyleBackColor = true;
            this.btnMatch.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(917, 563);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // cbbFilenames
            // 
            this.cbbFilenames.FormattingEnabled = true;
            this.cbbFilenames.Location = new System.Drawing.Point(60, 34);
            this.cbbFilenames.Name = "cbbFilenames";
            this.cbbFilenames.Size = new System.Drawing.Size(758, 20);
            this.cbbFilenames.TabIndex = 15;
            // 
            // chbExport
            // 
            this.chbExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbExport.AutoSize = true;
            this.chbExport.Location = new System.Drawing.Point(828, 567);
            this.chbExport.Name = "chbExport";
            this.chbExport.Size = new System.Drawing.Size(84, 16);
            this.chbExport.TabIndex = 16;
            this.chbExport.Text = "分文件导出";
            this.chbExport.UseVisualStyleBackColor = true;
            // 
            // lblmatch
            // 
            this.lblmatch.AutoSize = true;
            this.lblmatch.Location = new System.Drawing.Point(712, 68);
            this.lblmatch.Name = "lblmatch";
            this.lblmatch.Size = new System.Drawing.Size(0, 12);
            this.lblmatch.TabIndex = 17;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eSIToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 25);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // eSIToolStripMenuItem
            // 
            this.eSIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入ToolStripMenuItem,
            this.查看ToolStripMenuItem});
            this.eSIToolStripMenuItem.Name = "eSIToolStripMenuItem";
            this.eSIToolStripMenuItem.Size = new System.Drawing.Size(38, 21);
            this.eSIToolStripMenuItem.Text = "ESI";
            // 
            // 导入ToolStripMenuItem
            // 
            this.导入ToolStripMenuItem.Name = "导入ToolStripMenuItem";
            this.导入ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导入ToolStripMenuItem.Text = "查看";
            this.导入ToolStripMenuItem.Click += new System.EventHandler(this.导入ToolStripMenuItem_Click);
            // 
            // 查看ToolStripMenuItem
            // 
            this.查看ToolStripMenuItem.Name = "查看ToolStripMenuItem";
            this.查看ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.查看ToolStripMenuItem.Text = "导入";
            this.查看ToolStripMenuItem.Click += new System.EventHandler(this.查看ToolStripMenuItem_Click);
            // 
            // txtSaveDirectory
            // 
            this.txtSaveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSaveDirectory.Location = new System.Drawing.Point(201, 563);
            this.txtSaveDirectory.Name = "txtSaveDirectory";
            this.txtSaveDirectory.Size = new System.Drawing.Size(526, 21);
            this.txtSaveDirectory.TabIndex = 19;
            // 
            // btnOpenSaveDirectory
            // 
            this.btnOpenSaveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenSaveDirectory.Location = new System.Drawing.Point(740, 562);
            this.btnOpenSaveDirectory.Name = "btnOpenSaveDirectory";
            this.btnOpenSaveDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnOpenSaveDirectory.TabIndex = 20;
            this.btnOpenSaveDirectory.Text = "打开";
            this.btnOpenSaveDirectory.UseVisualStyleBackColor = true;
            this.btnOpenSaveDirectory.Click += new System.EventHandler(this.btnOpenSaveDirectory_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(103, 567);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "导出数据文件夹";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "文件名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "|";
            // 
            // cbxExportAll
            // 
            this.cbxExportAll.AutoSize = true;
            this.cbxExportAll.Location = new System.Drawing.Point(15, 67);
            this.cbxExportAll.Name = "cbxExportAll";
            this.cbxExportAll.Size = new System.Drawing.Size(72, 16);
            this.cbxExportAll.TabIndex = 24;
            this.cbxExportAll.Text = "全部版本";
            this.cbxExportAll.UseVisualStyleBackColor = true;
            // 
            // cbxCategorys
            // 
            this.cbxCategorys.FormattingEnabled = true;
            this.cbxCategorys.Location = new System.Drawing.Point(486, 64);
            this.cbxCategorys.Name = "cbxCategorys";
            this.cbxCategorys.Size = new System.Drawing.Size(121, 20);
            this.cbxCategorys.TabIndex = 25;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(12, 566);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(0, 12);
            this.lblTotalCount.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(447, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "学科";
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(714, 62);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 28;
            this.btnView.Text = "统计";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 594);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.cbxCategorys);
            this.Controls.Add(this.cbxExportAll);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOpenSaveDirectory);
            this.Controls.Add(this.txtSaveDirectory);
            this.Controls.Add(this.lblmatch);
            this.Controls.Add(this.chbExport);
            this.Controls.Add(this.cbbFilenames);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudMonth);
            this.Controls.Add(this.nudYear);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ESI论文筛选";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMonth;
        private System.Windows.Forms.NumericUpDown nudYear;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnMatch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox cbbFilenames;
        private System.Windows.Forms.CheckBox chbExport;
        private System.Windows.Forms.Label lblmatch;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem eSIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看ToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSaveDirectory;
        private System.Windows.Forms.Button btnOpenSaveDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbxExportAll;
        private System.Windows.Forms.ComboBox cbxCategorys;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnView;
    }
}