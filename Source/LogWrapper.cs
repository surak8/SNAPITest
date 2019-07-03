using Colt3.Logging;
using Colt3.Utility;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using static Colt3.Logging.Logger;

namespace NSSNAPITest {
    static class LogWrapper {

        static Logger _logger;
        static bool _didInit;
        static ConfigurationList _cfg;

        static void init() {
            string asm;

            if (!_didInit) {
                _didInit = true;

                /*
                 * 	_OutputDirectory = m_config.getValue("Log Output Directory", @"c:\Colt\\Logs\");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(199):            m_doubleBuffer = m_config.getValue("double Buffer", false);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(204):                m_rollingSizeMessageCount = 10000 * m_config.getValue("Log File Bytes", 10);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(205):                m_logCount = m_config.getValue("Log Archive Depth", 80);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(209):                _ProcessName = m_config.getValue("Process Name", "Unknown Process");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(210):                m_logLevel = m_config.getValue("File Logging Enabled", 3);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(211):                m_format = m_config.getValue("Log Format", "{ticks} {Timestamp:MM-dd-yyyy HH:mm:ss.ffff} ({Thread}) - [{LevelName}] {Message}");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(213):                m_matchSCLineFormat = m_config.getValue("Match SC Line Format", false);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(231):            string tmp = m_config.getValue("Log To Trace", "false");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(301):            m_logLevel = m_config.getValue("File Logging Enabled", 3);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(317):            m_logCount = m_config.getValue("Log Archive Depth", DEFAULT_LOG_DEPTH);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(318):            int logFileBytes = m_config.getValue("Log File Bytes", 3);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(320):            _ProcessName = m_config.getValue("Process Name", "Unknown Process");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(327):            _OutputDirectory = m_config.getValue("Log Output Directory", @"c:\Colt\Logs\");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(331):            m_format = m_config.getValue("Log Format", "{ticks} {Timestamp:MM-dd-yyyy HH:mm:ss.ffff} ({Thread}) - [{LevelName}] {Message}");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(333):            m_matchSCLineFormat = m_config.getValue("Match SC Line Format", false);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(680):                _ProcessName = m_config.getValue("Process Name", "Unknown Process");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(687):                m_format = m_config.getValue("Log Format", "{ticks} {Timestamp:MM-dd-yyyy HH:mm:ss.ffff} ({Thread}) - [{LevelName}] {Message}");
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(689):                m_matchSCLineFormat = m_config.getValue("Match SC Line Format", false);
  C:\Users\RTCOUSENS\colt\NewProjects\Colt3\Colt3\Logger.cs(692):                outfile.WriteLine(_ProcessName + ": Log initialized - Log Level: " + logLevelName() + ", Log File Bytes: " + m_config.getValue("Log F
                 * */
                _cfg = new ConfigurationList();
                asm = Assembly.GetEntryAssembly().GetName().Name;
#if DEBUG
                _cfg.Add("File Logging Enabled", "5");
#endif
                _cfg.Add("Log To Trace", "true");
                _cfg.Add("Process Name", asm);
                _cfg.Add("Log Output Directory",
                    Path.Combine(
                        @"\\appdeploy\APPDEPLOY\Colt Software\Logs",
                        asm ));

                _logger = new Logger(_cfg);
            }
        }
        internal static void log(MethodBase mb, string msg) {
            log(Level.Debug, mb, msg);
        }
        internal static void log(MethodBase mb, Exception  ex) {
            log(Level.Debug,exceptionValue(ex)+Environment.NewLine+ex.StackTrace);
        }

          static string exceptionValue(Exception ex) {
            StringBuilder sb = new StringBuilder();
            Exception ex0 = ex;

            sb.Append("[" + ex.GetType().Name + "] " + ex.Message);
            while (ex0.InnerException!=null) {
                ex0 = ex0.InnerException;
                sb.Append("[" + ex.GetType().Name + "] " + ex.Message);
            }
            return sb.ToString();

        }

        static void log(int logLevel, MethodBase mb, string msg) {
            if (string.IsNullOrEmpty(msg))
                log(logLevel, makeSig(mb));
            else
                log(logLevel, makeSig(mb) + " : " + msg);
        }

        internal static void log(string msg) {
            log(Level.Debug, msg);
        }
        internal static void log(MethodBase mb) {
            log(Level.Debug, mb, null);
        }


        static void log(int logLevel, string msg) {
            init();
            _logger.write(logLevel, msg);
        }

        internal static void logError(string msg) {
            log(Level.Error, msg);
        }

        internal static void logError(MethodBase mb, string msg) {
            log(Level.Error, mb, msg);
        }

        internal static string makeSig(MethodBase mb) {
            return mb.ReflectedType.Name + "." + mb.Name;
        }

        internal static void logInfo(string msg) {
            log(Level.Info, msg);
        }

        internal static void logWarning(string msg) {
            log(Level.Warning , msg);
        }

        //internal static void log(string v) {
        //    throw new NotImplementedException();
        //}
    }


}

#if !USE_COLT3
#if true
namespace Colt3 {
    namespace Logging {
        public class Logger {
            ConfigurationList _cfg;

            public Logger(ConfigurationList cfg) {
                _cfg = cfg;
            }

            internal void write(int logLevel, string msg) {
                Trace.WriteLine("(" + logLevel + ") " + msg);
            }

            public struct Level {
                public const int UNKNOWN = -1;
                public const int Error = 0;
                public const int Warning = 1;
                public const int Info = 2;
                public const int Debug = 3;
            }
        }
    }
    namespace Utility {
        public class ConfigurationList {
            internal void Add(string v1, string v2) {
                Trace.WriteLine( "Add: " + v1 + " = " + v2);
            }
        }
    }
}
#endif
#endif