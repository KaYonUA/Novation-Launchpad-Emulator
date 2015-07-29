using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using AlphaForm;

namespace GUI_MIDI
{
    partial class PadForm
    {
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PadForm));
            this.openPresetDialog = new System.Windows.Forms.OpenFileDialog();
            this.savePresetDialog = new System.Windows.Forms.SaveFileDialog();
            this.alphaTheme = new AlphaForm.AlphaFormTransformer();
            this.presetButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.presetPart = new System.Windows.Forms.Label();
            this.cPresetName = new System.Windows.Forms.Label();
            this.editmodeCheckBox = new System.Windows.Forms.CheckBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.alphaFormMarker1 = new AlphaForm.AlphaFormMarker();
            this.alphaTheme.SuspendLayout();
            this.SuspendLayout();
            // 
            // openPresetDialog
            // 
            this.openPresetDialog.Filter = "LE Preset|*.lep";
            this.openPresetDialog.InitialDirectory = "E:\\VisualStudio\\Common7\\IDE\\Presets";
            this.openPresetDialog.Title = "Open Preset";
            // 
            // savePresetDialog
            // 
            this.savePresetDialog.Filter = "LE Preset|*.lep";
            this.savePresetDialog.InitialDirectory = "E:\\VisualStudio\\Common7\\IDE\\Presets";
            this.savePresetDialog.Title = "Save Preset";
            // 
            // alphaTheme
            // 
            this.alphaTheme.BackgroundImage = global::GUI_MIDI.Properties.Resources.LaunchpadBig;
            this.alphaTheme.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.alphaTheme.Controls.Add(this.presetButton);
            this.alphaTheme.Controls.Add(this.button1);
            this.alphaTheme.Controls.Add(this.presetPart);
            this.alphaTheme.Controls.Add(this.cPresetName);
            this.alphaTheme.Controls.Add(this.editmodeCheckBox);
            this.alphaTheme.Controls.Add(this.exitButton);
            this.alphaTheme.Controls.Add(this.alphaFormMarker1);
            this.alphaTheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alphaTheme.DragSleep = ((uint)(30u));
            this.alphaTheme.Location = new System.Drawing.Point(0, 0);
            this.alphaTheme.Name = "alphaTheme";
            this.alphaTheme.Size = new System.Drawing.Size(633, 631);
            this.alphaTheme.TabIndex = 0;
            // 
            // presetButton
            // 
            this.presetButton.Location = new System.Drawing.Point(421, 579);
            this.presetButton.Name = "presetButton";
            this.presetButton.Size = new System.Drawing.Size(66, 23);
            this.presetButton.TabIndex = 10;
            this.presetButton.Text = "Preset";
            this.presetButton.UseVisualStyleBackColor = true;
            this.presetButton.Click += new System.EventHandler(this.presetButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(493, 579);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // presetPart
            // 
            this.presetPart.AutoSize = true;
            this.presetPart.ForeColor = System.Drawing.Color.Gray;
            this.presetPart.Location = new System.Drawing.Point(35, 584);
            this.presetPart.Name = "presetPart";
            this.presetPart.Size = new System.Drawing.Size(38, 13);
            this.presetPart.TabIndex = 8;
            this.presetPart.Text = "Part: 1";
            // 
            // cPresetName
            // 
            this.cPresetName.AutoSize = true;
            this.cPresetName.ForeColor = System.Drawing.Color.White;
            this.cPresetName.Location = new System.Drawing.Point(156, 582);
            this.cPresetName.Name = "cPresetName";
            this.cPresetName.Size = new System.Drawing.Size(0, 13);
            this.cPresetName.TabIndex = 7;
            // 
            // editmodeCheckBox
            // 
            this.editmodeCheckBox.AutoSize = true;
            this.editmodeCheckBox.ForeColor = System.Drawing.Color.Gray;
            this.editmodeCheckBox.Location = new System.Drawing.Point(341, 583);
            this.editmodeCheckBox.Name = "editmodeCheckBox";
            this.editmodeCheckBox.Size = new System.Drawing.Size(74, 17);
            this.editmodeCheckBox.TabIndex = 5;
            this.editmodeCheckBox.Text = "Edit Mode";
            this.editmodeCheckBox.UseVisualStyleBackColor = true;
            this.editmodeCheckBox.CheckedChanged += new System.EventHandler(this.editmodeCheckBox_Checked);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(559, 579);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(33, 23);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // alphaFormMarker1
            // 
            this.alphaFormMarker1.FillBorder = ((uint)(4u));
            this.alphaFormMarker1.Location = new System.Drawing.Point(312, 329);
            this.alphaFormMarker1.Name = "alphaFormMarker1";
            this.alphaFormMarker1.Size = new System.Drawing.Size(17, 17);
            this.alphaFormMarker1.TabIndex = 0;
            // 
            // PadForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(633, 631);
            this.Controls.Add(this.alphaTheme);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PadForm";
            this.Text = "Novation Launchpad Emulator";
            this.Load += new System.EventHandler(this.Program_Load);
            this.alphaTheme.ResumeLayout(false);
            this.alphaTheme.PerformLayout();
            this.ResumeLayout(false);

        }

        private AlphaFormTransformer alphaTheme;
        private AlphaFormMarker alphaFormMarker1;
        private System.Windows.Forms.CheckBox editmodeCheckBox;
        private System.Windows.Forms.OpenFileDialog openPresetDialog;
        private System.Windows.Forms.SaveFileDialog savePresetDialog;
        private System.Windows.Forms.Label cPresetName;
        private System.Windows.Forms.Button exitButton;
        private Label presetPart;
        private Button button1;
        private Button presetButton;
    }
}
