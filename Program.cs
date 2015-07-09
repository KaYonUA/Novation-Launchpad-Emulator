using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Midi;

using AlphaForm;

namespace GUI_MIDI
{
    class Program : Form
    {
        Launchpad _launch;
        KeyMap kmap;
        private AlphaFormTransformer alphaFormTransformer1;
        private AlphaFormMarker alphaFormMarker1;
        private PadButton[] buttons;

        private Button button1;

        //Point last;

        [STAThread]
        public static void Main()
        {

            Application.EnableVisualStyles();
            Application.Run(new Program());
        }
        public Program()
        {
            this.AutoSize = true;

            _launch = new Launchpad(1);
            kmap = new KeyMap();

            kmap.mapKey('Z', 57);
            kmap.mapKey('X', 58);
            kmap.mapKey('C', 59);
            kmap.mapKey('V', 60);
            kmap.mapKey('S', 18);
            kmap.mapKey('D', 19);
            kmap.mapKey('E', 11);
            kmap.mapKey('N', 60);
            kmap.mapKey('O', 40);
            kmap.mapKey('I', 39);
            kmap.mapKey('U', 38);
            kmap.mapKey('Y', 37);
            kmap.mapKey('T', 36);
            kmap.mapKey('6', 45);
            kmap.mapKey('H', 53);
            kmap.mapKey('J', 46);
            kmap.mapKey('7', 30);
            kmap.mapKey('8', 31);
            kmap.mapKey('9', 32);
            kmap.mapKey('K', 47);
            
            InitializeComponent();
            buttons = new PadButton[64];
            for (int i = 0, x = 45, y = 105; i < 64; i++, x += 60)
            {
                if (x == 45 + (60 * 8))
                {
                    x = 45;
                    y += 60;
                }
                this.buttons[i] = new PadButton();
                this.buttons[i].Location = new System.Drawing.Point(x, y);
                this.buttons[i].Name = "pictureBox1";
                this.buttons[i].Size = new System.Drawing.Size(50, 50);
                this.buttons[i].TabIndex = 1;
                this.buttons[i].ID = i;
                this.buttons[i].TabStop = false;
                this.buttons[i].mouseDown += new pButtonHandler(this.mouseP);
                this.buttons[i].mouseUp += new pButtonHandler(this.mouseR);
                this.alphaFormTransformer1.Controls.Add(this.buttons[i]);
            }
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_keyUp);
        }

