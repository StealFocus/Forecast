namespace StealFocus.Forecast
{
    using System;

    internal struct ScheduleDay : IEquatable<ScheduleDay>
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public static bool operator ==(ScheduleDay scheduleDay1, ScheduleDay scheduleDay2)
        {
            return scheduleDay1.Equals(scheduleDay2);
        }

        public static bool operator !=(ScheduleDay scheduleDay1, ScheduleDay scheduleDay2)
        {
            return !scheduleDay1.Equals(scheduleDay2);
        }

        public bool Equals(ScheduleDay other)
        {
            return this.DayOfWeek == other.DayOfWeek && this.StartTime.Equals(other.StartTime) && this.EndTime.Equals(other.EndTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is ScheduleDay && this.Equals((ScheduleDay)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)this.DayOfWeek;
                hashCode = (hashCode * 397) ^ this.StartTime.GetHashCode();
                hashCode = (hashCode * 397) ^ this.EndTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
