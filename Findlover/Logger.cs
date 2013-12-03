using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Findlover
{
    public static class Logger
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger("Logger");
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Trace(string message)
        {
            logger.Trace(message);
        }

        public static void Debug(string message)
        {
            logger.Debug(message);
        }

        public static void Info(string message)
        {
            logger.Info(message);
        }

        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        public static void Error(string message)
        {
            logger.Error(message);
        }

        public static void Fatal(string message)
        {
            logger.Fatal(message);
        }
    }
}