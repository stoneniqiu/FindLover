using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Findlover.Helper
{
    public class Helpers
    {
        public static string GetMd5Code(string text)
        {
            var result = Encoding.Default.GetBytes(text);
            MD5 md5=new MD5CryptoServiceProvider();
            var oBytes = md5.ComputeHash(result);
            return BitConverter.ToString(oBytes).Replace("-", "");  
        }

        /// <summary>
        /// 简化时间
        /// </summary>
        /// <returns></returns>
        public static string SimpleTime(DateTime time)
        {
            if (time.DayOfYear == DateTime.Today.DayOfYear)
            {
                return "今天 " + time.Hour + "：" + time.Minute;
            }
            if (time.DayOfYear == DateTime.Today.DayOfYear-1)
            {
                return "昨天 " + time.Hour + "：" + time.Minute;
            }
            return time.ToShortDateString();
        }

        /// <summary>
        /// 计算两个时间差
        /// </summary>
        /// <param name="beforeTime"></param>
        /// <param name="afterTime"></param>
        /// <returns>返回分钟数</returns>
        public static double DiffMinute(DateTime beforeTime, DateTime afterTime)
        {
            TimeSpan timeSpan = afterTime - beforeTime;
            return timeSpan.TotalMinutes;
        }
    }
}