using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{
    class CAM2_DataGridView : Form
    {
        MainForm main;
        public DataGridView dataGridView1;
        ImageView imgView;
        public int CAM2_ColCount = 0;

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
            this.dataGridView1.Size = new System.Drawing.Size(284, 261);
            this.dataGridView1.TabIndex = 0;
            // 
            // CAM2_DataGridView
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.dataGridView1);
            this.Name = "CAM2_DataGridView";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        public CAM2_DataGridView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
            imgView = (ImageView)main.ImageView_forPublicRef();
        }

        private delegate void CAM2_RefreshTemperatureGrid();
        public void CAM2_RefreshGrid()
        {
            if (dataGridView1.InvokeRequired)
            {
                CAM2_RefreshTemperatureGrid c2_rtg = new CAM2_RefreshTemperatureGrid(CAM2_RefreshGrid);
                dataGridView1.Invoke(c2_rtg);
            }
            else
            {
                CAM2_ShowTemperatureOnGrid();
            }
        }

        public void CAM2_ShowTemperatureOnGrid()
        {
            if (imgView.CAM2_POICount == 0)
            {
                dataGridView1.Columns.Clear();
                return;
            }
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = imgView.CAM2_POICount;

            for(int i=0; i < imgView.CAM2_POICount; i++)
            {
                string temp = (i + 1).ToString();
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns[i].Name = "#" + temp;
                dataGridView1[(i), 0].Value = imgView.CAM2_TemperatureArr[i].ToString("N1")+"℃";
            }
        }
    }
}
