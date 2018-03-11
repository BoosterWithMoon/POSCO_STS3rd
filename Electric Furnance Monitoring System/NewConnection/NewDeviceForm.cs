using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Text;

namespace Electric_Furnance_Monitoring_System
{
    public partial class NewDeviceForm : Form
    {
        MainForm main;
        SystemPropertyGrid grid;

        Thread thr1, thr2;

        public bool isDetected = true;
        public string[] DeviceID;
        // Variable that prevent for CS1690 Warning =======================
        uint NDF_DetectedDevices;
        // ======================= Variable that prevent for CS1690 Warning

        public NewDeviceForm(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();

        }

        public void NewDeviceForm_Load(object sender, EventArgs e)
        {
            //DeviceDetection();
        }

        private void NewDeviceForm_KeyDown(object sender, KeyEventArgs e)
        {
            //esc키를 누르면
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();   // 그냥 닫음
            }
            // enter키를 누르면...
            else if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        public void DeviceDetection()
        {
            Cursor.Current = Cursors.WaitCursor;
            NDF_DetectedDevices = DIASDAQ.DDAQ_DEVICE_DO_DETECTION();
            //Cursor.Current = Cursors.Default;

            if (NDF_DetectedDevices == 0)
            {
                Cursor.Current = Cursors.Default;
                isDetected = false;
                MessageBox.Show("새 연결을 만들 카메라가 감지되지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                MessageBox.Show(NDF_DetectedDevices.ToString()+"개의 카메라가 감지 되었습니다.", "New Device Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        public void ReadyToRun()
        {
            main.DetectedDevices = NDF_DetectedDevices;

            DeviceID = new string[NDF_DetectedDevices];

            byte[] tempDeviceID = new byte[64];                                                                                                                                                                                                                                                                        
            char[] ch_tempDeviceID = new char[64];
            StringBuilder result = new StringBuilder(45);

            // CAMERA #1 (320L)
            DIASDAQ.DDAQ_DEVICE_DO_OPEN(NDF_DetectedDevices, null); 
            DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(NDF_DetectedDevices, tempDeviceID, 64);
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(NDF_DetectedDevices, ref main.pIRDX_Array[0]);
            for (int i = 0; i < 64; i++)
            {
                ch_tempDeviceID[i] = (char)tempDeviceID[i];
                string tmp = ch_tempDeviceID[i].ToString();
                result.Append(tmp);
            }
            string forComboBox = result.ToString();
            DeviceID[0] = forComboBox;
            comboBox1.Items.Add(DeviceID[0]);
            comboBox1.SelectedIndex = 0;

            if (NDF_DetectedDevices == 1) return;
            // CAMERA #2 (512N)
            result.Clear();
            DIASDAQ.DDAQ_DEVICE_DO_OPEN(1, null);
            DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(1, tempDeviceID, 64);
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(1, ref main.pIRDX_Array[1]);
            for (int i = 0; i < 64; i++)
            {
                ch_tempDeviceID[i] = (char)tempDeviceID[i];
                string tmp = ch_tempDeviceID[i].ToString();
                result.Append(tmp);
            }
            forComboBox = result.ToString();
            DeviceID[1] = forComboBox;
            comboBox2.Items.Add(DeviceID[1]);
            comboBox2.SelectedIndex = 0;
        }

        // CLICK ACCEPT BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            //ReadyToRun();

            for (uint i = 0; i < NDF_DetectedDevices; i++)
            {
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(main.pIRDX_Array[i], ref main.sizeX_Array[i], ref main.sizeY_Array[i]);

                DIASDAQ.COLORREF color = new DIASDAQ.COLORREF();
                color.Red = 0; color.Green = 0; color.Blue = 0;

                DIASDAQ.DDAQ_SET_TEMPPRECISION(1);      // scaling bar에서 max,min 온도 사이 값 표현 편차
                ushort avg = 0;
                float min = 0, max = 0;
                DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(main.pIRDX_Array[i], main.currentEmissivity);
                DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(main.pIRDX_Array[i], main.currentTransmitance);
                DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(main.pIRDX_Array[i], 0, 256);
                DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(main.pIRDX_Array[i], ref min, ref max);
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[i], min, max);
                DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(main.pIRDX_Array[i], ref avg);
            }

            // CAMERA #1 Thread ID Registering
            /// Get 기본 Thread ID Value
            uint nThreadID = (uint)Thread.CurrentThread.ManagedThreadId;
            /// Throw Thread ID                    
            if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(NDF_DetectedDevices, nThreadID) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;
            /// Default Frequency
            //if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[0], 8) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            if(DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[0], 1)!=DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;
            /// Device do start
            if (DIASDAQ.DDAQ_DEVICE_DO_START(NDF_DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            if (NDF_DetectedDevices != 1)
            {
                // CAMERA #2 thread id registering
                uint nThreadID_two = (uint)Thread.CurrentThread.ManagedThreadId;
                if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(1, nThreadID_two) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                    return;
                //if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[1], 8) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[1], 1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                    return;
                if (DIASDAQ.DDAQ_DEVICE_DO_START(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                    return;
            }

            grid = (SystemPropertyGrid)main.customGrid_forPublicRef();
            grid.GetAttributesInfo(main.pIRDX_Array[0]);
            main.propertyGrid1.Refresh();

            main.InitImageView();
            main.InitChart();
            main.InitGridView();
            main.InitResultView();

            main.label1.Visible = true;
            main.label3.Visible = true;
            main.label5.Visible = true;
            main.textBox1.Visible = true;
            main.textBox3.Visible = true;

            main.toolStripButton4.Enabled = true;   // Draw POI
            main.toolStripButton5.Enabled = true;   // Delete POI
            main.toolStripButton8.Enabled = true;   // Move POI

            main.toolStripButton6.Enabled = true;

            thr1 = (Thread)main.Thread1_forPublicRef();
            thr1.Start();

            if (NDF_DetectedDevices != 1)
            {
                main.label2.Visible = true;
                main.label4.Visible = true;
                main.label6.Visible = true;
                main.textBox2.Visible = true;
                main.textBox4.Visible = true;
                thr2 = (Thread)main.Thread2_forPublicRef();
                thr2.Start();
            }
            
            this.Close();
        }

        // CLICK CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

       

    }
}