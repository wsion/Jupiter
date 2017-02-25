using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UserControls
{
    public partial class TableControl : UserControl
    {
        private string _tableName;
        private Point mouseDownLocation;

        //Properties
        public string ConnectionStr { set; get; }
        public string TableOwner { set; get; }
        public string TableName
        {
            set
            {
                _tableName = value;
                label3.Text = _tableName;
            }
            get
            {
                return _tableName;
            }
        }
        public List<string> SelectedColumns { set; get; }

        //Events
        public event EventHandler CloseClicked;
        public event EventHandler SelectionChanged;

        public TableControl()
        {
            InitializeComponent();
        }

        private void buildTableControl(string owner, string tableName)
        {
            var list = new List<TableEntity>();

            using (var con = new SqlConnection(ConnectionStr))
            {
                con.Open();
                var schema = con.GetSchema("Columns",
                    new string[4] {
                        null,
                        owner,
                        tableName,
                        null }).AsEnumerable().ToList();
                foreach (var item in schema)
                {
                    list.Add(new TableEntity()
                    {
                        Column = item.Field<string>("Column_Name"),
                        DataType = item.Field<string>("Data_Type")
                    });
                }
            }

            dataGridView1.DataSource = list;
        }

        public class TableEntity
        {
            public string Column { get; set; }
            public string DataType { get; set; }
            public bool Checked { get; set; }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var chk = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
            if (chk != null)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                getSelectedRows();
                SelectionChanged(null, new EventArgs());
            }
        }

        private void getSelectedRows()
        {
            var columns = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells["Checked"].Value)
                {
                    columns.Add(string.Format("[{0}]", row.Cells["Column"].Value.ToString()));
                }
            }

            SelectedColumns = columns;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            CloseClicked(sender, e);
        }

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

            if (this.Left > 0 && this.Top > 0)
            {
                this.Left = e.X + this.Left - mouseDownLocation.X;
                this.Top = e.Y + this.Top - mouseDownLocation.Y;
            }

            if (this.Left <= 0)
            {
                this.Left = 1;
            }


            if (this.Top <= 0)
            {
                this.Top = 1;
            }
        }
    }
}
