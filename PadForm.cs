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
    partial class PadForm : Form
    {
        public Launchpad launchpad;
        public Preset preset;
        public MidiIn midiin;

        public bool showlables;
        public bool errorState;
        public bool hlButtons;
        public bool loadCompleted;
        
        public PadButton[] buttons;
        Settings settings;
        PresetDialog presetDial;

        public PadForm()
        {
            this.AutoSize = true;

            errorState = false;
            showlables = false;
            hlButtons = false;

            preset = new Preset(pCount: 8,bCount: 72);
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
                    launchpad = new Launchpad(i);
                    break;
                }
            }

            if (midiin != null && launchpad != null)
            {
                midiin.Start();
                midiin.MessageReceived += new EventHandler<MidiInMessageEventArgs>(midiEffectsIn);

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
                    this.buttons[i] = new PadButton(i, rounded: false);
                    this.buttons[i].Location = new System.Drawing.Point(x, y);
                    this.buttons[i].Name = "centerButtons";
                    this.buttons[i].Size = new System.Drawing.Size(52, 52);
                    this.buttons[i].TabIndex = 1;
                    this.buttons[i].TabStop = false;
                    this.buttons[i].ButtonClick += new EventHandler(onButtonClick);
                    this.buttons[i].ButtonRelease += new EventHandler(onButtonRelease);
                    this.buttons[i].KeyChanged += new KeyChangeEvent(onKeyChanged);
                    this.buttons[i].ID = i;
                    this.alphaTheme.Controls.Add(this.buttons[i]);
                }
                for (int i = 64, y = 105; i < 72; i++, y += 60)
                {
                    this.buttons[i] = new PadButton(i, rounded: true);
                    this.buttons[i].Location = new System.Drawing.Point(532, y);
                    this.buttons[i].Name = "rightButtons";
                    this.buttons[i].Size = new System.Drawing.Size(52, 52);
                    this.buttons[i].TabIndex = 1;
                    this.buttons[i].TabStop = false;
                    this.buttons[i].ButtonClick += new EventHandler(onButtonClick);
                    this.buttons[i].ButtonRelease += new EventHandler(onButtonRelease);
                    this.buttons[i].KeyChanged += new KeyChangeEvent(onKeyChanged);
                    this.buttons[i].ID = i;
                    this.alphaTheme.Controls.Add(this.buttons[i]);
                }
                this.KeyPreview = true;
                this.KeyDown += new KeyEventHandler(onKeyDown);
                this.KeyUp += new KeyEventHandler(onKeyUp);
                this.BackColor = PadButton.backColor;
                alphaTheme.CanDrag = true;
                
            }
        }
        private void Program_Load(object sender, EventArgs e)
        {
            alphaTheme.TransformForm(255);
            
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.alphaTheme.Fade(FadeType.FadeOut, true, false, 600);
            Close();
        }

        private void onButtonClick(object sender, EventArgs e)
        {
            PadButton button = (PadButton)sender;
            if(hlButtons)
                button.HighLightButton = true;
            launchpad.startPlayingNote(button.ID);
        }
        private void onButtonRelease(object sender, EventArgs e)
        {
            PadButton button = (PadButton)sender;
            launchpad.stopPlayingNote(button.ID);
            if (hlButtons)
                button.HighLightButton = false;
            if (button.Key >= Keys.F1 && button.Key <= Keys.F24)
                changePresetPart(button.Key - Keys.F1);
        }

        void onKeyUp(object sender, KeyEventArgs e)
        {
            if (PadButton.KeyChangedOk)
                PadButton.KeyChangedOk = false;
            else
                if (!PadButton.Editing)
                {
                    int buttonID = preset.currentPart.keyMap.getNote(e.KeyValue);
                    if (buttonID != -1)
                    {
                        launchpad.stopPlayingNote(buttonID);
                        if (hlButtons)
                            buttons[buttonID].HighLightButton = false;
                    }
                    if (e.KeyData >= Keys.F1 && e.KeyData <= Keys.F12)
                        changePresetPart(e.KeyData - Keys.F1);
                }
        }
        void onKeyDown(object sender, KeyEventArgs e)
        {
            if (!PadButton.Editing)
            {
                
                int buttonID = preset.currentPart.keyMap.getNote(e.KeyValue);
                if (buttonID != -1)
                {
                    launchpad.startPlayingNote(buttonID);
                    if (hlButtons)
                        buttons[buttonID].HighLightButton = true;
                }
            }
        }

        private bool onKeyChanged(object sender, KeyEventArgs e)
        {
            PadButton s = (PadButton)sender;
            if (e.KeyCode == Keys.None)
            {
                preset.currentPart.keyMap.resetButton(s.ID + 1);
                //this.Focus();
                return true;
            }
            else
            {
                if (!preset.currentPart.keyMap.mapKey(e.KeyValue, s.ID + 1))
                {
                    MessageBox.Show("Кнопка " + e.KeyData.ToString() + " уже назначена.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    //this.Focus();
                    return true;
                }
            }
        }

        private void midiEffectsIn(object sender, MidiInMessageEventArgs e)
        {
            //if (e.MidiEvent != null)
            //    if (true)
            //        Console.WriteLine("Inpute message from " + e.MidiEvent.Channel + " Comand" + e.MidiEvent.CommandCode);
            
            if (e.MidiEvent.Channel == 1 && e.MidiEvent.CommandCode != MidiCommandCode.AutoSensing)
            {
                if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    NoteOnEvent noEvent = (NoteOnEvent)e.MidiEvent;

                    if (noEvent.NoteNumber <= 107 && noEvent.NoteNumber >= 36)
                    {
                        int button = Launchpad.getButtonNo(noEvent.NoteNumber);
                        int red = noEvent.Velocity & 0x3;
                        int green = noEvent.Velocity & 0x30;
                        green = green >> 4;
                        if(red == 0 && green == 0)
                            buttons[button].LED = Color.FromArgb(170, 170, 170);
                        else
                           buttons[button].LED = Color.FromArgb(red * 85, green * 85, 0);
                    }
                }
                if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
                {
                    NoteEvent nEvent = (NoteEvent)e.MidiEvent;
                    if (nEvent.NoteNumber <= 107 && nEvent.NoteNumber >= 36)
                    {
                        int button = Launchpad.getButtonNo(nEvent.NoteNumber);
                        buttons[button].LED = Color.FromArgb(170, 170, 170);
                    }
                }
            }
        }

        public void saveButton(string filename)
        {
            for (int i = 0; i < preset.buttonsCount; i++)
                preset.currentPart.bLabels[i] = buttons[i].Text;
            preset.saveToFile(filename);
            this.cPresetName.Text = "";
            
        }
        public void loadButton(string filename,string filepath)
        {
            preset.loadFromFile(filepath);
            this.cPresetName.Text = filename;
            if (!PadButton.EditMode)
            {
                PadButton.EditMode = true;
                this.editmodeCheckBox.Checked = true;
            }
            for (int i = 0; i < preset.buttonsCount; i++)
                buttons[i].Text = preset.currentPart.bLabels[i];
        }
        private void resetButton_Click(object sender, EventArgs e)
        {
            foreach (var button in buttons)
                button.Text = "";
            preset.currentPart.keyMap.indexes.Clear();
            preset.currentPart.keyMap.keys.Clear();                                           
        }

        private void editmodeCheckBox_Checked(object sender, EventArgs e)
        {
            PadButton.EditMode = (sender as CheckBox).Checked;
            foreach (var buttonObj in buttons)
                buttonObj.Invalidate();  
        }
        private void changePresetPart(int index)
        {
            for (int i = 0; i < 72; i++)
            {
                preset.currentPart.bLabels[i] = buttons[i].Text;
                buttons[i].Text = preset[index].bLabels[i];
            }
            preset.selectPart(index);
            this.presetPart.Text = "Part: " + (index+1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settings = new Settings();
            settings.StartPosition = FormStartPosition.CenterScreen;
            settings.ShowDialog();
        }

        private void presetButton_Click(object sender, EventArgs e)
        {
            presetDial = new PresetDialog();
            presetDial.ShowDialog();
        }
    }
}
