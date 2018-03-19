using System;
using System.Drawing;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{
    public partial class CAM2_ImageView : Form
    {
        MainForm main;
        ImageView imgView;
        //CAM2_DataGridView c2_grid;

        public Point CAM2_clickedPoint;
        public bool CAM2_isMouseButtonDown = false;
        public bool CAM2_PointMoveFlag = false;
        public bool CAM2_clicked = false;
        public bool CAM2_POIClicked = false;
        public int CAM2_pointIdx = 0;

        public bool CAM2_isImageInPoint = false;

        public CAM2_ImageView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
            imgView = (ImageView)main.ImageView_forPublicRef();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void CAM2_ImageView_Resize(object sender, EventArgs e)
        {
            //imgView.DrawImage(main.pIRDX_Array[1], pictureBox1);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (main.currentOpenMode == MainForm.OpenMode.IRDX)
            {
                return;
            }

            CAM2_isImageInPoint = false;

            float DataPoint = 0.0f;
            imgView = (ImageView)main.ImageView_forPublicRef();

            if (imgView.c2_m_bmp_zoom == 0) return;
            Point temp = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            float currentZoom = imgView.c2_m_bmp_zoom;

            float fx = (float)temp.X - (float)imgView.c2_m_bmp_ofs_x;
            float fy = (float)temp.Y - (float)imgView.c2_m_bmp_ofs_y;

            if (fx <= 0) fx = 0;
            if (fy <= 0) fy = 0;

            fx /= currentZoom;
            fy /= currentZoom;

            DIASDAQ.DDAQ_ZMODE zoomMode = (ushort)DIASDAQ.DDAQ_ZMODE.DIRECT;
            float zoom = 1.0f;

            DIASDAQ.DDAQ_IRDX_IMAGE_GET_ZOOM(main.pIRDX_Array[1], ref zoomMode, ref zoom);
            if (zoomMode > DIASDAQ.DDAQ_ZMODE.DIRECT)
            {
                fx /= zoom; fy /= zoom;
            }

            if ((fx >= 0.0f) && (fy >= 0.0f))
            {
                ushort ux = Convert.ToUInt16((ushort)fx + 1);
                ushort uy = Convert.ToUInt16((ushort)fy + 1);

                ushort sizex = 0, sizey = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(main.pIRDX_Array[1], ref sizex, ref sizey);
         
                if ((ux <= sizex) && (uy <= sizey))
                {
                    CAM2_isImageInPoint = true;
                    DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(main.pIRDX_Array[1], ux, uy, ref DataPoint);

                    if (DataPoint < 0) DataPoint = 0;
                    else
                    {
                        imgView.CAM2_pointTemperatureData = DataPoint.ToString("N1");
                    }

                    if (CAM2_PointMoveFlag)
                    {
                        int tempX, tempY;

                        tempX = imgView.CAM2_ClickedPosition[CAM2_pointIdx].X - (CAM2_clickedPoint.X - ux);
                        tempY = imgView.CAM2_ClickedPosition[CAM2_pointIdx].Y - (CAM2_clickedPoint.Y - uy);

                        if(tempX>0 && tempX<=320 &&
                            tempY>0 && tempY <= 240)
                        {
                            imgView.CAM2_ClickedPosition[CAM2_pointIdx].X -= (CAM2_clickedPoint.X - ux);
                            imgView.CAM2_ClickedPosition[CAM2_pointIdx].Y -= (CAM2_clickedPoint.Y - uy);
                        }
                        CAM2_clickedPoint.X = ux;
                        CAM2_clickedPoint.Y = uy;
                    }
                }
                else
                {
                    CAM2_isImageInPoint = false;
                }
            }

            //Font f = new Font("맑은 고딕", 10);

            //Graphics g = pictureBox1.CreateGraphics();
            //g.DrawString(imgView.CAM2_pointTemperatureData, f, Brushes.Black, new Point(temp.X-imgView.c2_m_bmp_ofs_x+5, temp.Y-imgView.c2_m_bmp_ofs_y-20));
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 현재 Logging이 동작 중이라면 POI 그리기/이동의 작업을 하지 않음
            if (main.isLoggingRunning) return;

            // 좌클릭 이외에 다른 버튼을 클릭 했을 때에는 아무것도 안함
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) { return; }

            //imgView.isCAM2Focused = pictureBox1.Focused;
            CAM2_clickedPoint = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            //MessageBox.Show(CAM2_clickedPoint.X.ToString() + ", " + CAM2_clickedPoint.Y.ToString());
            imgView.CalculatePoint(main.pIRDX_Array[1], CAM2_clickedPoint);

            if (main.Activate_DrawPOI == true && imgView.CAM2_POICount < 10 &&
                (CAM2_clickedPoint.X > imgView.c2_m_bmp_ofs_x && CAM2_clickedPoint.Y > imgView.c2_m_bmp_ofs_y &&
                (CAM2_clickedPoint.X < (imgView.c2_m_bmp_ofs_x + imgView.c2_m_bmp_size_x)) && (CAM2_clickedPoint.Y < (imgView.c2_m_bmp_ofs_y + imgView.c2_m_bmp_size_y))))
            {
                Point tempPoint = new Point();
                tempPoint.X = imgView.c2_ux;
                tempPoint.Y = imgView.c2_uy;

                CAM2_pointIdx = imgView.CAM2_POICount;

                CAM2_clickedPoint.X = imgView.c2_ux;
                CAM2_clickedPoint.Y = imgView.c2_uy;

                imgView.CAM2_ClickedPosition[imgView.CAM2_POICount].X = tempPoint.X;
                imgView.CAM2_ClickedPosition[imgView.CAM2_POICount].Y = tempPoint.Y;

                imgView.CAM2_POICount++;

                imgView.CAM2_POICheckFlag = true;

                CAM2_isMouseButtonDown = true;
                CAM2_PointMoveFlag = true;
                CAM2_POIClicked = true;

            }
            else if (imgView.CAM2_POICount > 0)
            {
                bool hit = false;

                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    if (imgView.CAM2_ClickedPosition[i].X-4 < imgView.c2_ux-1 && imgView.CAM2_ClickedPosition[i].X+4 > imgView.c2_ux- 1 &&
                        imgView.CAM2_ClickedPosition[i].Y-4 < imgView.c2_uy-1 && imgView.CAM2_ClickedPosition[i].Y+4 > imgView.c2_uy-1)
                    {
                        CAM2_PointMoveFlag = true;
                        CAM2_POIClicked = true;
                        hit = true;

                        CAM2_pointIdx = i;

                        CAM2_clickedPoint.X = imgView.c2_ux;
                        CAM2_clickedPoint.Y = imgView.c2_uy;
                    }
                }
                if (!hit)
                    CAM2_POIClicked = false;
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            imgView.isCAM2Focused = true;
            imgView.isCAM1Focused = false;

            if (e.X > imgView.c2_m_bmp_ofs_x && e.Y > imgView.c2_m_bmp_ofs_y &&
                e.X < (imgView.c2_m_bmp_ofs_x + imgView.c2_m_bmp_size_x) && e.Y < (imgView.c2_m_bmp_ofs_y + imgView.c2_m_bmp_size_x))
            {
                CAM2_isMouseButtonDown = false;
                CAM2_POIClicked = false;
            }
            if (CAM2_PointMoveFlag)
            {
                CAM2_PointMoveFlag = false;
                CAM2_clickedPoint.X = 0;
                CAM2_clickedPoint.Y = 0;
            }
        }
    }
}
