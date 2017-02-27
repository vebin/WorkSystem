using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ymatou.Infrastructure
{
   public static  class DateTimeExtensions
    {
       public static DateTime VerifyTimeRange(this DateTime dt,DateTime start,DateTime end,DateTime defaultTime,bool throwOut=false)
       {
           if (dt < start || dt > end)
           {
               if (throwOut) throw new ArgumentException("给定的时间不在指定的时间范围内");
               else return defaultTime;
           }
           else
           {
               return dt;
           }
       }
    }
}
