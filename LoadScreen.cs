using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_MIDI
{
    public partial class LoadScreen : Form
    {
        public LoadScreen()
        {
            InitializeComponent();
            this.Refresh();
            timer1.Tick += startCheckTick;
            timer1.Interval = 500;
            timer1.Start();
        }
        private void startCheckTick(object sender, EventArgs e)
        {
            if(Program.mainform.loadCompleted)
            {
                timer1.Stop();
                this.Close();
            }
        }
    }
}
