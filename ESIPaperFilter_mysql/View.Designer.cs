namespace ESIPaperFilter
{
    partial class View
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
            this.cbxYears = new System.Windows.Forms.ComboBox();
            this.nudCite = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnView = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudCite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxYears
            // 
            this.cbxYears.FormattingEnabled = true;
            this.cbxYears.Location = new System.Drawing.Point(74, 15);
            this.cbxYears.Name = "cbxYears";
            this.cbxYears.Size = new System.Drawing.Size(121, 20);
            this.cbxYears.TabIndex = 0;
            // 
            // nudCite
            // 
            this.nudCite.Location = new System.Drawing.Point(367, 15);
            this.nudCite.Name = "nudCite";
            this.nudCite.Size = new System.Drawing.Size(91, 21);
            this.nudCite.TabIndex = 1;
            this.nudCite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "年份";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "被引频次阈值";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(859, 437);
            this.dataGridView1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.Controls.Add(this.nudCite);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbxYears);
            this.groupBox1.Location = new System.Drawing.Point(23, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(859, 45);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(766, 14);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 4;
            this.btnView.Text = "查看";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 535);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "View";
            this.Text = "View";
            this.Load += new System.EventHandler(this.View_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudCite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxYears;
        private System.Windows.Forms.NumericUpDown nudCite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnView;
    }
}