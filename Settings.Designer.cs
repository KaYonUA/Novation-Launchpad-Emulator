namespace GUI_MIDI
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.alphaTheme = new AlphaForm.AlphaFormTransformer();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.alphaFormMarker2 = new AlphaForm.AlphaFormMarker();
            this.cPresetName = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.savePresetDialog = new System.Windows.Forms.SaveFileDialog();
            this.openPresetDialog = new System.Windows.Forms.OpenFileDialog();
            this.alphaTheme.SuspendLayout();
            this.SuspendLayout();
            // 
            // alphaTheme
            // 
            this.alphaTheme.BackgroundImage = global::GUI_MIDI.Properties.Resources.settings1;
            this.alphaTheme.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.alphaTheme.Controls.Add(this.saveButton);
            this.alphaTheme.Controls.Add(this.loadButton);
            this.alphaTheme.Controls.Add(this.checkBox1);
            this.alphaTheme.Controls.Add(this.button1);
            this.alphaTheme.Controls.Add(this.alphaFormMarker2);
            this.alphaTheme.Controls.Add(this.cPresetName);
            this.alphaTheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alphaTheme.DragSleep = ((uint)(30u));
            this.alphaTheme.Location = new System.Drawing.Point(0, 0);
            this.alphaTheme.Name = "alphaTheme";
            this.alphaTheme.Size = new System.Drawing.Size(250, 462);
            this.alphaTheme.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(45, 114);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(109, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Highlight pressing";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(91, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // alphaFormMarker2
            // 
            this.alphaFormMarker2.FillBorder = ((uint)(4u));
            this.alphaFormMarker2.Location = new System.Drawing.Point(121, 224);
            this.alphaFormMarker2.Name = "alphaFormMarker2";
            this.alphaFormMarker2.Size = new System.Drawing.Size(17, 17);
            this.alphaFormMarker2.TabIndex = 2;
            // 
            // cPresetName
            // 
            this.cPresetName.AutoSize = true;
            this.cPresetName.ForeColor = System.Drawing.Color.White;
            this.cPresetName.Location = new System.Drawing.Point(52, 583);
            this.cPresetName.Name = "cPresetName";
            this.cPresetName.Size = new System.Drawing.Size(0, 13);
            this.cPresetName.TabIndex = 7;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(45, 85);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(77, 23);
            this.loadButton.TabIndex = 10;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(128, 85);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(80, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // savePresetDialog
            // 
            this.savePresetDialog.Filter = "LE Preset|*.lep";
            this.savePresetDialog.InitialDirectory = "E:\\VisualStudio\\Common7\\IDE\\Presets";
            this.savePresetDialog.Title = "Save Preset";
            // 
            // openPresetDialog
            // 
            this.openPresetDialog.Filter = "LE Preset|*.lep";
            this.openPresetDialog.InitialDirectory = "E:\\VisualStudio\\Common7\\IDE\\Presets";
            this.openPresetDialog.Title = "Open Preset";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 462);
            this.Controls.Add(this.alphaTheme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.alphaTheme.ResumeLayout(false);
            this.alphaTheme.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AlphaForm.AlphaFormTransformer alphaTheme;
        private System.Windows.Forms.Label cPresetName;
        private AlphaForm.AlphaFormMarker alphaFormMarker2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.SaveFileDialog savePresetDialog;
        private System.Windows.Forms.OpenFileDialog openPresetDialog;
    }
}