using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace NETKukulka
{
    public class ElapsedTimeTracker
    {
        public long ElapsedMilliseconds { get { return watch.ElapsedMilliseconds; } }
        public long ElapsedSeconds { get { return ElapsedMilliseconds / 1000; } }
        public long ElapsedMinutes { get { return ElapsedMilliseconds / 60000; } }
        public long ElapsedHours { get { return ElapsedMilliseconds / 3600000; } }
        public long ElapsedDays { get { return ElapsedMilliseconds / 86400000; } }

        public string ElapsedTimeFormatted
        {
            get
            {
                long elapsed = ElapsedSeconds;

                long days = elapsed / 86400;
                byte hours = (byte)((elapsed -= (days * 86400)) / 3600);
                byte minutes = (byte)((elapsed -= (hours * 3600)) / 60);
                byte seconds = (byte)(elapsed - (minutes * 60));

                return timeFormatter(days, hours, minutes, seconds);
            }
        }

        private Stopwatch watch;
        private Func<long, byte, byte, byte, string> timeFormatter;

        public ElapsedTimeTracker(bool start)
        {
            resetTimeFormatter();
            watch = new Stopwatch();
            if (start) resumeTracking();
        }

        public void resumeTracking()
        {
            watch.Start();
        }

        public void restartTracking()
        {
            watch.Restart();
        }

        public void resetTracking()
        {
            watch.Reset();
        }

        public void stopTracking()
        {
            watch.Stop();
        }

        public void setTimeFormatter(Func<long, byte, byte, byte, string> formatter)
        {
            timeFormatter = formatter;
        }

        public void resetTimeFormatter()
        {
            timeFormatter = defaultTimeFormatter;
        }

        protected string defaultTimeFormatter(long days, byte hours, byte minutes, byte seconds)
        {
            StringBuilder sb = new StringBuilder(200);

            sb.Append("Days elapsed: ").Append(days.ToString()).Append(",").Append(Environment.NewLine);
            sb.Append("Hours elapsed: ").Append(hours.ToString()).Append(",").Append(Environment.NewLine);
            sb.Append("Minutes elapsed: ").Append(minutes.ToString()).Append(",").Append(Environment.NewLine);
            sb.Append("Seconds elapsed: ").Append(seconds.ToString()).Append(".");

            return sb.ToString();
        }
    }
}
