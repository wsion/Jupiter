namespace ConfigurationBuilder
{
    partial class MainForm
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.数据库连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建查询NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewTables = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonJoinTables = new System.Windows.Forms.Button();
            this.dataGridViewPreview = new System.Windows.Forms.DataGridView();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.listBoxSelectedColumns = new System.Windows.Forms.ListBox();
            this.dataGridViewJoinConditions = new System.Windows.Forms.DataGridView();
            this.TableA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStripMain.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJoinConditions)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据库连接ToolStripMenuItem,
            this.新建查询NToolStripMenuItem,
            this.保存配置ToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(806, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // 数据库连接ToolStripMenuItem
            // 
            this.数据库连接ToolStripMenuItem.Name = "数据库连接ToolStripMenuItem";
            this.数据库连接ToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.数据库连接ToolStripMenuItem.Text = "数据库连接(&C)";
            this.数据库连接ToolStripMenuItem.Click += new System.EventHandler(this.数据库连接ToolStripMenuItem_Click);
            // 
            // 新建查询NToolStripMenuItem
            // 
            this.新建查询NToolStripMenuItem.Name = "新建查询NToolStripMenuItem";
            this.新建查询NToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.新建查询NToolStripMenuItem.Text = "新建查询(&N)";
            // 
            // 保存配置ToolStripMenuItem
            // 
            this.保存配置ToolStripMenuItem.Name = "保存配置ToolStripMenuItem";
            this.保存配置ToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.保存配置ToolStripMenuItem.Text = "保存结果(&S)";
            // 
            // treeViewTables
            // 
            this.treeViewTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTables.Location = new System.Drawing.Point(0, 0);
            this.treeViewTables.Name = "treeViewTables";
            this.treeViewTables.Size = new System.Drawing.Size(198, 490);
            this.treeViewTables.TabIndex = 0;
            this.treeViewTables.DoubleClick += new System.EventHandler(this.treeViewTables_DoubleClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewTables);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AllowDrop = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(806, 492);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Controls.Add(this.buttonJoinTables);
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Size = new System.Drawing.Size(602, 492);
            this.splitContainer2.SplitterDistance = 400;
            this.splitContainer2.TabIndex = 2;
            // 
            // buttonJoinTables
            // 
            this.buttonJoinTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonJoinTables.Enabled = false;
            this.buttonJoinTables.Location = new System.Drawing.Point(522, 3);
            this.buttonJoinTables.Name = "buttonJoinTables";
            this.buttonJoinTables.Size = new System.Drawing.Size(75, 23);
            this.buttonJoinTables.TabIndex = 0;
            this.buttonJoinTables.Text = "连接";
            this.buttonJoinTables.UseVisualStyleBackColor = true;
            this.buttonJoinTables.Click += new System.EventHandler(this.buttonJoinTables_Click);
            // 
            // dataGridViewPreview
            // 
            this.dataGridViewPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPreview.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPreview.Name = "dataGridViewPreview";
            this.dataGridViewPreview.Size = new System.Drawing.Size(600, 52);
            this.dataGridViewPreview.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.dataGridViewPreview);
            this.splitContainer3.Size = new System.Drawing.Size(600, 86);
            this.splitContainer3.SplitterDistance = 30;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.listBoxSelectedColumns);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.dataGridViewJoinConditions);
            this.splitContainer4.Size = new System.Drawing.Size(600, 30);
            this.splitContainer4.SplitterDistance = 300;
            this.splitContainer4.TabIndex = 0;
            // 
            // listBoxSelectedColumns
            // 
            this.listBoxSelectedColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSelectedColumns.FormattingEnabled = true;
            this.listBoxSelectedColumns.Location = new System.Drawing.Point(0, 0);
            this.listBoxSelectedColumns.Name = "listBoxSelectedColumns";
            this.listBoxSelectedColumns.Size = new System.Drawing.Size(300, 30);
            this.listBoxSelectedColumns.TabIndex = 0;
            // 
            // dataGridViewJoinConditions
            // 
            this.dataGridViewJoinConditions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJoinConditions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TableA,
            this.TableB});
            this.dataGridViewJoinConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewJoinConditions.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewJoinConditions.Name = "dataGridViewJoinConditions";
            this.dataGridViewJoinConditions.Size = new System.Drawing.Size(296, 30);
            this.dataGridViewJoinConditions.TabIndex = 0;
            // 
            // TableA
            // 
            this.TableA.HeaderText = "TableA";
            this.TableA.Name = "TableA";
            this.TableA.ReadOnly = true;
            // 
            // TableB
            // 
            this.TableB.HeaderText = "TableB";
            this.TableB.Name = "TableB";
            this.TableB.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 516);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.Text = "配置生成器";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJoinConditions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem 数据库连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建查询NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存配置ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeViewTables;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridViewPreview;
        private System.Windows.Forms.Button buttonJoinTables;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListBox listBoxSelectedColumns;
        private System.Windows.Forms.DataGridView dataGridViewJoinConditions;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableA;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableB;
    }
}

