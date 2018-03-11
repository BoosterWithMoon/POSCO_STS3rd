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

namespace Electric_Furnance_Monitoring_System
{
    public partial class CAM1_ChartView : Form
    {
        MainForm main;
        ImageView imgView;
        int axisX_Count = 0;
        private static int MAX_xCount = 50;


        public CAM1_ChartView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
            this.CreateControl();
            
            imgView = (ImageView)main.ImageView_forPublicRef();
        }

        public void SetTimeInverval(int second)
        {

        }

        //bool FullCount = false;
        public void UpdateData()
        {
            if (imgView.CAM1_POICheckFlag)
            {
                CheckCurrentCount(imgView.CAM1_compPOICount, imgView.CAM1_POICount);
                imgView.CAM1_POICheckFlag = false;
            }

            if (imgView.CAM1_POICount != 0)
            {
                //if (axisX_Count <= MAX_xCount)
                //{
                //    for (int i = 0; i < imgView.CAM1_POICount; i++)
                //    {
                //        axTChart1.Series(i).AddXY(axisX_Count, imgView.CAM1_TemperatureArr[i], null, 0);
                //    }
                //    axisX_Count++;
                //}
                //else
                //{
                //   for(int i=0; i<imgView.CAM1_POICount; i++)
                //    {
                //        if (axTChart1.Series(i).YValues.Value[0] == 0) break;
                //        axTChart1.Series(i).Delete(0);
                //        //for(int k=0; k<MAX_xCount; k++)
                //        //{
                //        //    axTChart1.Series(i).XValues.Value[k] = axTChart1.Series(i).XValues.Value[k + 1];
                //        //    axTChart1.Series(i).YValues.Value[k] = axTChart1.Series(i).YValues.Value[k + 1];
                //        //}
                //        axTChart1.Series(i).AddXY(axisX_Count, imgView.CAM1_TemperatureArr[i],null, 0);
                //    }
                   
                    
              
                //    axisX_Count++;
                //}
                for(int i=0; i<imgView.CAM1_POICount; i++)
                {
                    if (axTChart1.SeriesCount < imgView.CAM1_POICount) return;
                    if (axTChart1.Series(i).XValues.Count > MAX_xCount)
                    {
                        axTChart1.Series(i).Delete(0);
                    }
                    axTChart1.Series(i).AddXY(axisX_Count, imgView.CAM1_TemperatureArr[i], null, 0);
                }
                axisX_Count++;
            }

        }

        // series create & delete
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
                imgView.CAM1_compPOICount = imgView.CAM1_POICount;
            }
            else if (POICount > currentPOICount)
            {
                for(int i = POICount-1; i >= currentPOICount; i--)
                {
                    axTChart1.Series(i).Clear();
                    axTChart1.RemoveSeries(i);
                }
                imgView.CAM1_compPOICount = imgView.CAM1_POICount;
            }
        }

    }
}
