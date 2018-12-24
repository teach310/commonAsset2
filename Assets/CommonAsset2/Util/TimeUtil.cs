using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CA2 {
    public class TimeUtil {

        private static DateTime UnixEpoch = new DateTime (1970, 1, 1, 0, 0, 0, 0);

        public static long GetCurrentUnixTime(){
            return GetUnixTime(DateTime.Now);
        }

        // DateTimeからUnixTimeへ変換
        public static long GetUnixTime (DateTime dateTime) {
            return (long) (dateTime - UnixEpoch).TotalSeconds;
        }

        // UnixTimeからDateTimeへ変換
        public static DateTime GetDateTime (long unixTime) {
            return UnixEpoch.AddSeconds (unixTime);
        }
    }
}