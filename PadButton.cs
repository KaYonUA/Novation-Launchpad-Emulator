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
    public delegate void pSetButtonHendler(int Key, int ID);
    public partial class PadButton : UserControl
    {
        public int ID = 0;
        public event pButtonHandler mouseDown;
        public event pButtonHandler mouseUp;

        public event pSetButtonHendler keyChanged;
        public PadButton()
        {
            InitializeComponent();
            this.label1.MouseDown += new MouseEventHandler(onLabelMouseDown);
            this.label1.MouseUp += new MouseEventHandler(onLabelMouseUp);
            this.textBox1.KeyDown += new KeyEventHandler(onEnterPress);
            this.setKeyToolStripMenuItem.Click += new EventHandler(onsetClick);
            this.resetToolStripMenuItem.Click += new EventHandler(onresetClick);
        }
        private void onresetClick(object sender, EventArgs e)
        {
            if (keyChanged != null)
                keyChanged(label1.Text[0], 0);
            this.label1.Text = "";
        }

        private void onsetClick(object sender, EventArgs e)
        {
            this.textBox1.Visible = true;
            this.textBox1.Focus();
        }
        private void onEnterPress(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 13)
            {
                if ((sender as TextBox).Text.Length == 1)
                {
                    label1.Text = (sender as TextBox).Text;
                    (sender as TextBox).Visible = false;
                    if (keyChanged != null)
                        keyChanged(label1.Text[0], ID+1);
                }
                else MessageBox.Show("Write one letter.", "Error");
            }
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
