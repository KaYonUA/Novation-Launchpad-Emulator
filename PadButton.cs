using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace GUI_MIDI
{
    public delegate bool KeyChangeEvent(object sender,KeyEventArgs e);
    public partial class PadButton : Control
    {
        private string _text;
        private string _texttemp;
        private bool _hlbutton;
        private Color _led;

        private Font TextFont;
        private Font idFont;

        private SolidBrush TextBrush;
        private SolidBrush idBrush;
        private SolidBrush bgBrush;
        private readonly Pen highlight = new Pen(Color.Red);

        GraphicsPath gfxPath;
        GraphicsPath pbPath;
        ContextMenuStrip keyMapMenu;
        ToolStripMenuItem setMenuItem;
        ToolStripMenuItem resetMenuItem;

        static public Color ledDisabled = Color.FromArgb(170, 170, 170);
        static public Color ledEditMode = Color.Orange;
        static public Color backColor = Color.FromArgb(37, 45, 58);

        private Timer editModeTimer;

        public event EventHandler ButtonClick;
        public event EventHandler ButtonRelease;
        public event KeyChangeEvent KeyChanged;
        
        public PadButton(int ID, bool rounded)
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            this.Rounded = rounded;
            this._led = ledDisabled;
            this.Text = "";

            TextFont = new System.Drawing.Font("Microsoft Sans Serif", 14F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            idFont = new Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            TextBrush = new SolidBrush(Color.Black);
            idBrush = new SolidBrush(Color.Gray);
            bgBrush = new SolidBrush(ledDisabled);

            keyMapMenu = new ContextMenuStrip();
            setMenuItem = new ToolStripMenuItem("Set");
            resetMenuItem = new ToolStripMenuItem("Reset");
            keyMapMenu.Items.AddRange(new ToolStripItem[] { setMenuItem, resetMenuItem });

            setMenuItem.Click += new EventHandler(setKeyDialog);
            resetMenuItem.Click += new EventHandler(resetKeyDialog);

            PadButton.Editing = false;
            this.editModeTimer = new Timer();
            this.editModeTimer.Interval = 350;
            this.editModeTimer.Tick += new EventHandler(editModeTimer_Tick);
        }

        private void resetKeyDialog(object sender, EventArgs e)
        {
            this.Text = "";
            if(KeyChanged != null)
                KeyChanged(this, new KeyEventArgs(Keys.None));
        }

        private void setKeyDialog(object sender, EventArgs e)
        {
            if (!PadButton.Editing)
            {
                PadButton.Editing = true;
                _texttemp = this.Text;
                editModeTimer.Start();
                this.LED = Color.Orange;
                this.Focus();
            }
        }

        [DllImport("user32")]
        static extern int MapVirtualKey(Keys uCode, int uMapType);
        const int MAPVK_VK_TO_CHAR = 2;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(PadButton.Editing && KeyChanged != null)
            {
                if (e.KeyData == Keys.Escape)
                {
                    PadButton.Editing = false;
                    editModeTimer.Stop();
                    this.LED = ledDisabled;
                    this.Text = _texttemp;
                }
                else
                {
                    if (KeyChanged(this, e))
                    {
                        KeyChangedOk = true;
                        PadButton.Editing = false;
                        editModeTimer.Stop();
                        this.LED = ledDisabled;
                        this.Key = e.KeyCode;
                        if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F24)
                            this.Text = e.KeyCode.ToString();
                        else
                            this.Text = (char)(MapVirtualKey(e.KeyCode, MAPVK_VK_TO_CHAR)) + "";
                    }
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if(ButtonClick != null)
                {
                    ButtonClick(this, new EventArgs());
                }
            }

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                keyMapMenu.Show(this, e.Location);
            }
            if (e.Button == MouseButtons.Left)
            {
                if (ButtonRelease != null)
                {
                    ButtonRelease(this, new EventArgs());
                }
            }
        }
        private void editModeTimer_Tick(object sender, EventArgs e)
        {
            if (this.Text == "")
            {
                this.Text = "-";
            }
            else
            {
                this.Text = "";
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if(!PadButton.Editing && PadButton.EditMode)
            {
                if(e.Button == MouseButtons.Middle)
                {
                    setKeyDialog(null, new EventArgs());
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            gfxPath = new GraphicsPath();
            pbPath = new GraphicsPath();
            if (!Rounded)
            {
                int width = ClientRectangle.Width - 1;
                int height = ClientRectangle.Height - 1;
                int radius = 3;

                gfxPath.AddLine(radius + 1, 1, (width - radius * 2) - 1, 1);
                gfxPath.AddArc((width - radius * 2) - 1, 1, radius * 2, radius * 2, 270, 90);
                gfxPath.AddLine(width - 1, radius + 1, width - 1, (height - radius * 2) - 1);
                gfxPath.AddArc((width - radius * 2) - 1, (height - radius * 2) - 1, radius * 2, radius * 2, 0, 90);
                gfxPath.AddLine((width - radius * 2) - 1, height - 1, radius+1, height-1);
                gfxPath.AddArc(1, (height - radius * 2) - 1, radius * 2, radius * 2, 90, 90);
                gfxPath.AddLine(1, (height - radius * 2) - 1, 1, radius + 1);
                gfxPath.AddArc(1, 1, radius * 2, radius * 2, 180, 90);
                gfxPath.CloseFigure();

                
                if (this._led != ledDisabled)
                {
                    using (var brush = new PathGradientBrush(gfxPath))
                    {
                        float h = _led.GetHue();
                        float s = _led.GetSaturation();

                        brush.CenterPoint = new PointF(width / 2f, height / 2f);
                        brush.CenterColor = PadButton.ColorFromHSV(h, s, 1f);
                        brush.SurroundColors = new[] { PadButton.ColorFromHSV(h, s, 0.55) };
                        brush.FocusScales = new PointF(0.2f, 0.2f);
                       
                        e.Graphics.FillPath(brush, gfxPath);
                    }
                }
                else
                {
                    bgBrush.Color = this._led;
                    e.Graphics.FillPath(bgBrush, gfxPath);
                }
                if (_hlbutton)
                {
                    pbPath.AddLine(radius, 0, width - radius * 2, 0);
                    pbPath.AddArc(width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                    pbPath.AddLine(width, radius, width, height - radius * 2);
                    pbPath.AddArc(width - radius * 2, height - radius * 2, radius * 2, radius * 2, 0, 90);
                    pbPath.AddLine(width - radius * 2, height, radius, height);
                    pbPath.AddArc(0, height - radius * 2, radius * 2, radius * 2, 90, 90);
                    pbPath.AddLine(0, height - radius * 2, 0, radius);
                    pbPath.AddArc(0, 0, radius * 2, radius * 2, 180, 90);

                    Pen pen2 = new Pen(Color.Red);
                    e.Graphics.DrawPath(pen2, pbPath);
                }
            }
            else
            {
                int width = (ClientRectangle.Width - 1) - 10;
                int height = (ClientRectangle.Height - 1) - 10;

                gfxPath.AddEllipse(5, 5, width, height);
                gfxPath.CloseFigure();

                if (this._led != ledDisabled)
                {
                    using (var brush = new PathGradientBrush(gfxPath))
                    {
                        float h = _led.GetHue();
                        float s = _led.GetSaturation();

                        brush.CenterPoint = new PointF(width / 2f, height / 2f);
                        brush.CenterColor = PadButton.ColorFromHSV(h, s, 1f);
                        brush.SurroundColors = new[] { PadButton.ColorFromHSV(h, s, 0.50) };
                        brush.FocusScales = new PointF(0.2f, 0.2f);
                        bgBrush.Color = this._led;
                        Pen pen = new Pen(bgBrush);
                       
                    }
                }
                else
                {
                    bgBrush.Color = this._led;
                    e.Graphics.FillPath(bgBrush, gfxPath);
                }

                if (_hlbutton)
                    e.Graphics.DrawPath(highlight, gfxPath);
            }

            

            if (PadButton.EditMode)
            {
                int x = 0, y = 0;
                SizeF size = e.Graphics.MeasureString(this.Text, TextFont);
                x = (ClientRectangle.Width - 1) / 2 - (int)(size.Width / 2);
                y = (ClientRectangle.Height - 1) / 2 - (int)(size.Height / 2);
                e.Graphics.DrawString(this.Text, TextFont, TextBrush, x, y);

                size = e.Graphics.MeasureString(ID.ToString(), idFont);

                x = (ClientRectangle.Width - 1) / 2 - (int)(size.Width / 2);
                if(this.Rounded)
                    y = (ClientRectangle.Height - 1) - (int)size.Height - 5;
                else
                    y = (ClientRectangle.Height - 1) - (int)size.Height;

                e.Graphics.DrawString(ID.ToString(), idFont, idBrush, x, y);
            }
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public override string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.Invalidate();
            }
        }
        public Color LED
        {
            get { return this._led; }
            set 
            {
                this._led = value;
                this.Invalidate();
            }
        }
        public bool HighLightButton
        {
            get { return this._hlbutton; }
            set
            {
                this._hlbutton = value;
                this.Invalidate();
            }
        }
        public int ID { get; set; }
        public bool Rounded { get; set; }
        public static bool EditMode { get; set; }
        public static bool Editing { get; set; }
        public static bool KeyChangedOk { get; set; }
        public Keys Key { get; private set; }
    }
}
