using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using Jupiter.UserControls;

namespace ConfigurationBuilder
{
    public partial class MainForm : Form
    {
        private string connectionStr = "Data Source=.;Initial Catalog=Jupiter;Integrated Security=True;";
        private List<TableControl> checkedTables = new List<TableControl>();

        public MainForm()
        {
            InitializeComponent();
            buildTablesTree();
        }

        private void 数据库连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buildTablesTree()
        {
            using (var con = new SqlConnection(connectionStr))
            {
                con.Open();
                var schema = con.GetSchema("Tables");
                foreach (DataRow item in schema.Rows)
                {
                    TreeNode node = new TreeNode();
                    node.Tag = item;
                    node.Text = string.Format("{0}.{1}", item[1], item[2]);
                    treeViewTables.Nodes.Add(node);
                }
            }
        }

        private void treeViewTables_DoubleClick(object sender, EventArgs e)
        {
            var node = treeViewTables.SelectedNode.Tag as DataRow;
            if (node != null)
            {
                addTableControl(node[1].ToString(), node[2].ToString());
            }
        }

        private void addTableControl(string tableOwner, string tableName)
        {
            var tableControl = new Jupiter.UserControls.TableControl(
                   this.connectionStr,
                   tableOwner,
                   tableName);

            tableControl.CloseClicked += (obj, args) =>
            {
                this.splitContainer2.Panel1.Controls.Remove(tableControl);
            };

            tableControl.CheckedChanged += (obj, args) =>
            {
                if (tableControl.Selected)
                {
                    checkedTables.Add(tableControl);
                }
                else
                {
                    checkedTables.Remove(tableControl);
                }
                buttonJoinTables.Enabled = this.checkedTables.Count == 2;
            };            

            tableControl.SelectedColumnChanged += (obj, args) =>
            {
                if (args.IsSelected)
                {
                    this.listBoxSelectedColumns.Items.Add(args.ColumnName);
                }
                else
                {
                    this.listBoxSelectedColumns.Items.Remove(args.ColumnName);
                }
            };

            this.splitContainer2.Panel1.Controls.Add(tableControl);
        }

        private void generateSQL()
        {
            string sql = "SELECT {0} FROM {1}";
            string sqlPreview = "SELECT TOP 5 {0} FROM {1}";
            var columns = new List<string>();

            sql = string.Format(sql, string.Join(",", columns.ToArray()), "tablename");
            sqlPreview = string.Format(sql, string.Join(",", columns.ToArray()), "tablename");

            generatePreviewGrid(sqlPreview);
        }

        private void generatePreviewGrid(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, new SqlConnection(connectionStr));
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridViewPreview.DataSource = ds.Tables[0];
        }

        private void buttonJoinTables_Click(object sender, EventArgs e)
        {

        }
    }
}
