using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace NETKukulka
{
    public partial class MainForm : Form
    {
        private Bitmap background;
        private Cuckoo cuckoo;
        private ElapsedTimeTracker timeTracker;
        private ElapsedTimeForm timeForm;
        private VisibilityController visibilityController;

        public MainForm()
        {
            InitializeComponent();
            additionalInit();

            cuckoo = new Cuckoo();
            timeTracker = new ElapsedTimeTracker(false);
            timeForm = new ElapsedTimeForm(timeTracker);
            visibilityController = new VisibilityController(this, 25, 100);

            timeTracker.setTimeFormatter(elapsedTimeFormat);
            cuckoo.NewHour += Event_NewHour;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            visibilityController.makeVisible();
            timeTracker.restartTracking();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(background, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
        }

        protected void Event_NewHour(Object sender)
        {
            System.Media.SystemSounds.Exclamation.Play();
            notifyIcon.ShowBalloonTip(10000, "KUKU! KUKU!", "Wybiła godzina " + cuckoo.CurrentHour.ToString() + "!", ToolTipIcon.Info);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                visibilityController.makeVisible();
        }

        private void closeApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showApp_Click(object sender, EventArgs e)
        {
            visibilityController.makeVisible();
        }

        private void timeActive_Click(object sender, EventArgs e)
        {
            timeForm.displayTimeForm();
        }

        private string elapsedTimeFormat(long days, byte hours, byte minutes, byte seconds)
        {
            bool displayDays = (days > 0);
            bool displayHours = (displayDays || hours > 0);
            bool displayMinutes = (displayHours || minutes > 0);

            StringBuilder sb = new StringBuilder(200);

            sb.Append("Jestem Kukułką").Append(Environment.NewLine).Append("i działam już od:").Append(Environment.NewLine);
            if (displayDays) sb.Append(days.ToString()).Append((days != 1) ? " dni," : " dnia,").Append(Environment.NewLine);
            if (displayHours) sb.Append(hours.ToString()).Append((hours != 1) ? " godzin," : " godziny,").Append(Environment.NewLine);
            if (displayMinutes) sb.Append(minutes.ToString()).Append((minutes != 1) ? " minut i" : " minuty i").Append(Environment.NewLine);
            sb.Append(seconds.ToString()).Append((seconds != 1) ? " sekund." : " sekundy.").Append(Environment.NewLine);
            sb.Append(":)");

            return sb.ToString();
        }

        private ContextMenu createContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            menu.MenuItems.Add(new MenuItem("Pokaż mnie!", showApp_Click));
            menu.MenuItems.Add(new MenuItem("Pokaż czas mojego działania", timeActive_Click));
            menu.MenuItems.Add(new MenuItem("Zamknij mnie...", closeApp_Click));

            return menu;
        }

        private void additionalInit()
        {
            notifyIcon.ContextMenu = createContextMenu();
            background = new Bitmap("..\\..\\kukulka.png");
            ClientSize = new Size(background.Width << 1, background.Height << 1);
            TransparencyKey = BackColor;
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
