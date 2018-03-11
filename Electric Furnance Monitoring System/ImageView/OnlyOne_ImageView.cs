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
    public partial class OnlyOne_ImageView : Form
    {
        MainForm main;

        public OnlyOne_ImageView(MainForm _main)
        {
            InitializeComponent();
            this.main = _main;
        }
    }
}
