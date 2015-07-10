using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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

        private Timer editModeTimer;
        private bool editMode;
        public PadButton(int ID)
        {
            InitializeComponent();
            this.label1.MouseDown += new MouseEventHandler(onLabelMouseDown);
            this.label1.MouseUp += new MouseEventHandler(onLabelMouseUp);
            this.label1.PreviewKeyDown += new PreviewKeyDownEventHandler(onKeyDown);
            this.label1.DoubleClick += new EventHandler(onsetClick);

            this.label2.Text = (this.ID = ID).ToString();
            this.label2.BackColor = Color.Transparent;

            this.textBox1.KeyDown += new KeyEventHandler(onEnterPress);
            this.textBox1.LostFocus += new EventHandler(textBox1_LostFocus);

            this.setKeyToolStripMenuItem.Click += new EventHandler(onsetClick);
            this.resetToolStripMenuItem.Click += new EventHandler(onresetClick);

            this.editMode = false;
            this.editModeTimer = new Timer();
            this.editModeTimer.Interval = 350;
            this.editModeTimer.Tick += new EventHandler(editModeTimer_Tick);
        }

        [DllImport("user32")]
        static extern int MapVirtualKey(Keys uCode, int uMapType);
        const int MAPVK_VK_TO_CHAR = 2;

        private void onKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (editMode == true)
            {
                if (true)
                {
                    //MessageBox.Show(e.KeyValue.ToString());
                    this.editModeTimer.Stop();
                    this.editMode = false;
                        this.label1.Text = (char)(MapVirtualKey(e.KeyData, MAPVK_VK_TO_CHAR))+"";
                    if (this.keyChanged != null)
                        this.keyChanged(e.KeyValue, ID + 1);
                }
            }
        }
        private void editModeTimer_Tick(object sender, EventArgs e)
        {
            if (this.label1.Text == "")
                this.label1.Text = "-";
            else
                this.label1.Text = "";
        }

        void textBox1_LostFocus(object sender, EventArgs e)
        {
            this.label1.Text = (sender as TextBox).Text;
            this.textBox1.Enabled = false;
            this.textBox1.Visible = false;
            this.textBox1.Text = "";
            if (this.keyChanged != null)
                this.keyChanged(label1.Text[0], ID + 1);
        }
        private void onresetClick(object sender, EventArgs e)
        {
            if (this.keyChanged != null)
                this.keyChanged(label1.Text[0], 0);
            this.label1.Text = "";
            this.textBox1.Text = "";
        }

        private void onsetClick(object sender, EventArgs e)
        {
            //this.textBox1.Visible = true;
            //this.textBox1.Enabled = true;
           // this.textBox1.Focus();
            this.editMode = true;
            this.label1.Text = "";
            this.label1.Focus();
            this.editModeTimer.Start();

        }
        private void onEnterPress(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 13)
            {
                //if ((sender as TextBox).Text.Length == 1)
                //{
                    label1.Text = (sender as TextBox).Text;
                    (sender as TextBox).Enabled = false;
                    (sender as TextBox).Visible = false;
                    if (keyChanged != null)
                        keyChanged(label1.Text[0], ID+1);
                //}
                //else MessageBox.Show("Write one letter.", "Error");
            }
            else if (editMode == true)
            {
                editMode = false;
                editModeTimer.Stop();
                if (keyChanged != null)
                    keyChanged(e.KeyValue, ID + 1);
                label1.Text = e.KeyData.ToString();
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

        public Color color
        {
            set
            {
                this.label1.BackColor = this.label2.BackColor = value;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
