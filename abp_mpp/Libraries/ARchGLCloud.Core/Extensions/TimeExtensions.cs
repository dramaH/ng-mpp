using System;

namespace ARchGLCloud.Core.Extensions
{
    public static class TimeExtensions
    {
        public static long UnixTime(this DateTime nowTime)
        {
            var now = TimeZoneInfo.ConvertTimeToUtc(nowTime, TimeZoneInfo.Local);
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (long)Math.Round((now - start).TotalMilliseconds, MidpointRounding.AwayFromZero);
        }

        public static string TimeAgo(this DateTime dt)
        {
            TimeSpan span = (DateTime.Now - dt).Duration();
            if (span.TotalDays > 365)
            {
                return span.TotalDays / 365 + "年前";
            }
            else if (span.TotalDays > 60)
            {
                return span.TotalDays / 30 + "个月前";
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }
    }
}