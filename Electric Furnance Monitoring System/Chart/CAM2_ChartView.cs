using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{
    public partial class CAM2_ChartView : Form
    {
        MainForm main;
        ImageView imgView;
        int axisX_Count = 0;


        public CAM2_ChartView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
            this.CreateControl();

            imgView = (ImageView)main.ImageView_forPublicRef();
        }

        public void UpdateData()
        {
            if (imgView.CAM2_POICheckFlag)
            {
                CheckCurrentCount(imgView.CAM2_compPOICount, imgView.CAM2_POICount);
                imgView.CAM2_POICheckFlag = false;
            }
            if (imgView.CAM2_POICount != 0)
            {
                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    if (axTChart1.SeriesCount < imgView.CAM2_POICount) return;
                    if (axTChart1.Series(i).XValues.Count > 50)
                    {
                        axTChart1.Series(i).Delete(0);
                    }
                    axTChart1.Series(i).AddXY(axisX_Count, imgView.CAM2_TemperatureArr[i], null, 0);
                    //if (axisX_Count > 50)
                    //{
                    //    axTChart1.Series(i).Delete(0);
                    //}
                }
                axisX_Count++;
            }
        }

        public void CheckCurrentCount(int POICount, int currentPOICount)
        {
            if (POICount < currentPOICount)
            {
                for (int i = POICount; i < currentPOICount; i++)
                {
                    string str = (i + 1).ToString();
                    axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
                    axTChart1.Series(i).Title = str;
                    axTChart1.Series(i).LegendTitle = str;
                }
                imgView.CAM2_compPOICount = imgView.CAM2_POICount;
            }
            else if (POICount > currentPOICount)
            {
                for (int i = POICount - 1; i >= currentPOICount; i--)
                {
                    axTChart1.Series(i).Clear();
                    axTChart1.RemoveSeries(i);
                }
                imgView.CAM2_compPOICount = imgView.CAM2_POICount;
            }
        }

    }
}
