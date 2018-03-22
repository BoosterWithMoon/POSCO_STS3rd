using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{

    public partial class ResultView : Form
    {
        MainForm main;
        ImageView imgView;
        SystemPropertyGrid property;
        SetThreshold thresholdForm;
        //public bool[] CAM1_POIConnected = new bool[10];
        //public bool[] CAM2_POIConnected;
        public Label[] CAM1_LabelArray = new Label[10];
        public Color[] CAM1_POIConnected = new Color[10];
        public float[] CAM1_ThresholdTemp = new float[10];
        bool[] CAM1_verify = new bool[10];

        public Label[] CAM2_LabelArray = new Label[10];
        public Color[] CAM2_POIConnected = new Color[10];
        public float[] CAM2_ThresholdTemp = new float[10];
        bool[] CAM2_verify = new bool[10];

        public static Color NotConnected = Color.Yellow;
        public static Color Connected_NoWarning = Color.Green;
        public static Color Connected_Warning = Color.Red;


        public ResultView(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            ShowingAreaAdjust();

            CAM1_AlarmInitialize();
            CAM2_AlarmInitialize();
            imgView = (ImageView)main.ImageView_forPublicRef();
            property = (SystemPropertyGrid)main.customGrid_forPublicRef();
            //thresholdForm = (SetThreshold)main.SetThreshold_forPublicRef();
            thresholdForm = new SetThreshold(_main);

            // Initialize Threshold Temperature 
            //for (int i = 0; i < 10; i++)
            //{
            //    CAM1_ThresholdTemp[i] = 0.0f;
            //    CAM2_ThresholdTemp[i] = 0.0f;
            //}
            LoadConfiguration_Temperature();
        }

        private void ShowingAreaAdjust()
        {
            AlartToConnection.SplitterDistance = main.split_CAM1info.Panel1.Height + main.split_CAM1info.Panel2.Height + main.split_CAM1ChartGrid.Panel1.Height;
        }

        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private void LoadConfiguration_Temperature()
        {
            string value = "";
            string appSetValue = "";

            // CAM1 ThresholdTemp Load
            for (int i = 0; i < 10; i++)
            {
                appSetValue = "CAM1_Threshold" + (i + 1).ToString();
                value = ConfigurationManager.AppSettings[appSetValue];
                CAM1_ThresholdTemp[i] = Convert.ToSingle(value);
            }
            // CAM2 ThresholdTemp Load
            for (int i = 0; i < 10; i++)
            {
                appSetValue = "CAM2_Threshold" + (i + 1).ToString();
                value = ConfigurationManager.AppSettings[appSetValue];
                CAM2_ThresholdTemp[i] = Convert.ToSingle(value);
            }
        }

        public object SetThreshold_forPublicRef() { return thresholdForm; }

        public void CAM1_AlarmInitialize()
        {
            #region CAM1Alarm

            CAM1_LabelArray[0] = c1_poi1;
            CAM1_LabelArray[1] = c1_poi2;
            CAM1_LabelArray[2] = c1_poi3;
            CAM1_LabelArray[3] = c1_poi4;
            CAM1_LabelArray[4] = c1_poi5;
            CAM1_LabelArray[5] = c1_poi6;
            CAM1_LabelArray[6] = c1_poi7;
            CAM1_LabelArray[7] = c1_poi8;
            CAM1_LabelArray[8] = c1_poi9;
            CAM1_LabelArray[9] = c1_poi10;

            c1_MainAlarm.ForeColor = NotConnected;
            for (int i = 0; i < 10; i++)
            {
                CAM1_LabelArray[i].ForeColor = NotConnected;
                CAM1_verify[i] = false;
            }

            #endregion
        }

        public void CAM2_AlarmInitialize()
        {
            #region CAM2Alarm

            CAM2_LabelArray[0] = c2_poi1;
            CAM2_LabelArray[1] = c2_poi2;
            CAM2_LabelArray[2] = c2_poi3;
            CAM2_LabelArray[3] = c2_poi4;
            CAM2_LabelArray[4] = c2_poi5;
            CAM2_LabelArray[5] = c2_poi6;
            CAM2_LabelArray[6] = c2_poi7;
            CAM2_LabelArray[7] = c2_poi8;
            CAM2_LabelArray[8] = c2_poi9;
            CAM2_LabelArray[9] = c2_poi10;

            c2_MainAlarm.ForeColor = NotConnected;
            for (int i = 0; i < 10; i++)
            {
                CAM2_LabelArray[i].ForeColor = NotConnected;
                CAM2_verify[i] = false;
            }

            #endregion
        }

        public void CAM1_Connection()
        {
            if ((imgView.CAM1_POICount + 1) == 0) return;
            else
            {
                for (int i = 0; i < imgView.CAM1_POICount + 1; i++)
                {
                    CAM1_LabelArray[i].ForeColor = Connected_NoWarning;
                }
            }
        }

        public void CAM1_DetectTempThreshold()
        {
            if (imgView.CAM1_POICount != 0)
            {
                #region POIAlarm Control
                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    //if (imgView.CAM1_TemperatureArr[i] >= property.Threshold)
                    if(imgView.CAM1_TemperatureArr[i] >= CAM1_ThresholdTemp[i])
                    {
                        CAM1_verify[i] = true;
                        CAM1_LabelArray[i].ForeColor = Connected_Warning;
                    }
                    //else if (imgView.CAM1_TemperatureArr[i] < property.Threshold ||
                    else if(imgView.CAM1_TemperatureArr[i] < CAM1_ThresholdTemp[i] ||
                        imgView.CAM1_TemperatureArr[i] == 0)
                    {
                        CAM1_verify[i] = false;
                        CAM1_LabelArray[i].ForeColor = Connected_NoWarning;
                    }
                }

                for (int i = imgView.CAM1_POICount; i < 10; i++)
                {
                    CAM1_verify[i] = false;
                    CAM1_LabelArray[i].ForeColor = NotConnected;
                }

                #endregion

                #region MainAlarm Control
                if (CAM1_verify[0] == false && CAM1_verify[1] == false && CAM1_verify[2] == false && CAM1_verify[3] == false && CAM1_verify[4] == false &&
                    CAM1_verify[5] == false && CAM1_verify[6] == false && CAM1_verify[7] == false && CAM1_verify[8] == false && CAM1_verify[9] == false)
                {
                    c1_MainAlarm.ForeColor = Connected_NoWarning;
                }
                else
                {
                    c1_MainAlarm.ForeColor = Connected_Warning;
                }


                #endregion

            }

            // POI가 찍혀있었지만 모두 지워서 현재 화면상에 남아있는 POI가 없을 때에는
            else if (imgView.CAM1_POICount == 0)
            {
                // 제일 처음 초기화 상태로 되돌린다
                CAM1_AlarmInitialize();
            }


        }

        public void CAM2_DetectTempThreshold()
        {
            if (imgView.CAM2_POICount != 0)
            {

                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    //if (imgView.CAM2_TemperatureArr[i] >= property.Threshold)
                    if(imgView.CAM2_TemperatureArr[i] >= CAM2_ThresholdTemp[i])
                    {
                        CAM2_verify[i] = true;
                        CAM2_LabelArray[i].ForeColor = Connected_Warning;
                    }
                    //else if (imgView.CAM2_TemperatureArr[i] < property.Threshold ||
                    else if(imgView.CAM2_TemperatureArr[i] < CAM2_ThresholdTemp[i] ||
                        imgView.CAM2_TemperatureArr[i] == 0)
                    {
                        CAM2_verify[i] = false;
                        CAM2_LabelArray[i].ForeColor = Connected_NoWarning;
                    }
                }
                for (int i = imgView.CAM2_POICount; i < 10; i++)
                {
                    CAM2_verify[i] = false;
                    CAM2_LabelArray[i].ForeColor = NotConnected;
                }
                if (CAM2_verify[0] == false && CAM2_verify[1] == false && CAM2_verify[2] == false && CAM2_verify[3] == false && CAM2_verify[4] == false &&
                    CAM2_verify[5] == false && CAM2_verify[6] == false && CAM2_verify[7] == false && CAM2_verify[8] == false && CAM2_verify[9] == false)
                {
                    //c1_MainAlarm.ForeColor = Connected_NoWarning;
                    c2_MainAlarm.ForeColor = Connected_NoWarning;
                }
                else
                {
                    c2_MainAlarm.ForeColor = Connected_Warning;
                }

            }
            else if (imgView.CAM2_POICount == 0)
            {
                CAM2_AlarmInitialize();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*DialogResult dr = */thresholdForm.ShowDialog();
        }
    }
}