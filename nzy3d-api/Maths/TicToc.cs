using System;
namespace nzy3D.Maths
{
    public class TicToc
    {
        internal DateTime tstart;

        internal DateTime tstop;
        public void tic()
        {
            tstart = DateTime.Now;
        }

        public double toc()
        {
            tstop = DateTime.Now;
            return elapsedSecond;
        }

        public TimeSpan elapsedTimeSpan
        {
            get { return tstop - tstart; }
        }

        public double elapsedMillisecond
        {
            get { return elapsedTimeSpan.TotalMilliseconds; }
        }

        public double elapsedSecond
        {
            get { return elapsedTimeSpan.TotalSeconds; }
        }

    }
}