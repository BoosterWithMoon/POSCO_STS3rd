using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{
    public partial class CAM1_DataGridView : Form
    {
        MainForm main;
        public DataGridView dataGridView1;
        //private System.ComponentModel.IContainer components;
        ImageView imgView;
        public int colCount = 0;

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(404, 204);
            this.dataGridView1.TabIndex = 0;
            // 
            // CAM1_DataGridView
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(404, 204);
            this.ControlBox = false;
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CAM1_DataGridView";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        public CAM1_DataGridView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
            imgView = (ImageView)main.ImageView_forPublicRef();

        }

        // for crossthread
        private delegate void RefreshTemperatureGrid();
        public void RefreshGrid()
        {
            if (dataGridView1.InvokeRequired)
            {
                RefreshTemperatureGrid rtg = new RefreshTemperatureGrid(RefreshGrid);
                dataGridView1.Invoke(rtg);
            }
            else
            {
                ShowTemperatureOnGrid();
            }
        }

        public void ShowTemperatureOnGrid()
        {
            if (imgView.CAM1_POICount == 0)
            {
                dataGridView1.Columns.Clear();
                return;
            } 
            dataGridView1.Columns.Clear();
            //dataGridView1.ColumnCount = imgView.CAM1_POICount + 1;
            dataGridView1.ColumnCount = imgView.CAM1_POICount;

            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                string temp = (i + 1).ToString();
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns[i].Name = "#" + temp;
                dataGridView1[(i), 0].Value = imgView.CAM1_TemperatureArr[i].ToString("N1") + "℃";
            }
        }

    }
}
