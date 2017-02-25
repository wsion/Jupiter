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

namespace ConfigurationBuilder
{
    public partial class MainForm : Form
    {
        private string connectionStr = "Data Source=.;Initial Catalog=Jupiter;Integrated Security=True;";

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
                
        public class TableEntity
        {
            public string Column { get; set; }
            public string DataType { get; set; }
            public bool Checked { get; set; }
        }

        private Point mouseDownLocation;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLocation = e.Location;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (panel1.Left > 0 && panel1.Top > 0)
            {
                panel1.Left = e.X + panel1.Left - mouseDownLocation.X;
                panel1.Top = e.Y + panel1.Top - mouseDownLocation.Y;
            }

            if (panel1.Left <= 0)
            {
                panel1.Left = 1;
            }


            if (panel1.Top <= 0)
            {
                panel1.Top = 1;
            }

        }

        private void treeViewTables_DoubleClick(object sender, EventArgs e)
        {
            var node = treeViewTables.SelectedNode.Tag as DataRow;
            if (node != null)
            {
                //buildTableControl(node[1].ToString(), node[2].ToString());
                string tableName = node[1].ToString() + "." + node[2].ToString();
                label1.Text = "表名:" + tableName;
                label1.Tag = tableName;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1.Controls.Remove(panel1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var chk = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
            if (chk != null)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                generateSQL();
            }
        }

        private void generateSQL()
        {
            string sql = "SELECT {0} FROM {1}";
            string sqlPreview = "SELECT TOP 5 {0} FROM {1}";
            var columns = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells["Checked"].Value)
                {
                    columns.Add(string.Format("[{0}]", row.Cells["Column"].Value.ToString()));
                }
            }

            sql = string.Format(sql, string.Join(",", columns.ToArray()), label1.Tag as string);
            sqlPreview = string.Format(sql, string.Join(",", columns.ToArray()), label1.Tag as string);

            generatePreviewGrid(sqlPreview);
        }

        private void generatePreviewGrid(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, new SqlConnection(connectionStr));
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }
    }
}
