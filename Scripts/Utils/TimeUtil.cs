using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UDK
{
    public static class TimeUtil
    {
        public const int TickToSecond = 10000000;
        public static readonly DateTime TIME1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// 获取服务器所在时区
        /// </summary>
        public static TimeZoneInfo GetServerTimeZone()
        {
            // 服务器统一使用GMT时间，Utc + 0
            return TimeZoneInfo.Utc;
        }

        /// <summary>
        /// 获取本地时区
        /// </summary>
        public static TimeZoneInfo GetLocalTimeZone()
        {
            return TimeZoneInfo.Local;
        }

        /// <summary>
        /// 获取服务器时区与本地时区的时差
        /// </summary>
        public static long GetOffset()
        {
            return System.Math.Abs(TimeSpanToTick(GetServerTimeZone().BaseUtcOffset - GetLocalTimeZone().BaseUtcOffset));
        }

        public static DateTime ConvertGMT8StrToLocalTime(string str)
        {
            DateTime dt = DateTime.UtcNow;
            DateTime.TryParse(str, out dt);
            dt = dt.AddHours(-8);
            dt = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, GetLocalTimeZone());
            return dt;
        }

        public static DateTime ConvertTimeZone(DateTime dateTime, TimeZoneInfo src, TimeZoneInfo dest)
        {
            return TimeZoneInfo.ConvertTime(dateTime, src, dest);
        }

        /// <summary>
        /// 将时间戳转换到服务器时区
        /// </summary>
        public static DateTime ConvertToServerTime(long t)
        {
            return TimeZoneInfo.ConvertTime(TickToDateTime(t), TimeZoneInfo.Utc, GetServerTimeZone());
        }

        /// <summary>
        /// 将时间戳转换到本地时区
        /// </summary>
        public static DateTime ConvertToLocalTime(long t)
        {
            return TimeZoneInfo.ConvertTime(TickToDateTime(t), TimeZoneInfo.Utc, GetLocalTimeZone());
        }

        /// <summary>
        /// 将本地时间转换到服务器时区
        /// </summary>
        public static DateTime ConvertToServerTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, GetLocalTimeZone(), GetServerTimeZone());
        }

        /// <summary>
        /// 将服务器时间转换到本地时区
        /// </summary>
        public static DateTime ConvertToLocalTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, GetServerTimeZone(), GetLocalTimeZone());
        }

        /// <summary>
        /// 将服务器时间转换到时间戳
        /// </summary>
        public static long ConvertLocalTimeToTick(DateTime dateTime)
        {
            return DateTimeToTick(TimeZoneInfo.ConvertTime(dateTime, GetLocalTimeZone(), TimeZoneInfo.Utc));
        }

        /// <summary>
        /// 将本地时间转换到时间戳
        /// </summary>
        public static long ConvertServerTimeToTick(DateTime dateTime)
        {
            return DateTimeToTick(TimeZoneInfo.ConvertTime(dateTime, GetServerTimeZone(), TimeZoneInfo.Utc));
        }

        public static DateTime TickToDateTime(long t)
        {
            return new DateTime(TIME1970.Ticks + (long)((double)t * TickToSecond), DateTimeKind.Utc);
        }

        /// <summary>
        /// 将date转换为时间戳，date需要是UTC时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long DateTimeToTick(DateTime date)
        {
            return (long)((double)(date.Ticks - TIME1970.Ticks) / TickToSecond);
        }

        public static long TimeSpanToTick(TimeSpan time)
        {
            return (long)((double)(time.Ticks) / TickToSecond);
        }
    }
}