        void Form1_keyUp(object sender, KeyEventArgs e)
        {
            _launch.stopPlayingNote(kmap.getNote(e.KeyValue));
            buttons[kmap.getNote(e.KeyValue)].button.BackColor = System.Drawing.Color.LightGray;
        }
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            _launch.startPlayingNote(kmap.getNote(e.KeyValue));
            buttons[kmap.getNote(e.KeyValue)].button.BackColor = System.Drawing.Color.Orange;
        }

        private void InitializeComponent()
        {
            this.alphaFormTransformer1 = new AlphaForm.AlphaFormTransformer();
            this.button1 = new System.Windows.Forms.Button();
            this.alphaFormMarker1 = new AlphaForm.AlphaFormMarker();
            this.alphaFormTransformer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // alphaFormTransformer1
            // 
            this.alphaFormTransformer1.BackgroundImage = global::GUI_MIDI.Properties.Resources.LaunchpadPro;
            this.alphaFormTransformer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.alphaFormTransformer1.Controls.Add(this.button1);
            this.alphaFormTransformer1.Controls.Add(this.alphaFormMarker1);
            this.alphaFormTransformer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alphaFormTransformer1.DragSleep = ((uint)(30u));
            this.alphaFormTransformer1.Location = new System.Drawing.Point(0, 0);
            this.alphaFormTransformer1.Name = "alphaFormTransformer1";
            this.alphaFormTransformer1.Size = new System.Drawing.Size(633, 631);
            this.alphaFormTransformer1.TabIndex = 0;
            this.alphaFormTransformer1.Paint += new System.Windows.Forms.PaintEventHandler(this.alphaFormTransformer1_Paint_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(512, 577);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 27);
            this.button1.TabIndex = 2;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // alphaFormMarker1
            // 
            this.alphaFormMarker1.FillBorder = ((uint)(4u));
            this.alphaFormMarker1.Location = new System.Drawing.Point(311, 313);
            this.alphaFormMarker1.Name = "alphaFormMarker1";
            this.alphaFormMarker1.Size = new System.Drawing.Size(17, 17);
            this.alphaFormMarker1.TabIndex = 0;
            // 
            // Program
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(633, 631);
            this.Controls.Add(this.alphaFormTransformer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Program";
            this.Text = "Novation Launchpad Emulator";
            this.Load += new System.EventHandler(this.Program_Load);
            this.alphaFormTransformer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private void Program_Load(object sender, EventArgs e)
        {
            alphaFormTransformer1.TransformForm(255);
        }
        private void alphaFormTransformer1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mouseP(object sender,MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _launch.startPlayingNote((sender as PadButton).ID);
                (sender as PadButton).button.BackColor = System.Drawing.Color.Orange;
            }
            else
            {
                Point cords = new Point(this.Location.X + (sender as PadButton).Location.X + e.Location.X,
                    this.Location.Y + (sender as PadButton).Location.Y + e.Location.Y);
                (sender as PadButton).conMenStrip.Show(cords);
            }
        }
        private void mouseR(object sender,MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _launch.stopPlayingNote((sender as PadButton).ID);
                (sender as PadButton).button.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }
    class Launchpad
    {
        private MidiOut midiout;

        public readonly int mainPadButtonsCount = 64;
        public readonly int rightPadButtonsCount = 8;

        private int[] mainPad;
        private int[] rightPad;
        private bool[] noteStatus;

        public Launchpad(int deviceID)
        {
            midiout = new MidiOut(deviceID);
            int[] leftColumn = new int[32];
            int[] rightColumn = new int[32];

            for (int i = 0; i < 32; i++)
                leftColumn[i] = i + 36;
            for (int i = 0; i < 32; i++)
                rightColumn[i] = i + 68;

            mainPad = new int[mainPadButtonsCount];
            for (int i = 0,l = 0; i < 8; i++){
               for (int z = 3; z >= 0; z--, l++)
                   mainPad[l] = leftColumn[31 - (z + (i * 4))];
               for (int z = 3; z >= 0; z--, l++)
                   mainPad[l] = rightColumn[31 - (z + (i * 4))];
            }

            rightPad = new int[rightPadButtonsCount];
            for (int i = 0; i < rightPadButtonsCount; i++){
                rightPad[i] = 100 + i;
            }
            noteStatus = new bool[72];
        }

        private bool checkButton(int buttonNo)
        {
            if (buttonNo >= 0 && buttonNo <= 72)
                return true;
            else
                return false;
        }

        public int getButtonNote(int buttonNo)
        {
            if (buttonNo >= 0 && buttonNo <= 63)
                return mainPad[buttonNo];
            else
                if ((buttonNo - 64) >= 0 && (buttonNo - 64) <= 7 && buttonNo > 63)
                    return rightPad[buttonNo - 64];
                else return -1;
        }

        public void startPlayingNote(int buttonNo)
        {
            if (checkButton(buttonNo) && noteStatus[buttonNo] == false)
            {
                midiout.Send(MidiMessage.StartNote(getButtonNote(buttonNo), 127, 1).RawData);
                //Console.WriteLine("Note: " + getButtonNote(buttonNo) + " start play.");
                noteStatus[buttonNo] = true;
            }
        }
        public void stopPlayingNote(int buttonNo)
        {
            if (checkButton(buttonNo) && noteStatus[buttonNo] == true)
            {
                midiout.Send(MidiMessage.StopNote(getButtonNote(buttonNo), 0, 1).RawData);
                //Console.WriteLine("Note: " + getButtonNote(buttonNo) + " stop play.");
                noteStatus[buttonNo] = false;
            }
        }
    }

    class KeyMap
    {
        private int[] keyTable;

        public KeyMap()
        {
            keyTable = new int[256];
        }
        public void mapKey(int keyValue, int buttonNo)
        {
            keyTable[keyValue] = buttonNo;
        }
        public int getNote(int keyValue)
        {
            if (keyTable[keyValue] == 0)
            {
                Console.WriteLine("KeyNotMapped");
                MessageBox.Show("Key Not Mapped!");
                return -1;
            }
            else
                return keyTable[keyValue] - 1;
        }       
                   
    }
    
}
