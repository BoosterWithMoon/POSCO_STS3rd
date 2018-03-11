using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Electric_Furnance_Monitoring_System
{
    class ImageView
    {
        // picturebox 위에서 마우스 온도값을 보여줄 위치 설정
        public enum TemperatureShowLocation : ushort
        {
            DEFAULT = 0,
            QUADRANT_ONE = 1,
            QUADRANT_TWO = 2,
            QUADRANT_THREE = 3,
            QUADRANT_FOUR = 4
        }

        MainForm main;
        CAM1_ImageView c1_imgView;
        CAM2_ImageView c2_imgView;
        public Image img;
        public Bitmap bmp;
        public Bitmap CAM2_bmp;
        public string pointTemperatureData = "";
        public string CAM2_pointTemperatureData = "";
        public TemperatureShowLocation TextLocation = TemperatureShowLocation.DEFAULT;

        public Bitmap backbuffer;

        private Rectangle oldRect, currentRect;

        public Graphics g;
        public Graphics CAM2_g;

        public ImageView(MainForm _main)
        {
            this.main = _main;
        }

        #region Variables

        public ushort m_bmp_isize_x = 0;    // real bmp image x
        public ushort m_bmp_isize_y = 0;    // real bmp image y
        public ushort c2_m_bmp_isize_x = 0;
        public ushort c2_m_bmp_isize_y = 0;

        public int m_bmp_size_x = 0;        // stretched bmp image x
        public int m_bmp_size_y = 0;        // stretched bmp image y
        public int c2_m_bmp_size_x = 0;
        public int c2_m_bmp_size_y = 0;

        // offsets
        public int m_bmp_ofs_x = 0;
        public int m_bmp_ofs_y = 0;
        public int c2_m_bmp_ofs_x = 0;
        public int c2_m_bmp_ofs_y = 0;

        // zoom
        public float m_bmp_zoom = 0.0f;
        public float c2_m_bmp_zoom = 0.0f;

        public int CAM1_POICount = 0;
        public int CAM1_compPOICount = 0;
        public bool CAM1_POICheckFlag = false;
        public Point[] CAM1_ClickedPosition = new Point[10];
        public float[] CAM1_TemperatureArr = new float[10];

        public int CAM2_POICount = 0;
        public int CAM2_compPOICount = 0;
        public bool CAM2_POICheckFlag = false;
        public Point[] CAM2_ClickedPosition = new Point[10];
        public float[] CAM2_TemperatureArr = new float[10];

        public bool isCAM1Focused = false;
        public bool isCAM2Focused = false;

        #endregion


        // 화면 깜빡임 현상 제거를 위한 테스트 함수
        //delegate void RefreshImage(Control ctrl, Image img);
        //public void SafeRefresh(Control ctrl, Image img)
        //{
        //    if (ctrl.InvokeRequired)
        //    {
        //        ctrl.Invoke(new RefreshImage(SafeRefresh), ctrl, img);
        //    }
        //    else
        //    {
        //        ctrl.BackgroundImage = img;
        //    }
        //}

        // 열화상 이미지 Zoom 계산
        public void CalculateImageZoom(IntPtr irdxHandle, PictureBox p)
        {
            int wnd_sizex = p.Width;
            int wnd_sizey = p.Height;

            IntPtr ppbits = new IntPtr();
            IntPtr ppbitmapinfo = new IntPtr();

            DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(irdxHandle, ref m_bmp_isize_x, ref m_bmp_isize_y, out ppbits, out ppbitmapinfo);

            float zoomx = (float)wnd_sizex / (float)m_bmp_isize_x;
            float zoomy = (float)wnd_sizey / (float)m_bmp_isize_y;

            m_bmp_zoom = (((zoomx) < (zoomy)) ? (zoomx) : (zoomy));
            if (m_bmp_zoom < 0.1f) m_bmp_zoom = 0.1f;

            m_bmp_size_x = (int)(m_bmp_zoom * m_bmp_isize_x);
            m_bmp_size_y = (int)(m_bmp_zoom * m_bmp_isize_y);

            m_bmp_ofs_x = (int)((wnd_sizex - m_bmp_size_x) / 2.0);
            m_bmp_ofs_y = (int)((wnd_sizey - m_bmp_size_y) / 2.0);


        }

        // picturebox 상의 상대 좌표 계산
        public ushort ux = 0, uy = 0;
        public ushort c2_ux = 0, c2_uy = 0;
        public void CalculatePoint(IntPtr irdxHandle, Point p)
        {
            if (irdxHandle == main.pIRDX_Array[0])
            {
                float fx = (float)p.X - (float)m_bmp_ofs_x;
                float fy = (float)p.Y - (float)m_bmp_ofs_y;

                fx /= m_bmp_zoom;
                fy /= m_bmp_zoom;

                DIASDAQ.DDAQ_ZMODE zoomMode = (ushort)DIASDAQ.DDAQ_ZMODE.DIRECT;
                float zoom = 1.0f;

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_ZOOM(irdxHandle, ref zoomMode, ref zoom);
                if (zoomMode > DIASDAQ.DDAQ_ZMODE.DIRECT)
                {
                    fx /= zoom;
                    fy /= zoom;
                }
                ux = Convert.ToUInt16((ushort)fx + 1);
                uy = Convert.ToUInt16((ushort)fy + 1);
            }

            else if (irdxHandle == main.pIRDX_Array[1])
            {
                float fx = (float)p.X - (float)c2_m_bmp_ofs_x;
                float fy = (float)p.Y - (float)c2_m_bmp_ofs_y;

                fx /= c2_m_bmp_zoom;
                fy /= c2_m_bmp_zoom;

                DIASDAQ.DDAQ_ZMODE zoomMode = (ushort)DIASDAQ.DDAQ_ZMODE.DIRECT;
                float zoom = 1.0f;

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_ZOOM(irdxHandle, ref zoomMode, ref zoom);
                if (zoomMode > DIASDAQ.DDAQ_ZMODE.DIRECT)
                {
                    fx /= zoom;
                    fy /= zoom;
                }
                c2_ux = Convert.ToUInt16((ushort)fx + 1);
                c2_uy = Convert.ToUInt16((ushort)fy + 1);
            }
        }

        // 실제 이미지를 뿌려주는 함수
        public void DrawImage(IntPtr hIRDX, PictureBox pb)
        {
            if (hIRDX == IntPtr.Zero) return;
            else
            {
                //CalculateImageZoom(hIRDX, pb);
                // CalculateImageZoom
                int wnd_sizex = pb.Width;
                int wnd_sizey = pb.Height;

                IntPtr ppbits = new IntPtr();
                IntPtr ppbitmapinfo = new IntPtr();

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(hIRDX, ref m_bmp_isize_x, ref m_bmp_isize_y, out ppbits, out ppbitmapinfo);

                float zoomx = (float)wnd_sizex / (float)m_bmp_isize_x;
                float zoomy = (float)wnd_sizey / (float)m_bmp_isize_y;

                m_bmp_zoom = (((zoomx) < (zoomy)) ? (zoomx) : (zoomy));
                if (m_bmp_zoom < 0.1f) m_bmp_zoom = 0.1f;

                m_bmp_size_x = (int)(m_bmp_zoom * m_bmp_isize_x);
                m_bmp_size_y = (int)(m_bmp_zoom * m_bmp_isize_y);

                m_bmp_ofs_x = (int)((wnd_sizex - m_bmp_size_x) / 2.0);
                m_bmp_ofs_y = (int)((wnd_sizey - m_bmp_size_y) / 2.0);

                bmp = GET_BITMAP(hIRDX);
                //if (bmp == null) return;
                g = pb.CreateGraphics();
                Graphics temp = Graphics.FromImage(bmp);
                
                //temp.DrawString(pointTemperatureData, new Font("Arial", 10), Brushes.Black,
                //    new Point(Control.MousePosition.X-m_bmp_ofs_x, Control.MousePosition.Y-m_bmp_ofs_y));
                //pb.Invalidate(new Rectangle(0, 0, 100, 100));
                DrawPOI(hIRDX, pb, CAM1_ClickedPosition, CAM1_POICount, ref temp);

                Point MousePosTemp = c1_imgView.pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                temp.DrawString(pointTemperatureData+"℃", new Font("맑은 고딕", 10), Brushes.Black, new Point((int)((MousePosTemp.X/m_bmp_zoom) - m_bmp_ofs_x + 5), (int)((MousePosTemp.Y/m_bmp_zoom) - m_bmp_ofs_y - 20)));

                g.DrawImage((Image)bmp, m_bmp_ofs_x, m_bmp_ofs_y, m_bmp_size_x, m_bmp_size_y);

                //OnMouseMove_ShowTemp(hIRDX, Control.MousePosition, pb);
            }
        }

        // POI 그리는 함수
        public void DrawPOI(IntPtr irdxHandle, PictureBox pb, Point[] position, /*float[] temp, */int poiCount, ref Graphics g)
        {
            c1_imgView = (CAM1_ImageView)main.CAM1_ImageView_forPublicRef();
            if (position[0].X <= 0 || position[0].Y <= 0) return;

            //int fShort = 1, fLong = 4;
            float fShort = 0.5f, fLong = 0.5f;

            for (int i = 0; i < poiCount; i++)
            {
                if (position[i].X == 0 && position[i].Y == 0) continue;

                float x1 = position[i].X - (fLong + 2);
                float y1 = position[i].Y - (fShort + 2);
                float x2 = position[i].X + (fLong + 2);
                float y2 = position[i].Y + (fShort + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X - (fShort + 2);
                y1 = position[i].Y - (fLong + 2);
                x2 = position[i].X + (fShort + 2);
                y2 = position[i].Y + (fLong + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X /** m_bmp_zoom*/ - (fShort + 1);
                y1 = position[i].Y /** m_bmp_zoom*/ - (fLong + 1);
                x2 = position[i].X /** m_bmp_zoom*/ + (fShort + 1);
                y2 = position[i].Y /** m_bmp_zoom*/ + (fLong + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X/* * m_bmp_zoom*/ - (fLong + 1);
                y1 = position[i].Y/* * m_bmp_zoom*/ - (fShort + 1);
                x2 = position[i].X/* * m_bmp_zoom*/ + (fLong + 1);
                y2 = position[i].Y/* * m_bmp_zoom*/ + (fShort + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));


            }
            Font f = new Font("맑은 고딕", 6);
            //CalculateCurrentTemp(irdxHandle, CAM1_POICount, CAM1_ClickedPosition, CAM1_TemperatureArr);
            for (int i = 0; i < poiCount; i++)
            {
                //g.DrawString(CAM1_TemperatureArr[i].ToString(), f, Brushes.Black, CAM1_ClickedPosition[i].X * m_bmp_zoom, CAM1_ClickedPosition[i].Y * m_bmp_zoom);
                g.DrawString("POI #"+(i+1)+" "+CAM1_TemperatureArr[i].ToString("N1")+"℃", f, Brushes.Black, position[i].X /** m_bmp_zoom*/, position[i].Y /** m_bmp_zoom*/);
            }
        }

        public void CAM2_DrawImage(IntPtr irdxHandle, PictureBox pb, Point[] clickedPos, int poiCnt)
        {
            if (irdxHandle == IntPtr.Zero) return;
            else
            {
                // CalculateImageZoom
                int wnd_sizex = pb.Width;
                int wnd_sizey = pb.Height;

                IntPtr ppbits = new IntPtr();
                IntPtr ppbitmapinfo = new IntPtr();

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(irdxHandle, ref c2_m_bmp_isize_x, ref c2_m_bmp_isize_y, out ppbits, out ppbitmapinfo);

                float zoomx = (float)wnd_sizex / (float)c2_m_bmp_isize_x;
                float zoomy = (float)wnd_sizey / (float)c2_m_bmp_isize_y;

                c2_m_bmp_zoom = (((zoomx) < (zoomy)) ? (zoomx) : (zoomy));
                if (c2_m_bmp_zoom < 0.1f) c2_m_bmp_zoom = 0.1f;

                c2_m_bmp_size_x = (int)(c2_m_bmp_zoom * c2_m_bmp_isize_x);
                c2_m_bmp_size_y = (int)(c2_m_bmp_zoom * c2_m_bmp_isize_y);

                c2_m_bmp_ofs_x = (int)((wnd_sizex - c2_m_bmp_size_x) / 2.0);
                c2_m_bmp_ofs_y = (int)((wnd_sizey - c2_m_bmp_size_y) / 2.0);

                CAM2_bmp = GET_BITMAP(irdxHandle);
                CAM2_g = pb.CreateGraphics();
                Graphics gTemp = Graphics.FromImage(CAM2_bmp);
                CAM2_DrawPOI(irdxHandle, pb, clickedPos, poiCnt, ref gTemp);

                if (c2_imgView.CAM2_isImageInPoint)
                {
                    Point CAM2_MousePosTemp = c2_imgView.pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    gTemp.DrawString(CAM2_pointTemperatureData + "℃", new Font("맑은 고딕", 10), Brushes.Black, new Point((int)((CAM2_MousePosTemp.X / m_bmp_zoom) - m_bmp_ofs_x + 5), (int)((CAM2_MousePosTemp.Y / m_bmp_zoom) - m_bmp_ofs_y - 20)));
                }
                else if (!c2_imgView.CAM2_isImageInPoint)
                {
                    gTemp.DrawString("", new Font("맑은 고딕",10), Brushes.Black, 0, 0);
                }
                //CAM2_g.DrawImage(CAM2_bmp, m_bmp_ofs_x, m_bmp_ofs_y, m_bmp_size_x, m_bmp_size_y);
                CAM2_g.DrawImage(CAM2_bmp, c2_m_bmp_ofs_x, c2_m_bmp_ofs_y, c2_m_bmp_size_x, c2_m_bmp_size_y);
            }
        }

        public void CAM2_DrawPOI(IntPtr irdxHandle, PictureBox pb, Point[] position, /*float[] temp, */int poiCount, ref Graphics g)
        {
            //c1_imgView = (CAM1_ImageView)main.CAM1_ImageView_forPublicRef();
            c2_imgView = (CAM2_ImageView)main.CAM2_ImageView_forPublicRef();
            if (position[0].X <= 0 || position[0].Y <= 0) return;

            //int pWidth = 1, fShort = 1, fLong = 4, xGap = 3, yGap = 5;
            float fShort = 0.5f, fLong = 0.5f;

            for (int i = 0; i < poiCount; i++)
            {
                if (position[i].X == 0 && position[i].Y == 0) continue;

                float x1 = position[i].X - (fLong + 2);
                float y1 = position[i].Y - (fShort + 2);
                float x2 = position[i].X + (fLong + 2);
                float y2 = position[i].Y + (fShort + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X - (fShort+ 2);
                y1 = position[i].Y - (fLong + 2);
                x2 = position[i].X + (fShort + 2);
                y2 = position[i].Y + (fLong + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X /** m_bmp_zoom*/ - (fShort + 1);
                 y1 = position[i].Y /** m_bmp_zoom*/ - (fLong + 1);
                 x2 = position[i].X /** m_bmp_zoom*/ + (fShort + 1);
                 y2 = position[i].Y /** m_bmp_zoom*/ + (fLong + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X/* * m_bmp_zoom*/ - (fLong + 1);
                y1 = position[i].Y/* * m_bmp_zoom*/ - (fShort + 1);
                x2 = position[i].X/* * m_bmp_zoom*/ + (fLong + 1);
                y2 = position[i].Y/* * m_bmp_zoom*/ + (fShort + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));
            }
            Font f = new Font("맑은 고딕", 6);
            for (int i = 0; i < poiCount; i++)
            {
                g.DrawString("POI #" + (i + 1) + " " + CAM2_TemperatureArr[i].ToString("N1")+"℃", f, Brushes.Black, position[i].X /** m_bmp_zoom*/, position[i].Y /** m_bmp_zoom*/);
            }
        }

        // 현재 온도값을 계산(스레드에 들어가있는데 빠져도 무방할 듯)
        public void CalculateCurrentTemp(IntPtr irdxHandle, int POICount, Point[] clickedPos, float[] tempArray)
        {
            for (int i = 0; i < POICount; i++)
            {
                float temp = 0.0f;
                ushort tempX = (ushort)clickedPos[i].X;
                ushort tempY = (ushort)clickedPos[i].Y;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(irdxHandle, tempX, tempY, ref temp);
                tempArray[i] = temp;
            }
        }

        // POI 삭제 버튼을 눌렀을 때
        public void DeletePOI_InArray()
        {
            if (isCAM1Focused)
            {
                if (CAM1_POICount == 0) return;
                CAM1_POICount--;
                CAM1_ClickedPosition[CAM1_POICount] = Point.Empty;
                CAM1_TemperatureArr[CAM1_POICount] = 0;
            }
            else if (isCAM2Focused)
            {
                if (CAM2_POICount == 0) return;
                CAM2_POICount--;
                CAM2_ClickedPosition[CAM2_POICount] = Point.Empty;
                CAM2_TemperatureArr[CAM2_POICount] = 0;
            }
        }

        // 깜빡임 현상 제거를 위해 테스트 중인 함수(MainForm의 DllImport (MainForm #129) 참조)
        /*public void InvalidateTextRect(PictureBox pb, Point p, int x1, int y1, int x2, int y2)
        {
            //DLLIMPORT.InvalidateRect(ref oldRect, 0, true);
            //IntPtr pbHandle = Control.FromHandle(pb.Handle);
            IntPtr pbHandle = pb.Handle;

            Rectangle invalidateArea = new Rectangle(x1, y1, x2, y2);

            main.PaintWindow(pbHandle);
        }
        */

        // (bitmap을 카메라로부터 얻어옴)
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

        //public void Test_DrawImage(IntPtr irdxHandle, PictureBox pb, Point[] clickedPos, int poiCnt)
        //{
        //    if (irdxHandle == IntPtr.Zero) return;
        //    else if (irdxHandle == main.pIRDX_Array[0])
        //    {
        //        CalculateImageZoom(irdxHandle, pb);
        //        bmp = GET_BITMAP(irdxHandle);
        //        g = pb.CreateGraphics();
        //        Graphics temp = Graphics.FromImage(bmp);

        //        DrawPOI(irdxHandle, pb, CAM1_ClickedPosition, CAM1_POICount, ref temp);
        //        g.DrawImage((Image)bmp, m_bmp_ofs_x, m_bmp_ofs_y, m_bmp_size_x, m_bmp_size_y);
        //    }
        //    else if (irdxHandle == main.pIRDX_Array[1])
        //    {
        //        CalculateImageZoom(irdxHandle, pb);

        //        c2_m_bmp_zoom = m_bmp_zoom;
        //        c2_m_bmp_ofs_x  = m_bmp_ofs_x;
        //        c2_m_bmp_ofs_y = m_bmp_ofs_y;
        //        c2_m_bmp_size_x = m_bmp_size_x;
        //        c2_m_bmp_size_y = m_bmp_size_y;

        //        CAM2_bmp = GET_BITMAP(irdxHandle);
        //        CAM2_g = pb.CreateGraphics();
        //        Graphics gTemp = Graphics.FromImage(CAM2_bmp);

        //        CAM2_DrawPOI(irdxHandle, pb, CAM2_ClickedPosition, CAM2_POICount, ref gTemp);
        //        CAM2_g.DrawImage((Image)CAM2_bmp, c2_m_bmp_ofs_x, c2_m_bmp_ofs_y, c2_m_bmp_size_x, c2_m_bmp_size_y);
        //    }
        //}

    }
}