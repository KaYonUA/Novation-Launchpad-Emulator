using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Midi;
using TobiasErichsen.teVirtualMIDI;

namespace GUI_MIDI
{
    class Program
    {
        public static PadForm mainform;
        [STAThread]
        public static void Main()
        {
            try
            {
                Console.WriteLine("teVirtualMIDI C# loopback sample");
                //Console.WriteLine("using driver-version: " + TeVirtualMIDI.versionString);

                //TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);
                //port = new TobiasErichsen.teVirtualMIDI.TeVirtualMIDI("Launchpad Emulator");
            }
            catch (TeVirtualMIDIException e)
            {
                MessageBox.Show(e.Message);
            }
            Application.EnableVisualStyles();
            mainform = new PadForm();
            if (mainform.midiin == null || mainform.launchpad == null)
            {
                MessageBox.Show("Driver not found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(mainform);
           // port.shutdown();
        }
    }
}
