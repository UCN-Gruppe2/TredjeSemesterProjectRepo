using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class HourSpan : IComparable<HourSpan>
    {
        private int _hour;
        private int _minute;

        public int Hour
        {
            get => _hour;
        }

        public int Minute
        {
            get => _minute;
        }

        public HourSpan(int hour, int minute)
        {
            _hour = hour;
            _minute = minute;
        }

        public HourSpan(DateTime time)
        {
            _hour = time.Hour;
            _minute = time.Minute;
        }

        public override bool Equals(object obj)
        {
            return CompareTo((HourSpan)obj) == 0;
        }

        public int CompareTo(HourSpan other)
        {
            return ((other.Hour * 60) - other.Minute) - ((this.Hour * 60) - this.Minute);
        }
    }
}