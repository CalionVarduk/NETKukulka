using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETKukulka
{
    public class VisibilityController
    {
        
        public int TimerInterval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        public long VisibleMilliseconds { get { return (long)TimerInterval * (long)TicksCount; } }
        public bool Visible { get { return form.Visible; } }

        public int TicksLeft { get { return TicksCount - timeTick; } }
        public long MillisecondsLeft { get { return (long)TicksLeft * (long)TimerInterval; } }

        public int TicksCount { get; set; }

        private int timeTick;
        private Timer timer;
        private Form form;

        public VisibilityController(Form form, int ticksCount, int timerInterval)
        {
            this.form = form;
            timer = new Timer();
            timer.Interval = timerInterval;
            timer.Tick += Event_Tick;

            TicksCount = ticksCount;
            timeTick = 0;
        }

        public void makeVisible()
        {
            timeTick = 0;
            form.Visible = true;
            if (!timer.Enabled) timer.Start();
        }

        protected virtual void Event_Tick(Object sender, EventArgs e)
        {
            if(++timeTick > TicksCount)
            {
                timeTick = TicksCount;
                form.Hide();
                timer.Stop();
            }
        }
    }
}
