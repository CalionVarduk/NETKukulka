using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETKukulka
{
    public partial class ElapsedTimeForm : Form
    {
        public ElapsedTimeTracker TimeTracker { get; set; }
        public bool Locked { get; private set; }

        private Timer timer;
        private int targetHeightOffset;
        private long prevSeconds;

        public ElapsedTimeForm(ElapsedTimeTracker timeTracker)
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 30;
            timer.Tick += Event_Tick;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;

            TimeTracker = timeTracker;
            prevSeconds = -1;
            targetHeightOffset = 0;
            Locked = false;
        }

        public void displayTimeForm()
        {
            Locked = false;
            Top = Screen.PrimaryScreen.Bounds.Height;
            updateTime();
            timer.Start();
            Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.DodgerBlue, lockButton.Right, 0, closeButton.Left, closeButton.Bottom);
        }

        protected void Event_Tick(Object sender, EventArgs e)
        {
            if (!Locked)
            {
                int targetHeight = Cursor.Position.Y - targetHeightOffset;
                Top -= (Top - targetHeight) >> 3;
            }
            updateTime();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            lockButton.Text = "L";
            Locked = false;
            labelTime.Text = "";
            prevSeconds = -1;
            targetHeightOffset = 0;
            timer.Stop();
            Hide();
        }

        private void lockButton_Click(object sender, EventArgs e)
        {
            Locked = !Locked;
            if (Locked)
            {
                lockButton.Text = "U";
                toolTip.SetToolTip(lockButton, "Odblokuj pozycję");
            }
            else
            {
                lockButton.Text = "L";
                toolTip.SetToolTip(lockButton, "Zablokuj pozycję");
            }
        }

        private void updateTime()
        {
            long seconds = TimeTracker.ElapsedSeconds;

            if (prevSeconds != seconds)
            {
                prevSeconds = seconds;
                labelTime.Text = TimeTracker.ElapsedTimeFormatted;

                ClientSize = new Size(labelTime.Right + 12, labelTime.Bottom + 12);
                closeButton.Left = Width - closeButton.Width + 1;

                targetHeightOffset = closeButton.Top + (closeButton.Height >> 1);
                Left = Screen.PrimaryScreen.Bounds.Width - Width;
            }
        }

        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            //e.DrawBorder();
            e.DrawText();
        }
    }
}
