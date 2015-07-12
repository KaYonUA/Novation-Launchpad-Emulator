using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Midi;

namespace GUI_MIDI
{
    class Program
    {
        public static PadForm mainform;

        [STAThread]
        public static void Main()
        {

            Application.EnableVisualStyles();
            mainform = new PadForm();
            if (mainform.midiin == null || mainform._launch == null)
            {
                MessageBox.Show("Driver not found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(mainform);
        }
    }
}
