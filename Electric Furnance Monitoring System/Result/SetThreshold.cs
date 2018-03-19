using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_System
{
    public partial class SetThreshold : Form
    {
        MainForm main;
        ResultView result;
        ImageView imgView;
        TextBox[] CAM1_Threshold;
        TextBox[] CAM2_Threshold;

        //public SetThreshold(ResultView resultView)
        public SetThreshold(MainForm _main)
        {
            this.main = _main;
            //this.result = resultView;
            InitializeComponent();

            imgView = (ImageView)main.ImageView_forPublicRef();
            CAM1_Threshold = new TextBox[10];
            CAM2_Threshold = new TextBox[10];
            ConnectionTextbox();
            
        }

        private void SetThreshold_Load(object sender, EventArgs e)
        {
            result = (ResultView)main.ResultView_forPublicRef();
            for(int i=0; i<10; i++)
            {
                CAM1_Threshold[i].Text = result.CAM1_ThresholdTemp[i].ToString();
                CAM2_Threshold[i].Text = result.CAM2_ThresholdTemp[i].ToString();
            }

            for (int i = 0; i < 10; i++)
            {
                CAM1_Threshold[i].Enabled = false;
                CAM2_Threshold[i].Enabled = false;
            }

            for (int i = 0; i < 10; i++)
            {
                if (imgView.CAM1_TemperatureArr[i] != 0)
                {
                    CAM1_Threshold[i].Enabled = true;
                }
                if (imgView.CAM2_TemperatureArr[i] != 0)
                {
                    CAM2_Threshold[i].Enabled = true;
                }
            }
        }

        private void ConnectionTextbox()
        {
            CAM1_Threshold[0] = textBox1;
            CAM1_Threshold[1] = textBox2;
            CAM1_Threshold[2] = textBox3;
            CAM1_Threshold[3] = textBox4;
            CAM1_Threshold[4] = textBox5;
            CAM1_Threshold[5] = textBox6;
            CAM1_Threshold[6] = textBox7;
            CAM1_Threshold[7] = textBox8;
            CAM1_Threshold[8] = textBox9;
            CAM1_Threshold[9] = textBox10;

            CAM2_Threshold[0] = textBox2_1;
            CAM2_Threshold[1] = textBox2_2;
            CAM2_Threshold[2] = textBox2_3;
            CAM2_Threshold[3] = textBox2_4;
            CAM2_Threshold[4] = textBox2_5;
            CAM2_Threshold[5] = textBox2_6;
            CAM2_Threshold[6] = textBox2_7;
            CAM2_Threshold[7] = textBox2_8;
            CAM2_Threshold[8] = textBox2_9;
            CAM2_Threshold[9] = textBox2_10;

            for (int k = 0; k < 10; k++)
            {
                // TextAlign 지정
                CAM1_Threshold[k].TextAlign = HorizontalAlignment.Center;
                CAM2_Threshold[k].TextAlign = HorizontalAlignment.Center;

                // Tempeorary Initialize Value
                //CAM1_Threshold[k].Text = 20.ToString();
                //CAM2_Threshold[k].Text = 20.ToString();
                //CAM1_Threshold[k].Text = result.CAM1_ThresholdTemp[0].ToString();

                CAM1_Threshold[k].Enabled = false;
                CAM2_Threshold[k].Enabled = false;
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            result = (ResultView)main.ResultView_forPublicRef();
            for (int k = 0; k < 10; k++)
            {
                result.CAM1_ThresholdTemp[k] = Convert.ToSingle(CAM1_Threshold[k].Text);
                result.CAM2_ThresholdTemp[k] = Convert.ToSingle(CAM2_Threshold[k].Text);
            }
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
