using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETKukulka
{
    public delegate void CuckooEventHandler(Object sender);

    public class Cuckoo
    {
        public int CurrentHour { get; private set; }

        public event CuckooEventHandler NewHour;
        private Timer timer;

        public Cuckoo()
        {
            CurrentHour = DateTime.Now.Hour;
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Event_Tick;
            timer.Start();
        }

        protected void Event_Tick(Object sender, EventArgs e)
        {
            int newHour = DateTime.Now.Hour;
            if (CurrentHour != newHour)
            {
                CurrentHour = newHour;
                NewHour(this);
            }
        }
    }
}
