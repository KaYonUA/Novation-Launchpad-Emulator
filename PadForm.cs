using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using NAudio.Midi;

using AlphaForm;

namespace GUI_MIDI
{
    class PadForm : Form
    {
        public Launchpad _launch;
        public Presset preset;
        public MidiIn midiin;
        public bool editmode;
        public bool showlables;
        public bool errorState;
        private AlphaFormTransformer alphaFormTransformer1;
        private AlphaFormMarker alphaFormMarker1;
        public PadButton[] buttons;
        private Button button3;
        private Button button2;
        private CheckBox checkBox1;
        private Button button4;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Label label1;

        private Button button1;

        public PadForm()
        {
            this.AutoSize = true;
            editmode = false;
            errorState = false;
            showlables = false;
            preset = new Presset();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                if (MidiIn.DeviceInfo(i).ProductName == "Launchpad Emulator")
                {
                    midiin = new MidiIn(i);
                    break;
                }
            }
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                if (MidiOut.DeviceInfo(i).ProductName == "Launchpad Emulator")
                {
                    _launch = new Launchpad(i);
                    break;
                }
            }
            if (midiin != null && _launch != null)
            {
                midiin.Start();
                midiin.MessageReceived += new EventHandler<MidiInMessageEventArgs>(midiEffectsIn);
            }

            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            buttons = new PadButton[72];
            for (int i = 0, x = 36, y = 105; i < 64; i++, x += 62)
            {
                if (x == 36 + (62 * 8))
                {
                    x = 36;
                    y += 60;
                }
                this.buttons[i] = new PadButton(i);
                this.buttons[i].Location = new System.Drawing.Point(x, y);
                this.buttons[i].Name = "pictureBox1";
                this.buttons[i].Size = new System.Drawing.Size(50, 50);
                this.buttons[i].TabIndex = 1;
                this.buttons[i].TabStop = false;
                this.buttons[i].mouseDown += new pButtonHandler(this.mouseP);
                this.buttons[i].mouseUp += new pButtonHandler(this.mouseR);
                this.buttons[i].keyChanged += new pSetButtonHendler(this.onKeySet);
                this.buttons[i].label2.BackColor = Color.FromArgb(170, 170, 170);
                this.alphaFormTransformer1.Controls.Add(this.buttons[i]);
            }
            for (int i = 64, y = 105; i < 72; i++, y += 60)
            {
                this.buttons[i] = new PadButton(i);
                this.buttons[i].Location = new System.Drawing.Point(532, y);
                this.buttons[i].Name = "pictureBox1";
                this.buttons[i].Size = new System.Drawing.Size(50, 50);
                this.buttons[i].TabIndex = 1;
                this.buttons[i].TabStop = false;
                this.buttons[i].mouseDown += new pButtonHandler(this.mouseP);
                this.buttons[i].mouseUp += new pButtonHandler(this.mouseR);
                this.buttons[i].keyChanged += new pSetButtonHendler(this.onKeySet);
                this.buttons[i].label2.BackColor = Color.FromArgb(170, 170, 170);
                this.alphaFormTransformer1.Controls.Add(this.buttons[i]);
            }
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_keyUp);
            this.BackColor = System.Drawing.Color.FromArgb(37, 45, 58);
        }
        private void InitializeComponent()
        {
            this.alphaFormTransformer1 = new AlphaForm.AlphaFormTransformer();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.alphaFormMarker1 = new AlphaForm.AlphaFormMarker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.alphaFormTransformer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // alphaFormTransformer1
            // 
            this.alphaFormTransformer1.BackgroundImage = global::GUI_MIDI.Properties.Resources.LaunchpadBig;
            this.alphaFormTransformer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.alphaFormTransformer1.Controls.Add(this.label1);
            this.alphaFormTransformer1.Controls.Add(this.button4);
            this.alphaFormTransformer1.Controls.Add(this.checkBox1);
            this.alphaFormTransformer1.Controls.Add(this.button3);
            this.alphaFormTransformer1.Controls.Add(this.button2);
            this.alphaFormTransformer1.Controls.Add(this.button1);
            this.alphaFormTransformer1.Controls.Add(this.alphaFormMarker1);
            this.alphaFormTransformer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alphaFormTransformer1.DragSleep = ((uint)(30u));
            this.alphaFormTransformer1.Location = new System.Drawing.Point(0, 0);
            this.alphaFormTransformer1.Name = "alphaFormTransformer1";
            this.alphaFormTransformer1.Size = new System.Drawing.Size(633, 631);
            this.alphaFormTransformer1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(52, 583);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(356, 579);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(48, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Reset";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(276, 583);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Edit Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(410, 579);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(46, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Load";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(462, 579);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(44, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.alphaFormMarker1.Location = new System.Drawing.Point(287, 331);
            this.alphaFormMarker1.Name = "alphaFormMarker1";
            this.alphaFormMarker1.Size = new System.Drawing.Size(17, 17);
            this.alphaFormMarker1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "LE preset|*.lep";
            this.openFileDialog1.InitialDirectory = "./";
            this.openFileDialog1.Title = "Open Preset";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "LE preset|*.lep";
            this.saveFileDialog1.Title = "Save Preset";
            // 
            // PadForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(633, 631);
            this.Controls.Add(this.alphaFormTransformer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PadForm";
            this.Text = "Novation Launchpad Emulator";
            this.Load += new System.EventHandler(this.Program_Load);
            this.alphaFormTransformer1.ResumeLayout(false);
            this.alphaFormTransformer1.PerformLayout();
            this.ResumeLayout(false);

        }
        private void Program_Load(object sender, EventArgs e)
        {
            alphaFormTransformer1.TransformForm(255);
            alphaFormTransformer1.CanDrag = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mouseP(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _launch.startPlayingNote((sender as PadButton).ID);
                //(sender as PadButton).button.BackColor = System.Drawing.Color.Orange;
            }
            else
            {
                if (e.Button == MouseButtons.Right && showlables)
                {
                    Point cords = new Point(this.Location.X + (sender as PadButton).Location.X + e.Location.X,
                        this.Location.Y + (sender as PadButton).Location.Y + e.Location.Y);
                    (sender as PadButton).conMenStrip.Show(cords);
                }
            }
        }
        private void mouseR(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _launch.stopPlayingNote((sender as PadButton).ID);
            }
        }

        void Form1_keyUp(object sender, KeyEventArgs e)
        {
            if (!editmode)
            {
                int buttonNo = preset.kmap.getNote(e.KeyValue);
                if (buttonNo != -1)
                    _launch.stopPlayingNote(buttonNo);
            }
        }
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!editmode)
            {
                //Console.WriteLine("Key pressed: " + e.KeyValue);
                int buttonNo = preset.kmap.getNote(e.KeyValue);
                if (buttonNo != -1)
                    _launch.startPlayingNote(buttonNo);
            }
        }

        private void onKeySet(int newkey, int id)
        {
            if (!preset.kmap.mapKey(newkey, id))
            {
                MessageBox.Show("Error");
                errorState = true;
            }
            if (newkey == 0)
                preset.kmap.resetButton(id);
            //Console.WriteLine("Key setted: " + newkey + ",to ID: " + id);
        }

        private void midiEffectsIn(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent.Channel == 5 && e.MidiEvent.CommandCode != MidiCommandCode.AutoSensing)
            {
                if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    if ((e.MidiEvent as NoteOnEvent).NoteNumber <= 107 && (e.MidiEvent as NoteOnEvent).NoteNumber >= 36)
                    {
                        NoteOnEvent ev = (NoteOnEvent)e.MidiEvent;
                        int velosity = ev.Velocity;
                        int red = ev.Velocity & 0x3;
                        int green = ev.Velocity & 0x30;
                        green = green >> 4;
                        //Console.WriteLine()
                        if(red == 0 && green == 0)
                            buttons[Launchpad.getButtonNo((e.MidiEvent as NoteEvent).NoteNumber)].color = System.Drawing.Color.FromArgb(170, 170, 170);
                        else
                            buttons[Launchpad.getButtonNo(ev.NoteNumber)].color = System.Drawing.Color.FromArgb(red * 85, green * 85, 0);
                    }
                }
                if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
                {
                    if ((e.MidiEvent as NoteEvent).NoteNumber <= 107 && (e.MidiEvent as NoteEvent).NoteNumber >= 36)
                        buttons[Launchpad.getButtonNo((e.MidiEvent as NoteEvent).NoteNumber)].color = System.Drawing.Color.FromArgb(170, 170, 170);
                    // Console.WriteLine("Light Off button No. " + Launchpad.getButtonNo(notee.NoteNumber));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < 72; i++)
                    preset.bLabels[i] = buttons[i].text;
                preset.saveToFile(saveFileDialog1.FileName);
                this.label1.Text = "";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                preset.loadFromFile(openFileDialog1.FileName);
                this.label1.Text = openFileDialog1.SafeFileName;
                for (int i = 0; i < 72; i++)
                    buttons[i].text = preset.bLabels[i];
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (var button in buttons)
                button.text = "";
            preset.kmap.indexes.Clear();
            preset.kmap.keys.Clear();                                                                         
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as CheckBox).Checked)
            {
                showlables = true;
                foreach (var button in buttons)
                {
                    button.label1.Text = button.text;
                    button.label2.Visible = true;
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    button.label1.Text = "";
                    button.label2.Visible = false;
                }
                showlables = false;
            }
        }     
    }
}
