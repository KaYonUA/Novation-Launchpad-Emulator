using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_MIDI
{
    public delegate void pButtonHandler(object sender, MouseEventArgs e);
    public partial class PadButton : UserControl
    {
        public int ID = 0;
        public event pButtonHandler mouseDown;
        public event pButtonHandler mouseUp;  
        public PadButton()
        {
            InitializeComponent();
            this.label1.MouseDown += new MouseEventHandler(onLabelMouseDown);
            this.label1.MouseUp += new MouseEventHandler(onLabelMouseUp);
        }

        private void onLabelMouseDown(object sender, MouseEventArgs e)
        {
            if (mouseDown != null)
                mouseDown(this,e);
            
        }
        private void onLabelMouseUp(object sender, MouseEventArgs e)
        {
            if (mouseUp != null)
                mouseUp(this, e);
        }
        public Label button
        {
            get
            {
                return label1;
            }
        }
        public ContextMenuStrip conMenStrip
        {
            get
            {
                return contextMenuStrip1;
            }
        }
    }
}
