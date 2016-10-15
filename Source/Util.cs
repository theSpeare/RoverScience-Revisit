using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
    public static class Util
    {
    }

    public static class RSDebug
    {
        // logLevelCurrent
        // 0 - NO LOGGING
        // 1 - log only the prioritized
        // 2 - also log somewhat important stuff
        // 3 - also log not important stuff

        private const int logRankCurrent = 1;
        private const bool logWithTime = true;

        public static void Log(string _s, int _logRank = 1)
        {
            if (_logRank <= logRankCurrent)
            {
                string RS_prefix = "[RS]: ";
                string time_suffix = logWithTime ? " | @ <" + DateTime.Now + ">" : "";
                Debug.Log(RS_prefix + _s + time_suffix);
            }
        }
    }
}
