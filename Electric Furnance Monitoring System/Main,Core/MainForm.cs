using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxTeeChart;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using Kepware.ClientAce.OpcDaClient;
using Kepware.ClientAce.OpcCmn;

namespace Electric_Furnance_Monitoring_System
{
    public partial class MainForm : Form
    {
        //DaServerMgt DAServer = new DaServerMgt();

        // HERE IS THE AREA FOR GLOBAL VARIABLES
        #region GlobalVariableDefinition

            #region ClassReference

        // New: Online에 사용할 Dialog + Class
        OpenFileDialog openDlg;
            NewDeviceForm newDevice;
            // Image를 보여줄 스레드 클래스
            //CoreThread coreThread;
            Thread mThread;
            Thread mThread_two;
        Thread CAM1_UpdateThr;
        //Thread CAM2_UpdateThr;
        //EventWaitHandle thr1_waitHandle = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(false);
        
        // Image를 보여주며 사용할 함수들이 정의된 클래스
        ImageView imageView;
            // PropertyGrid 클래스
            SystemPropertyGrid customGrid;
        
            // 카메라가 두 대 붙었을 때 이미지를 보여줄 두 개의 클래스
            CAM1_ImageView c1_img;
            CAM2_ImageView c2_img;
            // Simulation or IRDX 파일을 열었을 때 이미지를 보여줄 클래스
            OnlyOne_ImageView onlyone_img;

            // 카메라가 두 대 붙었을 때 온도값을 표로 보여줄 두 개의 클래스
            CAM1_DataGridView c1_gridView;
            CAM2_DataGridView c2_gridView;
            // Simulation or IRDX 파일을 열었을 때 온도값을 표로 보여줄 클래스
            OnlyOne_DataGridView onlyone_gridView;

            // 카메라가 두 대 붙었을 때 온도값을 그래프로 보여줄 두 개의 클래스
            CAM1_ChartView c1_chartView;
            CAM2_ChartView c2_chartView;
            // Simulation or IRDX 파일을 열었을 때 온도값을 그래프로 보여줄 클래스
            OnlyOne_ChartView onlyone_chartView;

            ResultView result;

            //DaServerMgt daServerMgt = new DaServerMgt();
            //ServerIdentifier[] availableOPCServers;

            //int activeServerSubscriptionHandle;
            //int activeClientSubscriptionHandle;

        #endregion

            #region Variables

        public IntPtr pIRDX;            // 1번 카메라 IRDX HANDLE
            public IntPtr pIRDX_2;         // 2번 카메라 IRDX HANDLE

            public IntPtr[] pIRDX_Array;    // IRDX Handle Array

            bool isClosing = false;

            public string fileFullName = "";    // Open한 file의 full path

            public ushort sizeX;        // 1번 카메라 bmp size x
            public ushort sizeY;        // 1번 카메라 bmp size y
            public ushort sizeX_2;      // 2번 카메라 bmp size x
            public ushort sizeY_2;      // 2번 카메라 bmp size y

            public ushort[] sizeX_Array;    // bmp size x array
            public ushort[] sizeY_Array;    // bmp size y array

            public float currentEmissivity;
            public float currentTransmitance;
            public float currentAmbientTemp;

            public uint NumOfDataRecord;
            public uint CurDataRecord;

            public float m_fps;     // frame per sec
            public ushort m_avg;   
            
            // for scale setting
            public float min = 0, max = 0;
            public float min_2 = 0, max_2 = 0;
            public ushort avg_2 = 0;

            
            public uint DetectedDevices;
            uint position = 0;
            uint numDataSet = 0;
            
            // for keep frame moving in irdx
            System.Windows.Forms.Timer dataplayerTimer = new System.Windows.Forms.Timer();
            bool isTimerRunning = false;

            public bool isCAM1Focused = false;
            
            // draw poi flag
            public bool Activate_DrawPOI = false;


            public bool device_running;
            public uint acq_index = 0;

            public float cMaxTemp = 0;
            public float Scale_MaxTemp = 0;
            public float cMinTemp = 0;
            public float Scale_MinTemp = 0;


            // for data logging
            string NewIRDXFileName = "";
            string newRawDataFileName = "";
            string newResultDataFileName = "";
            string CAM2_NewIRDXFileName = "";
            string CAM2_newRawDataFileName = "";
            string CAM2_newResultDataFileName = "";
            bool isLoggingRunning = false;

            enum POIMode : int
            {
                DRAW_POI=1,
                DELETE_POI=2,
                MOVE_MODE=3
            }

            public static int MAX_PIXEL_X = 320;
            public static int MAX_PIXEL_Y = 240;
            //POIMode currentMode = POIMode.MOVE_MODE;

            #endregion


            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase);

            [DllImport("user32.dll")]
            private static extern bool UpdateWindow(IntPtr hwnd);

            public bool PaintWindow(IntPtr hwnd)
            {
                InvalidateRect(hwnd, IntPtr.Zero, true);
                if (UpdateWindow(hwnd))
                {
                    Application.DoEvents();
                    return true;
                }
                return false;
            }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            #region GlobalVariableAllocation

                #region ClassAllocation

                openDlg = new OpenFileDialog();
                newDevice = new NewDeviceForm(this);
                //coreThread = new CoreThread(this);

                mThread = new Thread(new ThreadStart(run));
                mThread_two = new Thread(new ThreadStart(run_two));
                CAM1_UpdateThr = new Thread(new ThreadStart(run_update));

                imageView = new ImageView(this);
                customGrid = new SystemPropertyGrid(this);

                c1_img = new CAM1_ImageView(this);
                c2_img = new CAM2_ImageView(this);
                onlyone_img = new OnlyOne_ImageView(this);

                c1_gridView = new CAM1_DataGridView(this);
                c2_gridView = new CAM2_DataGridView(this);
                onlyone_gridView = new OnlyOne_DataGridView(this);


                c1_chartView = new CAM1_ChartView(this);
                c2_chartView = new CAM2_ChartView(this);
                onlyone_chartView = new OnlyOne_ChartView(this);

                result = new ResultView(this);

                #endregion

                #region VariableAllocation

                pIRDX = new IntPtr();
                pIRDX_2 = new IntPtr();

                pIRDX_Array = new IntPtr[5];

                sizeX = 0; sizeY = 0;
                sizeX_2 = 0; sizeY_2 = 0;

