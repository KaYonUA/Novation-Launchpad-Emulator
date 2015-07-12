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
    public delegate void pSetButtonHendler(int newKey, int ID);
    public partial class PadButton : UserControl
    {
        public int ID = 0;

        public event pButtonHandler mouseDown;
        public event pButtonHandler mouseUp;
        public event pSetButtonHendler keyChanged;

        private Timer editModeTimer;
        private bool editmode;
        private string Keylabel;
        public PadButton(int ID)
        {
            InitializeComponent();
            this.label1.MouseDown += new MouseEventHandler(onLabelMouseDown);
            this.label1.MouseUp += new MouseEventHandler(onLabelMouseUp);
            this.label1.PreviewKeyDown += new PreviewKeyDownEventHandler(onKeyDown);
            this.label1.MouseDoubleClick += new MouseEventHandler(onMouseDoubleClick);
            this.label2.BackColor = Color.FromArgb(170, 170, 170);
            this.label1.BackColor = Color.FromArgb(170, 170, 170);
            this.label1.Text =Keylabel= " ";

            this.label2.Text = (this.ID = ID).ToString();
            this.label2.BackColor = Color.Transparent;

            this.setKeyToolStripMenuItem.Click += new EventHandler(onsetClick);
            this.resetToolStripMenuItem.Click += new EventHandler(onresetClick);

            this.editmode = false;
            this.editModeTimer = new Timer();
            this.editModeTimer.Interval = 350;
            this.editModeTimer.Tick += new EventHandler(editModeTimer_Tick);
        }

        [DllImport("user32")]
        static extern int MapVirtualKey(Keys uCode, int uMapType);
        const int MAPVK_VK_TO_CHAR = 2;

        private void onKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (editMode && Program.mainform.editmode)
            {
                if (true)
                {
                    if (this.keyChanged != null)
                        this.keyChanged(e.KeyValue, ID + 1);
                    if (Program.mainform.errorState == false)
                    {
                        this.editModeTimer.Stop();
                        this.editMode = false;
                        Program.mainform.editmode = false;
                        if (e.KeyValue >= 112 && e.KeyValue <= 123)
                            this.label1.Text =Keylabel= e.KeyData.ToString();
                        else
                            this.label1.Text =Keylabel= (char)(MapVirtualKey(e.KeyData, MAPVK_VK_TO_CHAR)) + "";
                        this.label1.BackColor = this.label2.BackColor = Color.FromArgb(170, 170, 170);
                    }
                    else
                        Program.mainform.errorState = false;
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

        private void onresetClick(object sender, EventArgs e)
        {
            if (this.keyChanged != null)
                this.keyChanged(0, ID+1);
            this.label1.Text = "";
            Keylabel = "";
        }

        private void onMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && Program.mainform.showlables)
                onsetClick(sender, new EventArgs());
        }

        private void onsetClick(object sender, EventArgs e)
        {
            if (!Program.mainform.editmode && !this.editMode)
            {
                this.editMode = true;
                Program.mainform.editmode = true;
                this.label1.Text =Keylabel= "";
                this.label1.BackColor = this.label2.BackColor = Color.Orange;
                this.label1.Focus();
                this.editModeTimer.Start();
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
        public string text
        {
            get{return Keylabel; }
            set 
            {
                if (Program.mainform.showlables)
                    label1.Text = Keylabel = value;
                else
                    Keylabel = value;
            }
        }


        public ContextMenuStrip conMenStrip
        {
            get{ return contextMenuStrip1; }
        }

        public Color color
        {
            set{
                if (!editMode)
                    if (Program.mainform.showlables)
                    {
                        this.label1.BackColor = this.label2.BackColor = value;
                    }
                    else
                        this.label1.BackColor = value;
            }
        }

        public bool editMode
        {
            get { return editmode;}
            private set { editmode = value; }
        }
    }
}
