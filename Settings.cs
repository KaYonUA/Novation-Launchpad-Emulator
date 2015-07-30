using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AlphaForm;

namespace GUI_MIDI
{
    enum EventType
    {
        FileLoaded,
        FileSaved,
        None
    };
    public partial class Settings : Form
    {
        EventType eventType;
        public Settings()
        {
            InitializeComponent();
            eventType = EventType.None;
            this.BackColor = PadButton.backColor;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            alphaTheme.TransformForm(0);
            checkBox1.Checked = Program.mainform.hlButtons;
            alphaTheme.Fade(FadeType.FadeIn, true, false, 200);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.mainform.hlButtons = checkBox1.Checked;
            alphaTheme.Fade(FadeType.FadeOut, true, false, 200);
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.savePresetDialog.ShowDialog() == DialogResult.OK)
                Program.mainform.saveButton(savePresetDialog.FileName);
        }
        private void loadButton_Click(object sender, EventArgs e)
        {
            if (this.openPresetDialog.ShowDialog() == DialogResult.OK)
                Program.mainform.loadButton(openPresetDialog.SafeFileName, openPresetDialog.FileName);
        }
    }
}
