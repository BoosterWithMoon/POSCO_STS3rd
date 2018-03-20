using System;
using System.Drawing;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{
    public partial class CAM1_ImageView : Form
    {
        // 깜빡임 현상 해결 위해 만들어 놓은 double buffering :: 테스트 중
        public class DoubleBufferPanel : Panel
        {
            public DoubleBufferPanel()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
                this.UpdateStyles();
            }
        }

        public class DoubleBufferPictureBox : PictureBox
        {
            public DoubleBufferPictureBox()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
                this.UpdateStyles();
            }
        }

        MainForm main;
        ImageView imgView;
        //Point[] CAM1_POIArr = new Point[10];
        CAM1_ChartView c1_chart;
        CAM1_DataGridView c1_grid;
        ResultView result;
        public Point clickedPoint;
        public Point clickedAfterUp;
        public bool CAM1_isMouseButtonDown = false;
        public bool CAM1_PointMoveFlag = false;
        public bool CAM1_clicked = false;
        public bool CAM1_POIClicked = false;
        public int CAM1_pointIdx = 0;
        bool c1focused = false;

        public CAM1_ImageView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
            imgView = (ImageView)main.ImageView_forPublicRef();
            c1_chart = (CAM1_ChartView)main.CAM1_ChartView_forPublicRef();
            c1_grid = (CAM1_DataGridView)main.CAM1_GridView_forPublicRef();
            result = (ResultView)main.ResultView_forPublicRef();
        }


        Point temp;
        // picturebox위로 마우스가 돌아댕길때-

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Font f = new Font("맑은 고딕", 10);

            //Graphics g = e.Graphics;
            ////Graphics g = Graphics.FromImage(imgView.bmp);
            ////Graphics g = Graphics.FromHwnd(pictureBox1.Handle);

            //g.DrawString(imgView.pointTemperatureData, f, Brushes.Black, new Point(temp.X - imgView.m_bmp_ofs_x + 5, temp.Y - imgView.m_bmp_ofs_y - 20));
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            float DataPoint = 0.0f;
            imgView = (ImageView)main.ImageView_forPublicRef();

            if (imgView.m_bmp_zoom == 0) return;
            /*Point */
            temp = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            float currentZoom = imgView.m_bmp_zoom;

            // MousePosition에 대해 CalculatePoint
            float fx = (float)temp.X - (float)imgView.m_bmp_ofs_x;
            float fy = (float)temp.Y - (float)imgView.m_bmp_ofs_y;

            if (fx <= 0) fx = 0;
            if (fy <= 0) fy = 0;

            fx /= currentZoom;
            fy /= currentZoom;

            DIASDAQ.DDAQ_ZMODE zoomMode = (ushort)DIASDAQ.DDAQ_ZMODE.DIRECT;
            float zoom = 1.0f;

            DIASDAQ.DDAQ_IRDX_IMAGE_GET_ZOOM(main.pIRDX_Array[0], ref zoomMode, ref zoom);
            if (zoomMode > DIASDAQ.DDAQ_ZMODE.DIRECT)
            {
                fx /= zoom;
                fy /= zoom;
            }

            if ((fx >= 0.0f) && (fy >= 0.0f))
            {
                ushort ux = Convert.ToUInt16((ushort)fx + 1);
                ushort uy = Convert.ToUInt16((ushort)fy + 1);

                ushort sizex = 0, sizey = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(main.pIRDX_Array[0], ref sizex, ref sizey);

                if ((ux <= sizex) && (uy <= sizey))
                {
                    DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(main.pIRDX_Array[0], ux, uy, ref DataPoint);

                    if (DataPoint < 0) DataPoint = 0;
                    else
                    {
                        imgView.pointTemperatureData = DataPoint.ToString("N1");
                    }

                    if (CAM1_PointMoveFlag)
                    {
                        int tempX, tempY;

                        tempX = imgView.CAM1_ClickedPosition[CAM1_pointIdx].X - (clickedPoint.X - ux);
                        tempY = imgView.CAM1_ClickedPosition[CAM1_pointIdx].Y - (clickedPoint.Y - uy);

                        if (tempX > 0 && tempX <= 320 &&
                            tempY > 0 && tempY <= 240)
                        {
                            imgView.CAM1_ClickedPosition[CAM1_pointIdx].X -= (clickedPoint.X - ux);
                            imgView.CAM1_ClickedPosition[CAM1_pointIdx].Y -= (clickedPoint.Y - uy);
                        }
                        clickedPoint.X = ux;
                        clickedPoint.Y = uy;
                    }
                }
            }

            //Font f = new Font("맑은 고딕", 10);

            //Graphics g = pictureBox1.CreateGraphics();
            ////Graphics g = Graphics.FromImage(imgView.bmp);
            ////Graphics g = Graphics.FromHwnd(pictureBox1.Handle);

            //g.DrawString(imgView.pointTemperatureData, f, Brushes.Black, new Point(temp.X - imgView.m_bmp_ofs_x + 5, temp.Y - imgView.m_bmp_ofs_y - 20));

        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            // 현재 Logging이 동작 중이라면 POI 그리기/이동의 작업을 하지 않음
            if (main.isLoggingRunning) return;

            // 좌클릭 이외에 다른 버튼을 클릭 했을 때에는 아무것도 안함
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) { return; }

            clickedPoint = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            //MessageBox.Show(clickedPoint.X.ToString() + ", " + clickedPoint.Y.ToString());
            imgView.CalculatePoint(main.pIRDX_Array[0], clickedPoint);

            if (main.Activate_DrawPOI == true && imgView.CAM1_POICount < 10 &&
                clickedPoint.X > imgView.m_bmp_ofs_x && clickedPoint.Y > imgView.m_bmp_ofs_y &&
                clickedPoint.X < (imgView.m_bmp_ofs_x + imgView.m_bmp_size_x) && clickedPoint.Y < (imgView.m_bmp_ofs_y + imgView.m_bmp_size_y))
            {
                Point tempPoint = new Point();
                tempPoint.X = imgView.ux;
                tempPoint.Y = imgView.uy;

                CAM1_pointIdx = imgView.CAM1_POICount;

                clickedPoint.X = imgView.ux;
                clickedPoint.Y = imgView.uy;

                imgView.CAM1_ClickedPosition[imgView.CAM1_POICount].X = tempPoint.X;
                imgView.CAM1_ClickedPosition[imgView.CAM1_POICount].Y = tempPoint.Y;

                imgView.CAM1_POICount++;

                imgView.CAM1_POICheckFlag = true;
                if(main.currentOpenMode == MainForm.OpenMode.IRDX)
                {
                    imgView.CAM2_POICheckFlag = true;
                }

                CAM1_isMouseButtonDown = true;
                CAM1_PointMoveFlag = true;
                CAM1_POIClicked = true;
            }
            else if (imgView.CAM1_POICount > 0)
            {
                bool hit = false;

                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    if (imgView.CAM1_ClickedPosition[i].X - 4 < imgView.ux - 1 && imgView.CAM1_ClickedPosition[i].X + 4 > imgView.ux - 1 &&
                        imgView.CAM1_ClickedPosition[i].Y - 4 < imgView.uy - 1 && imgView.CAM1_ClickedPosition[i].Y + 4 > imgView.uy - 1)
                    {
                        CAM1_PointMoveFlag = true;
                        CAM1_POIClicked = true;
                        hit = true;

                        CAM1_pointIdx = i;

                        clickedPoint.X = imgView.ux;
                        clickedPoint.Y = imgView.uy;
                    }
                }
                if (!hit)
                    CAM1_POIClicked = false;
            }
            //MessageBox.Show(CAM1_POIClicked.ToString());
            if (main.currentOpenMode == MainForm.OpenMode.IRDX)
            {
                //c1_chart = (CAM1_ChartView)main.CAM1_ChartView_forPublicRef();
                //c1_grid = (CAM1_DataGridView)main.CAM1_GridView_forPublicRef();
                //result = (ResultView)main.ResultView_forPublicRef();
                //imgView.CalculateCurrentTemp(main.pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);
                //c1_chart.UpdateData();
                //c1_grid.RefreshGrid();
                //result.CAM1_DetectTempThreshold();
            }
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (main.rectROIDraw && clickedAfterUp==Point.Empty)
            {
                clickedAfterUp = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));

            }
            imgView.isCAM1Focused = true;
            imgView.isCAM2Focused = false;

            if (e.X > imgView.m_bmp_ofs_x && e.Y > imgView.m_bmp_ofs_y &&
                e.X < (imgView.m_bmp_ofs_x + imgView.m_bmp_size_x) && e.Y < (imgView.m_bmp_ofs_y + imgView.m_bmp_size_x))
            {
                CAM1_isMouseButtonDown = false;
                CAM1_POIClicked = false;
            }
            if (CAM1_PointMoveFlag)
            {
                CAM1_PointMoveFlag = false;
                clickedPoint.X = 0;
                clickedPoint.Y = 0;
            }
            //MessageBox.Show("ClickedPoint = " + clickedPoint.X.ToString() + "," + clickedPoint.Y.ToString() +
            //    "\nClickedAfterUp = " + clickedAfterUp.X.ToString() + "," + clickedAfterUp.Y.ToString());
        }
    }
}