                sizeX_Array = new ushort[5];
                sizeY_Array = new ushort[5];

                currentEmissivity = 1.0f;
                currentTransmitance = 1.0f;
                currentAmbientTemp = 0.0f;

                NumOfDataRecord = 0;
                CurDataRecord = 0;

                m_fps = 0;
                m_avg = 0;

                DetectedDevices = 0;

                #endregion

            #endregion
        }

        #region Publicize_AllocatedClass


        public object ImageView_forPublicRef() { return imageView; }

        public object customGrid_forPublicRef() { return customGrid; }

        public object CAM1_ImageView_forPublicRef() { return c1_img; }

        public object CAM2_ImageView_forPublicRef() { return c2_img; }

        public object CAM1_ChartView_forPublicRef() { return c1_chartView; }

        public object CAM2_ChartView_forPublicRef() { return c2_chartView; }

        public object CAM1_GridView_forPublicRef() { return c1_gridView; }

        public object CAM2_GridView_forPublicRef() { return c2_gridView; }

        public object ResultView_forPublicRef() { return result; }

        public object Thread1_forPublicRef() { return mThread; }

        public object Thread2_forPublicRef() { return mThread_two; }

        //public object OPC_ServerManagement_forPublicRef() { return daServerMgt; }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

            LoadConfiguration();
            //MessageBox.Show(Electric_Furnance_Monitoring_System.Properties.Settings.Default.Test.ToString());
            //MessageBox.Show("bool test" + Properties.Settings.Default.Setting.ToString());
            // Initialize PropertyGrid
            InitPropertyGrid();

            //// Initialize DataGridView
            //InitGridView();

            //// Initialize TChartArea
            //InitChart();

            //// Initialize ImageViewer
            //InitImageView();

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;

            toolStripButton4.Enabled = false;   // Draw POI
            toolStripButton5.Enabled = false;   // Delete POI
            toolStripButton8.Enabled = false;   // Move POI

            toolStripButton6.Enabled = false;   // Log Start
            toolStripButton7.Enabled = false;   // Log Stop

            // 테스트용 메뉴 버튼 hide
            toolStripButton9.Visible = false;
            toolStripButton10.Visible = false;

            //DetectOPCServer();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfiguration();

            isClosing = true;

            Thread.Sleep(3);

            // 스레드 둘중에 하나라도 돌고있으면 먼저 둘다 죽이고 시작하자
            if (mThread.IsAlive || mThread_two.IsAlive)
            {
                mThread.Abort();
                mThread_two.Abort();
                CAM1_UpdateThr.Abort();
            }

            DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
            DIASDAQ.DDAQ_DEVICE_DO_STOP(2);

            if (DetectedDevices != 0)
            {
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(2);
            }

            System.Threading.Thread.Sleep(100);

            c1_chartView.axTChart1.Dispose();
            c2_chartView.axTChart1.Dispose();
            onlyone_chartView.axTChart1.Dispose();
            //c1_chartView.Dispose();
            //c2_chartView.Dispose();
            //onlyone_chartView.Dispose();
        }

        #region InitComponents

        public void InitPropertyGrid()
        {
            propertyGrid1.PropertySort = PropertySort.Categorized;
            propertyGrid1.ToolbarVisible = false;
            propertyGrid1.SelectedObject = customGrid;
        }
        
        public void InitGridView()
        {
            c1_gridView.TopLevel = false;
            split_CAM1ChartGrid.Panel2.Controls.Add(c1_gridView);
            c1_gridView.Parent = this.split_CAM1ChartGrid.Panel2;
            c1_gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            c1_gridView.Text = "";
            c1_gridView.ControlBox = false;
            c1_gridView.dataGridView1.RowHeadersVisible = false;
            c1_gridView.Show();

            c2_gridView.TopLevel = false;
            split_Cam2ChartGrid.Panel2.Controls.Add(c2_gridView);
            c2_gridView.Parent = split_Cam2ChartGrid.Panel2;
            c2_gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            c2_gridView.Text = "";
            c2_gridView.ControlBox = false;
            c2_gridView.dataGridView1.RowHeadersVisible = false;
            c2_gridView.Show();
        }
        
        public void OnlyOne_InitGridView()
        {
            onlyone_gridView.TopLevel = false;
            split_CAM1ChartGrid.Panel2.Controls.Add(onlyone_gridView);
            onlyone_gridView.Parent = this.split_CAM1ChartGrid.Panel2;
            onlyone_gridView.Size = new Size(split_CAM1ChartGrid.Panel2.Width+split_Cam2ChartGrid.Panel2.Width, split_CAM1ChartGrid.Panel2.Height);
            onlyone_gridView.Text = "";
            onlyone_gridView.ControlBox= false;
            onlyone_gridView.Show();
        }
        
        public void InitImageView()
        {
            //CAM1_ImageView c1_img = new CAM1_ImageView(this);
            c1_img.TopLevel = false;
            split_CAM1info.Panel2.Controls.Add(c1_img);
            c1_img.Parent = split_CAM1info.Panel2;
            c1_img.Dock = DockStyle.Fill;
            c1_img.Text = "";
            c1_img.ControlBox = false;
            c1_img.Show();

            c2_img.TopLevel = false;
            split_CAM2info.Panel2.Controls.Add(c2_img);
            c2_img.Parent = split_CAM2info.Panel2;
            c2_img.Dock = DockStyle.Fill;
            c2_img.Text = "";
            c2_img.ControlBox = false;
            c2_img.Show();
        }

        public void OnlyOne_InitImageView()
        {
            onlyone_img.TopLevel = false;
            MessageBox.Show(split_CamToCam.SplitterWidth.ToString());
            MessageBox.Show(split_CamToCam.Width.ToString()); //1350
            MessageBox.Show(split_CamToCam.SplitterDistance.ToString()); //700
            split_CAM1info.Panel2.Controls.Add(onlyone_img);
            onlyone_img.Parent = split_CAM1info.Panel2;
            onlyone_img.Dock = DockStyle.Fill;
            onlyone_img.Text = "";
            onlyone_img.ControlBox = false;
            onlyone_img.Show();
        }

        public void InitChart()
        {
            //chartView.DrawTest();
            c1_chartView.TopLevel = false;
            split_CAM1ChartGrid.Panel1.Controls.Add(c1_chartView);
            c1_chartView.Parent = split_CAM1ChartGrid.Panel1;
            c1_chartView.Dock = DockStyle.Fill;
            c1_chartView.Text = "";
            c1_chartView.ControlBox = false;
            c1_chartView.Show();

            c2_chartView.TopLevel = false;
            split_Cam2ChartGrid.Panel1.Controls.Add(c2_chartView);
            c2_chartView.Parent = split_Cam2ChartGrid.Panel1;
            c2_chartView.Dock = DockStyle.Fill;
            c2_chartView.Text = "";
            c2_chartView.ControlBox = false;
            c2_chartView.Show();
        }

        public void OnlyOne_InitChart()
        {

        }

        public void InitResultView()
        {
            result.TopLevel = false;
            split_ViewToInfo.Panel2.Controls.Add(result);
            result.Parent = split_ViewToInfo.Panel2;
            result.Dock = DockStyle.Fill;
            result.Text = "";
            result.ControlBox = false;
            result.Show();
        }

        #endregion

        #region OpenNewDevice(Online)

        private void OpenNewDevice()
        {
            newDevice.DeviceDetection();
            if (newDevice.isDetected == true)
            {
                if (newDevice.isDetected == false) return;
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    newDevice.ReadyToRun();
                    Cursor.Current = Cursors.Default;

                    newDevice.ShowDialog();
                }
            }
        }

        

        #endregion

        #region OpenIRDX

        private void OpenIRDX()
        {
            // OpenFileDialog setting
            openDlg.Title = "Open Simulation";
            openDlg.Filter = "IRDX Files(*.irdx)|*.irdx";
            DialogResult dr = openDlg.ShowDialog();
            
            // dialog에서 파일을 열었을 때
            if (dr == DialogResult.OK)
            {
                // GET FULL FILE PATH
                fileFullName = openDlg.FileName;
            }
            // 파일을 열지 않고 닫으려고 할 때
            else if (dr == DialogResult.Cancel)
            {
                return;
            }
            //MessageBox.Show(fileFullName);

            // IRDX handle 받아오기
            DIASDAQ.DDAQ_IRDX_FILE_OPEN_READ(fileFullName, true, ref pIRDX_Array[0]);

            ushort tempX = 0, tempY = 0;
            DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[0], ref tempX, ref tempY);

            sizeX = tempX;  sizeY = tempY;

            //OnlyOne_InitImageView();
            //imageView.DrawImage(pIRDX_Array[0], onlyone_img.pictureBox1);
            InitImageView();
            InitChart();
            InitGridView();

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;

            imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);
            imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);

            customGrid.GetAttributesInfo(pIRDX);
            propertyGrid1.Refresh();
        }
       
        

        #endregion

        // [[TEST]] Device Detection Button ===================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            //// DEVICE DETECTION START
            //Cursor.Current = Cursors.WaitCursor;
            //DetectedDevices = DIASDAQ.DDAQ_DEVICE_DO_DETECTION();

            //// CAMERA OPEN
            //if (DIASDAQ.DDAQ_DEVICE_DO_OPEN(DetectedDevices, null) != DIASDAQ.DDAQ_ERROR.NO_ERROR)  /// 두번째 카메라 연결 | 현재는 320L
            //    return;
            //if (DIASDAQ.DDAQ_DEVICE_DO_OPEN(1, null) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                    /// 첫번째 카메라 연결 | 현재는 512N
            //    return;

            //byte[] temp = new byte[64];
            //char[] IDString = new char[64];
            //if (DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(DetectedDevices, temp, 64) != DIASDAQ.DDAQ_ERROR.NO_ERROR) return;
            //StringBuilder result = new StringBuilder(45);
            //for (int i = 0; i < 64; i++)
            //{
            //    IDString[i] = (char)temp[i];
            //    string cvt = IDString[i].ToString();
            //    //result.Insert(result.Length, cvt);
            //    result.Append(cvt);
            //}
            //string aa = result.ToString();
            //MessageBox.Show(aa);

            //if (DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(1, temp, 64) != DIASDAQ.DDAQ_ERROR.NO_ERROR) return;

            //// GET CAMERA'S IRDX HANDLE
            //if (DIASDAQ.DDAQ_DEVICE_GET_IRDX(DetectedDevices, ref pIRDX) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;
            //if (DIASDAQ.DDAQ_DEVICE_GET_IRDX(1, ref pIRDX_2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;

            //// GET CAMERA'S PIXEL SIZE
            //if (DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX, ref sizeX, ref sizeY) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;
            //if (DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_2, ref sizeX_2, ref sizeY_2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;


            //DIASDAQ.COLORREF color = new DIASDAQ.COLORREF();
            //color.Red = 0; color.Blue = 0; color.Green = 0;
            ////-----------------------------------------
            //DIASDAQ.DDAQ_SET_TEMPPRECISION(0);
            //ushort avg = 0;
            //DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(pIRDX, currentEmissivity);                          /// 두번째 카메라 핵심 프로퍼티 설정
            //DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(pIRDX, currentTransmission);
            //DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(pIRDX, 0, 256);
            //DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(pIRDX, ref min, ref max);
            //DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(pIRDX, min, max);
            //DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX, ref avg);

            //DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(pIRDX_2, currentEmissivity);                     /// 첫번째 카메라 핵심 프로퍼티 설정
            //DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(pIRDX_2, currentTransmission);
            //DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(pIRDX_2, 0, 256);
            //DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(pIRDX_2, ref min_2, ref max_2);
            //DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(pIRDX_2, min_2, max_2);
            //DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_2, ref avg_2);

            //// GET IMAGE VIEW THREAD ID
            //uint nThreadID = (uint)Thread.CurrentThread.ManagedThreadId;                        /// 기본 Thread ID Value get
            //uint nThreadID_two = (uint)Thread.CurrentThread.ManagedThreadId;
            //// THROW THREADS ID
            //if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(DetectedDevices, nThreadID) != DIASDAQ.DDAQ_ERROR.NO_ERROR)   /// 스레드 ID 등록
            //    return;
            //if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(1, nThreadID_two) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;

            //if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(pIRDX, 8) != DIASDAQ.DDAQ_ERROR.NO_ERROR)               /// Default ACQ_Frequency 8 으로 설정
            //    return;
            //if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(pIRDX_2, 8) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;

            //if (DIASDAQ.DDAQ_DEVICE_DO_START(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                   /// 각 카메라 Do Start!!
            //    return;
            //if (DIASDAQ.DDAQ_DEVICE_DO_START(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //    return;

            //Cursor.Current = Cursors.Default;

            //customGrid.GetAttributesInfo(pIRDX);
            //propertyGrid1.Refresh();

            //InitImageView();

            //coreThread.mThread.Start();
            //coreThread.mThread_two.Start();
        }
        // =================================================================== [[TEST]] Device Detection Button

        #region OpenSimulation

        private void OpenSimulation()
        {
            // OpenFileDialog setting
            openDlg.Title = "Open Simulation";
            openDlg.Filter = "IRD Files(*.ird)|*.ird";
            DialogResult dr = openDlg.ShowDialog();

            // dialog에서 파일을 열었을 때
            if (dr == DialogResult.OK)
            {
                // GET FULL FILE PATH
                fileFullName = openDlg.FileName;
            }
            // 파일을 열지 않고 닫으려고 할 때
            else if (dr == DialogResult.Cancel)
            {
                return;
            }

            DetectedDevices = 2;

            DIASDAQ.DDAQ_DEVICE_DO_OPENSIMULATION(1, fileFullName);     // DevNo1: 512N
            DIASDAQ.DDAQ_DEVICE_DO_OPENSIMULATION(2, fileFullName);     // DevNo2: 320L

            // IRDX handle 받아오기
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(1, ref pIRDX_Array[0]); // 512
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(2, ref pIRDX_Array[1]); // 320

            if (DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[0], ref sizeX, ref sizeY) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;
            if (DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[1], ref sizeX_2, ref sizeY_2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            DIASDAQ.COLORREF color = new DIASDAQ.COLORREF();
            color.Red = 0; color.Blue = 0; color.Green = 0;
            //-----------------------------------------
            DIASDAQ.DDAQ_SET_TEMPPRECISION(0);
            ushort avg = 0;
            DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(pIRDX_Array[0], currentEmissivity);                          /// 두번째 카메라 핵심 프로퍼티 설정
            DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(pIRDX_Array[0], currentTransmitance);
            DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(pIRDX_Array[0], 0, 256);
            DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(pIRDX_Array[0], ref min, ref max);
            DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(pIRDX_Array[0], min, max);
            DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_Array[0], ref avg);

            DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(pIRDX_Array[1], currentEmissivity);                          /// 두번째 카메라 핵심 프로퍼티 설정
            DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(pIRDX_Array[1], currentTransmitance);
            DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(pIRDX_Array[1], 0, 256);
            DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(pIRDX_Array[1], ref min, ref max);
            DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(pIRDX_Array[1], min, max);
            DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_Array[1], ref avg);

            uint nThreadID = (uint)Thread.CurrentThread.ManagedThreadId;                        /// 기본 Thread ID Value get
            uint nThreadID2 = (uint)Thread.CurrentThread.ManagedThreadId;

            // THROW THREADS ID
            if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(1, nThreadID) != DIASDAQ.DDAQ_ERROR.NO_ERROR)   /// 스레드 ID 등록
                return;
            if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(2, nThreadID2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            // SET ACQUISITION FREQUENCY
            if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(pIRDX_Array[0], 2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)               /// Default ACQ_Frequency 8 으로 설정
                return;
            if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(pIRDX_Array[1], 2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            if (DIASDAQ.DDAQ_DEVICE_DO_START(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                   /// 각 카메라 Do Start!!
                return;
            if (DIASDAQ.DDAQ_DEVICE_DO_START(2) != DIASDAQ.DDAQ_ERROR.NO_ERROR) return;

            DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(1);
            DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(2);

            //customGrid.GetAttributesInfo();
            //propertyGrid1.Refresh();

            InitImageView();
            InitChart();
            InitGridView();

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;

            customGrid.GetAttributesInfo(pIRDX_Array[0]);

            mThread.Start();
            mThread_two.Start();
        }

        private void newSimulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSimulation();
        }

        #endregion

        #region IRDX_FrameMoving

        // previous Frame
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(pIRDX_Array[0], ref position);

            if (position == 0)
            {
                // position이 이미 시작점에 있다면 아무것도 하지 않음
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_FILE_SET_CURDATASET(pIRDX_Array[0], position - 1);
                position--;
            }
            CurDataRecord = position;

            float pData = 0;
            uint bufferSize = 0;
            DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

            imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);
            imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);

            customGrid.GetAttributesInfo(pIRDX_Array[0]);
        }

        // next Frame
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
           
            DIASDAQ.DDAQ_IRDX_FILE_GET_NUMDATASETS(pIRDX_Array[0], ref numDataSet);
            
            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(pIRDX_Array[0], ref position);

            if (position+1 == numDataSet)
            {
                // position이 DataSet의 끝까지 갔다면 제일 첫 프레임으로 보냄
                position = 0;
                dataplayerTimer.Stop();

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);
                imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_FILE_SET_CURDATASET(pIRDX_Array[0], position + 1);
                position++;

                CurDataRecord = position;

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);
                imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
            }
        }

        private void InitTimerForPlayer()
        {
            dataplayerTimer.Interval = 10;  // ms
            dataplayerTimer.Tick += new EventHandler(toolStripButton2_Click);

            dataplayerTimer.Start();
            isTimerRunning = true;
        }

        // keep next frame
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (isTimerRunning == true) { }
            else
            {
                InitTimerForPlayer();
            }
        }

        // pause button
        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            dataplayerTimer.Stop();
            isTimerRunning = false;
        }

        #endregion

        #region Control_MenuItem

        #region Menu_File
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewDevice();
        }
        private void openIRDXOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenIRDX();
        }

        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Menu_DeviceLogging

        // Device Logging - Start
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceLoggingStart();
            toolStripButton6.Enabled = false;
            toolStripButton7.Enabled = true;
        }
        // Device Logging - Stop
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceLoggingStop();
            toolStripButton6.Enabled = true;
            toolStripButton7.Enabled = false;
        }

        #endregion

        #region Menu_DataPlayer

        // Data Player - Previous Record
        private void previousRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (position == 0)
            {
                // position이 이미 시작점에 있다면 아무것도 하지 않음
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_FILE_SET_CURDATASET(pIRDX_Array[0], position - 1);
                position--;
            }
            CurDataRecord = position;

            float pData = 0;
            uint bufferSize = 0;
            DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

            imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);
            imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);

            customGrid.GetAttributesInfo(pIRDX_Array[0]);
        }

        // Data Player - Next record
        private void nextRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DIASDAQ.DDAQ_IRDX_FILE_GET_NUMDATASETS(pIRDX_Array[0], ref numDataSet);

            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(pIRDX_Array[0], ref position);

            if (position + 1 == numDataSet)
            {
                // position이 DataSet의 끝까지 갔다면 제일 첫 프레임으로 보냄
                position = 0;
                dataplayerTimer.Stop();

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);
                imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_FILE_SET_CURDATASET(pIRDX_Array[0], position + 1);
                position++;

                CurDataRecord = position;

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                imageView.DrawImage(pIRDX_Array[0], c2_img.pictureBox1);
                imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
            }
        }

        // Data Player - Play
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isTimerRunning == true) { }
            else
            {
                InitTimerForPlayer();
            }
        }

        // data player - stop(pause)
        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataplayerTimer.Stop();
            isTimerRunning = false;
        }

        #endregion

        #region Menu_ROI

        // roi - draw
        private void drawROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = true;
        }

        // roi - move
        private void moveModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
        }

        // roi - delete
        private void deleteROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
            //toolStripButton4.Checked = false;
            //toolStripButton5.Checked = true;
            imageView.DeletePOI_InArray();
            if (imageView.isCAM1Focused)
            {
                imageView.CAM1_POICheckFlag = true;
            }
            else if (imageView.isCAM2Focused)
            {
                imageView.CAM2_POICheckFlag = true;
            }
        }

        #endregion

        #region Menu_Help

        // help - about
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

        #region Control_ToolStripButton

        // New: Online button
        private void 새로만들기ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenNewDevice();
        }
       
        // Open simulation button
        private void 열기ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenIRDX();
        }

        // DrawPOI button
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = true;
            //toolStripButton4.Checked = true;
            //toolStripButton5.Checked = false;
        }

        // DeletePOI button
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
            //toolStripButton4.Checked = false;
            //toolStripButton5.Checked = true;
            imageView.DeletePOI_InArray();
            if (imageView.isCAM1Focused)
            {
                imageView.CAM1_POICheckFlag = true;
            }
            else if (imageView.isCAM2Focused)
            {
                imageView.CAM2_POICheckFlag = true;
            }
            //imageView.CAM1_POICount--;
        }

        // LogStart button
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DeviceLoggingStart();
            toolStripButton6.Enabled = false;
            toolStripButton7.Enabled = true;
        }

        // logstop button
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            DeviceLoggingStop();
            toolStripButton6.Enabled = true;
            toolStripButton7.Enabled = false;
        }

        // MovePOI
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CAM1 Focus = " + imageView.isCAM1Focused.ToString()
                + "\nCAM2 Focus = " + imageView.isCAM2Focused.ToString());
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            //if (propertyGrid1.Enabled == true) propertyGrid1.Enabled = false;
            //else if (propertyGrid1.Enabled == false) propertyGrid1.Enabled = true;

            //if (toolStripButton6.Enabled == true) toolStripButton6.Enabled = false;
            //else if (toolStripButton6.Enabled == false) toolStripButton6.Enabled = true;
            ushort c1_av=0, c2_av=0;
            DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_Array[0], ref c1_av);
            DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_Array[1], ref c2_av);

            MessageBox.Show("CAMERA #1 = " + c1_av.ToString() + "\nCAMERA #2 = " + c2_av.ToString());
        }

        #endregion

        #region Thread

        public void Pause()
        {
            _pauseEvent.Reset();
        }

        public void Resume()
        {
            _pauseEvent.Set();
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void run_two()  // Current: 512N
        {
            bool newDataReady = false;

            while (true)
            {
                Thread.Sleep(10);

                if (DIASDAQ.DDAQ_DEVICE_GET_NEWDATAREADY(1, ref newDataReady) != DIASDAQ.DDAQ_ERROR.NO_ERROR)       /// 카메라가 새로운 데이터를 받을 준비가 되었을 시
                {
                    DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                    return;
                }
                if (newDataReady && !isClosing)
                {
                    if (DIASDAQ.DDAQ_DEVICE_DO_UPDATEDATA(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                        /// 새로운 데이터가 있으며 종료의 명령이 없을 시
                        return;                                                                                     /// UpDate Data !!

                    //Bitmap bmp = GET_BITMAP(pIRDX_Array[1]);// GET_BITMAP_TOW(hIRDX_two);                                                         /// 카메라의 핸들값을 넘겨 화면에 뿌려줌
                    //bmp = new Bitmap((Image)bmp, new Size(c2_img.pictureBox1.Width, c2_img.pictureBox1.Height));
                    //Image img = c2_img.pictureBox1.Image;
                    //c2_img.pictureBox1.Image = (Image)bmp;

                    //if (img != null) img.Dispose();                                                                 /// 메모리 관리를 위하여 Dispose.

                    imageView.CalculateCurrentTemp(pIRDX_Array[1], imageView.CAM2_POICount, imageView.CAM2_ClickedPosition, imageView.CAM2_TemperatureArr);
                    //imageView.CAM2_DrawImage(pIRDX_Array[1], c2_img.pictureBox1, imageView.CAM2_ClickedPosition, imageView.CAM2_POICount);
                    //imageView.Test_DrawImage(pIRDX_Array[1], c2_img.pictureBox1, imageView.CAM2_ClickedPosition, imageView.CAM2_POICount);
                    imageView.CAM2_DrawImage(pIRDX_Array[1], c2_img.pictureBox1, imageView.CAM2_ClickedPosition, imageView.CAM2_POICount);

                    c2_chartView.UpdateData();
                    c2_gridView.CAM2_RefreshGrid();
                    //result.DetectTempThreshold();
                    result.CAM2_DetectTempThreshold();

                    CAM2_CompareMaxTemperature(imageView.CAM2_TemperatureArr);

                    if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                    /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                        return;
                }

            }
        }


        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void run()  // Current: 320L
        {
            bool newDataReady = false;
            
            while (true)
            //while(thr1_waitHandle.WaitOne())
            {
                Thread.Sleep(10);

                if (DIASDAQ.DDAQ_DEVICE_GET_NEWDATAREADY(DetectedDevices, ref newDataReady) != DIASDAQ.DDAQ_ERROR.NO_ERROR)/// 카메라가 새로운 데이터를 받을 준비가 되었을 시
                {
                    DIASDAQ.DDAQ_DEVICE_DO_STOP(DetectedDevices);
                    return;
                }

                if (newDataReady && !isClosing)
                {
                    if (DIASDAQ.DDAQ_DEVICE_DO_UPDATEDATA(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                 /// 새로운 데이터가 있으며 종료의 명령이 없을 시
                        return;                                                                                                /// UpDate Data !!

                    imageView.CalculateCurrentTemp(pIRDX_Array[0], imageView.CAM1_POICount, imageView.CAM1_ClickedPosition, imageView.CAM1_TemperatureArr);
                    imageView.DrawImage(pIRDX_Array[0], c1_img.pictureBox1);
                    //if (img != null) img.Dispose();                                                                            /// 메모리 관리를 위하여 Dispose.
                    //c1_chartView.UpdateData();
                    ////c1_gridView.ShowTemperatureOnGrid();
                    //c1_gridView.RefreshGrid();
                    //result.CAM1_DetectTempThreshold();
                    ////customGrid.SafeRefresh_PropertyGrid();
                    //customGrid.SafeRefresh_Time();
                    //if (!CAM1_UpdateThr.IsAlive)
                    //{
                    //    CAM1_UpdateThr.Start();
                    //}
                    //else if (CAM1_UpdateThr.IsAlive)
                    //{
                    //    Resume();
                    //}

                    c1_chartView.UpdateData();
                    c1_gridView.RefreshGrid();
                    //result.DetectTempThreshold();
                    result.CAM1_DetectTempThreshold();

                    CompareMaxTemperature(imageView.CAM1_TemperatureArr);

                    acq_index++;

                    if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)             /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                        return;
                }
            }

        }

        private static Bitmap GET_BITMAP(IntPtr hIRDX)
        {
            IntPtr pbitsImage = new IntPtr();
            IntPtr bmiImage = new IntPtr();
            ushort width = 0, height = 0;
            if (DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(hIRDX, ref width, ref height, out pbitsImage, out bmiImage) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            {
                return null; // failure
            }

            MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus", BindingFlags.Static | BindingFlags.NonPublic);

            if (mi == null)
            {
                return null; // permission problem 
            }

            IntPtr pBmp = IntPtr.Zero;
            int status = DIASDAQ.GDIPLUS_GdipCreateBitmapFromGdiDib(bmiImage, pbitsImage, out pBmp);

            if ((status == 0) && (pBmp != IntPtr.Zero))
            {
                return (Bitmap)mi.Invoke(null, new object[] { pBmp }); // success 
            }
            else
            {
                return null; // failure
            }
        }

        private void run_update()
        {
            while (_pauseEvent.WaitOne())
            {
                c1_chartView.UpdateData();
                c1_gridView.RefreshGrid();
                result.CAM1_DetectTempThreshold();
                Pause();
                //Thread.Sleep(100);
                //_pauseEvent.WaitOne();
            }
        }



        #endregion

        #region ConfigurationControl

        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private void LoadConfiguration()
        {
            //customGrid.RawData_Location = Properties.Settings.Default.RawDataPath;
            //customGrid.ResultData_Location = Properties.Settings.Default.ResultDataPath;
            //customGrid.Threshold = Properties.Settings.Default.Threshold;
            //cMinTemp = Properties.Settings.Default.Minimum;
            //cMaxTemp = Properties.Settings.Default.Maximum;

            string value = "";
            value = ConfigurationManager.AppSettings["Emissivity"];
            customGrid.Emissivity = Convert.ToSingle(value);
            value = ConfigurationManager.AppSettings["Transmission"];
            customGrid.Transmission = Convert.ToSingle(value);
            value = ConfigurationManager.AppSettings["AmbientTemp"];
            customGrid.AmbientTemperature = Convert.ToSingle(value);

            value = ConfigurationManager.AppSettings["Maximum"];
            //customGrid.Maximum = Convert.ToSingle(value);
            cMaxTemp = Convert.ToSingle(value);
            value = ConfigurationManager.AppSettings["Minimum"];
            //customGrid.Minimun = Convert.ToSingle(value);
            cMinTemp = Convert.ToSingle(value);

            value = ConfigurationManager.AppSettings["RawData_Location"];
            customGrid.RawData_Location = value;
            value = ConfigurationManager.AppSettings["ResultData_Location"];
            customGrid.ResultData_Location = value;
            value = ConfigurationManager.AppSettings["Threshold"];
            customGrid.Threshold = Convert.ToSingle(value);
        }

        private void SaveConfiguration()
        {
            //Properties.Settings.Default.Threshold = customGrid.Threshold;
            config.AppSettings.Settings["Emissivity"].Value = customGrid.Emissivity.ToString();
            config.AppSettings.Settings["Transmission"].Value = customGrid.Transmission.ToString();
            config.AppSettings.Settings["AmbientTemp"].Value = customGrid.AmbientTemperature.ToString();
            config.AppSettings.Settings["Maximum"].Value = customGrid.Maximum.ToString();
            config.AppSettings.Settings["Minimum"].Value = customGrid.Minimun.ToString();
            config.AppSettings.Settings["RawData_Location"].Value = customGrid.RawData_Location;
            config.AppSettings.Settings["ResultData_Location"].Value = customGrid.ResultData_Location;
            config.AppSettings.Settings["Threshold"].Value = customGrid.Threshold.ToString();

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        #endregion

        #region DataLoggingControl

        private string GetNewDataFileName(int SaveType, IntPtr irdxHandle)
        {
            string newRawDataFolderName="";
            string newResultDataFolderName="";
            string strFileName="";
            uint id = 0;
            DIASDAQ.DDAQ_DEVICE_TYPE type = DIASDAQ.DDAQ_DEVICE_TYPE.NO;

            DIASDAQ.DDAQ_IRDX_DEVICE_GET_ID(irdxHandle, ref id, ref type);

            DateTime time = DateTime.Now;
            string currentTime = time.ToString("yyyyMMdd_HHmmss");
            string currentTime_DateOnly = time.ToString("yyMMdd");
            strFileName = "["+id.ToString()+"]";

            DirectoryInfo VerifyRawFolder = new DirectoryInfo(customGrid.RawData_Location);
            DirectoryInfo VerifyResultFolder = new DirectoryInfo(customGrid.ResultData_Location);

            bool isRawFolderExist = VerifyRawFolder.Exists;
            bool isResultFolderExist = VerifyResultFolder.Exists;

            //bool isRawFolderExist = File.Exists(customGrid.RawData_Location);
            //bool isResultFolderExist = File.Exists(customGrid.ResultData_Location);

            
            if(!isRawFolderExist || !isResultFolderExist)
            {
                string appPath = Application.StartupPath;
                switch (SaveType)
                {
                    case 0:
                        customGrid.RawData_Location = appPath;
                        customGrid.RawData_Location += "\\RawData";
                        Directory.CreateDirectory(customGrid.RawData_Location);
                        newRawDataFolderName = customGrid.RawData_Location + "\\" + currentTime_DateOnly;
                        Directory.CreateDirectory(newRawDataFolderName);
                        break;
                    case 1:
                        customGrid.RawData_Location = appPath;
                        customGrid.RawData_Location += "\\RawData";
                        Directory.CreateDirectory(customGrid.RawData_Location);
                        newRawDataFolderName = customGrid.RawData_Location + "\\" + currentTime_DateOnly;
                        Directory.CreateDirectory(newRawDataFolderName);
                        break;
                    case 2:
                        customGrid.ResultData_Location = appPath;
                        customGrid.ResultData_Location += "\\ResultData";
                        Directory.CreateDirectory(customGrid.ResultData_Location);
                        newResultDataFolderName = customGrid.ResultData_Location + "\\" + currentTime_DateOnly;
                        Directory.CreateDirectory(newResultDataFolderName);
                        break;
                }
            }
            if (isRawFolderExist)
            {
                //newRawDataFolderName = customGrid.RawData_Location + "\\" + currentTime;
                //Directory.CreateDirectory(newRawDataFolderName);
                newRawDataFolderName = customGrid.RawData_Location + "\\RawData";
                Directory.CreateDirectory(newRawDataFolderName);
                //newRawDataFolderName += currentTime_DateOnly;
                newRawDataFolderName = newRawDataFolderName + "\\" + currentTime_DateOnly;
                Directory.CreateDirectory(newRawDataFolderName);
            }
            if (isResultFolderExist)
            {
                //newResultDataFolderName = customGrid.ResultData_Location+ "\\" + currentTime;
                //Directory.CreateDirectory(newResultDataFolderName);
                newResultDataFolderName = customGrid.ResultData_Location + "\\ResultData";
                Directory.CreateDirectory(newResultDataFolderName);
                //newResultDataFolderName += currentTime_DateOnly;
                newResultDataFolderName = newResultDataFolderName + "\\" + currentTime_DateOnly;
                Directory.CreateDirectory(newResultDataFolderName);
            }

            switch (SaveType)
            {
                case 0:
                    strFileName = strFileName + currentTime + ".irdx";
                    strFileName = newRawDataFolderName + "\\" + strFileName;
                    break;
                case 1:
                    //strFileName = strFileName + time.ToString() + ".txt";
                    strFileName = strFileName + currentTime + ".txt";
                    strFileName = newRawDataFolderName + "\\" + strFileName;
                    //strFileName = "[" + id.ToString() + "]" + " Raw";
                    break;
                case 2:
                    strFileName = strFileName + currentTime + ".txt";
                    strFileName = newResultDataFolderName + "\\" + strFileName;
                    //strFileName = "[" + id.ToString() + "]" + " Result";
                    break;
            }

            return strFileName;
        }

        System.Windows.Forms.Timer LoggingTimer = new System.Windows.Forms.Timer();
        FileStream Text_RawData;
        FileStream Text_ResultData;
        StreamWriter outputFile;
        StreamWriter outputFile_Result;

        FileStream c2_Text_RawData;
        FileStream c2_Text_ResultData;
        StreamWriter c2_outputFile;
        StreamWriter c2_outputFile_Result;

        private void DeviceLoggingStart()
        {
            if (isLoggingRunning)
            {
                // 이미 데이터 로깅이 진행 중임
                MessageBox.Show("이미 진행 중인 Data Logging 세션이 존재합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isLoggingRunning)
            {
                isLoggingRunning = true;

                propertyGrid1.Enabled = false;      // Propertygrid Disable
                toolStripButton4.Enabled = false;   // Draw POI Disable
                toolStripButton8.Enabled = false;   // Move POI Disable
                toolStripButton5.Enabled = false;   // Delete POI Disable

                int IRDX = 0, RawData = 1, ResultData = 2;

                NewIRDXFileName = GetNewDataFileName(IRDX, pIRDX_Array[0]);
                newRawDataFileName = GetNewDataFileName(RawData, pIRDX_Array[0]);
                newResultDataFileName = GetNewDataFileName(ResultData, pIRDX_Array[0]);

                Text_RawData = new FileStream(newRawDataFileName, FileMode.Append, FileAccess.Write);
                Text_ResultData = new FileStream(newResultDataFileName, FileMode.Append, FileAccess.Write);

                outputFile = new StreamWriter(Text_RawData);
                outputFile_Result = new StreamWriter(Text_ResultData);

                //if (imageView.CAM1_POICount == 0)
                //{
                //    MessageBox.Show("저장할 데이터를 확인하세요.\n저장 프로세스가 시작되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    DeviceLoggingStop();
                //    return;
                //}

                string legend = "Index\t";
                for(int k=0; k<imageView.CAM1_POICount; k++)
                {
                    legend = legend + "POI #"+(k+1).ToString()+"\tTemperature\t";
                }
                outputFile_Result.WriteLine(legend);                

                // 카메라가 두대 붙어있으면 한번 더 하면 됨
                if (pIRDX_Array[1] != IntPtr.Zero)
                {
                    CAM2_NewIRDXFileName = GetNewDataFileName(IRDX, pIRDX_Array[1]);
                    CAM2_newRawDataFileName = GetNewDataFileName(RawData, pIRDX_Array[1]);
                    CAM2_newResultDataFileName = GetNewDataFileName(ResultData, pIRDX_Array[1]);

                    c2_Text_RawData = new FileStream(CAM2_newRawDataFileName, FileMode.Append, FileAccess.Write);
                    c2_Text_ResultData = new FileStream(CAM2_newResultDataFileName, FileMode.Append, FileAccess.Write);

                    c2_outputFile = new StreamWriter(c2_Text_RawData);
                    c2_outputFile_Result = new StreamWriter(c2_Text_ResultData);

                    string legend2 = "Index\t";
                    for (int k = 0; k < imageView.CAM2_POICount; k++)
                    {
                        legend2 = legend2 + "POI #" + (k + 1).ToString() + "\tTemperature\t";
                    }
                    c2_outputFile_Result.WriteLine(legend2);
                }
                
                LoggingTimer.Interval = 1000;   // ms 간격으로 tick 동작
                LoggingTimer.Tick += T_Tick;    // tick이 하나씩 돌때마다 T_Tick 함수 작동
                LoggingTimer.Start();
            }
        }

        IntPtr irdxHandle_write = new IntPtr();
        IntPtr c2_irdxHandle_write = new IntPtr();
        int tickCount = 0;

        private void T_Tick(object sender, EventArgs e)
        {
            string test = "TEXT WRITING TEST: RawData\n";
            string test2 = "TEXT WRITING TEST: ResultData\n";

            // 어떤 데이터를 Append해서 텍스트파일에 쓸 것인지 써주면 됨
            //outputFile.WriteLine(tickCount.ToString() + " " + test);
            //outputFile_Result.WriteLine(tickCount.ToString() + " " + test2);

            // index    poi #   temp    poi #   temp    ...
            test2 = tickCount.ToString();
            for(int i=0; i<imageView.CAM1_POICount; i++)
            {
                test2 = test2 + "\t\t" + imageView.CAM1_TemperatureArr[i];
            }
            outputFile_Result.WriteLine(test2);

            if (irdxHandle_write == IntPtr.Zero) // write용 irdxHandle이 없으면 새로 만들고
                DIASDAQ.DDAQ_IRDX_FILE_OPEN_WRITE(NewIRDXFileName, true, ref irdxHandle_write);
            else if (irdxHandle_write != IntPtr.Zero)   // 이미 있으면 이어붙이면 됨
                DIASDAQ.DDAQ_IRDX_FILE_ADD_IRDX(irdxHandle_write, pIRDX_Array[0]);

            // 카메라가 두대 붙어있으면 똑같이 더하면 됨
            if (pIRDX_Array[1] != IntPtr.Zero)
            {
                //c2_outputFile.WriteLine(tickCount.ToString() + " " + test);
                //c2_outputFile_Result.WriteLine(tickCount.ToString() + " " + test2);
                test2 = tickCount.ToString();
                for(int i=0; i < imageView.CAM2_POICount; i++)
                {
                    test2 = test2 + "\t\t" + imageView.CAM2_TemperatureArr[i];
                }
                c2_outputFile_Result.WriteLine(test2);
                if (c2_irdxHandle_write == IntPtr.Zero)
                    DIASDAQ.DDAQ_IRDX_FILE_OPEN_WRITE(CAM2_NewIRDXFileName, true, ref c2_irdxHandle_write);
                else if (c2_irdxHandle_write != IntPtr.Zero)
                    DIASDAQ.DDAQ_IRDX_FILE_ADD_IRDX(c2_irdxHandle_write, pIRDX_Array[1]);
            }

            tickCount++;
        }

        

        private void DeviceLoggingStop()
        {
            isLoggingRunning = false;
            LoggingTimer.Stop();    // tick timer 정지 시키고
            DIASDAQ.DDAQ_IRDX_FILE_CLOSE(irdxHandle_write); // 쓰고있던 irdx파일 닫고
            DIASDAQ.DDAQ_IRDX_FILE_CLOSE(c2_irdxHandle_write);
            irdxHandle_write = IntPtr.Zero;     // 쓰기용 irdx handle 초기화
            c2_irdxHandle_write = IntPtr.Zero;
            tickCount = 0;  // tick count 초기화

            propertyGrid1.Enabled = true;       // Propertygrid Enable
            toolStripButton4.Enabled = true;   // Draw POI Enable
            toolStripButton8.Enabled = true;   // Mobe POI Enable
            toolStripButton5.Enabled = true;   // Delete POI Enable

            // 텍스트파일 다 썼으니까 닫자
            outputFile.Close();
            outputFile_Result.Close();
            Text_RawData.Close();
            Text_ResultData.Close();

            c2_outputFile.Close();
            c2_outputFile_Result.Close();
            c2_Text_RawData.Close();
            c2_Text_ResultData.Close();
        }

        #endregion

        public void CompareMaxTemperature(float[] TemperatureArray)
        {
            float FloatMaxTemp = 0.0f;
            string MaxTemp = "";
            //if (imageView.CAM1_TemperatureArr)
            //for(int i=0; i<TemperatureArray.Length-1; i++)
            for(int i=0; i<imageView.CAM1_POICount; i++)
            {
                if (FloatMaxTemp < TemperatureArray[i])
                {
                    FloatMaxTemp = TemperatureArray[i];
                }
            }
            MaxTemp = FloatMaxTemp.ToString();
            textBox3.Text = MaxTemp;
        }

        public void CAM2_CompareMaxTemperature(float[] TemperatureArray)
        {
            float c2_FloatMaxTemp = 0.0f;
            string MaxTemp = "";
            //if (imageView.CAM1_TemperatureArr)
            for (int i = 0; i < imageView.CAM2_POICount;  i++)
            {
                if (c2_FloatMaxTemp < TemperatureArray[i])
                {
                    c2_FloatMaxTemp = TemperatureArray[i];
                }
            }
            MaxTemp = c2_FloatMaxTemp.ToString();
            textBox4.Text = MaxTemp;
        }


        private void DetectOPCServer()
        {
            try
            {
                DaServerMgt daServerMgt = new DaServerMgt();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            ServerIdentifier[] availableOPCServers;

            int activeServerSubscriptionHandle;
            int activeClientSubscriptionHandle;

            OpcServerEnum opcServerEnum = new OpcServerEnum();
            String nodeName = "localhost";

            bool returnAllServers = false;
            ServerCategory[] serverCategories = { ServerCategory.OPCDA };

            try
            {
                // 연결 가능한 OPC 서버 리스트를 받아옴
                opcServerEnum.EnumComServer(nodeName, returnAllServers, serverCategories, out availableOPCServers);

                // Detection된 OPC Server가 있으면 연결함
                if (availableOPCServers.GetLength(0) > 0)
                {
                    // Enter the sequence that connect to OPC
                }
                else // 없으면 말고
                {
                    MessageBox.Show(nodeName + "에 연결할 수 있는 OPC 서버가 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("선택한 OPC 서버 연결이 실패하였습니다.\nError Message: ", ex.Message);
            }
        }

    }
}