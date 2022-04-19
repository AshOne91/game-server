using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Service.Core
{
    public enum ELogLevel
    {
        Trace = 1,
        Warn,
        Err,
        Fatal,
        Always
    }

    public interface ILogger
    {
        void Log(ELogLevel level, string msg);
        void Log(ELogLevel level, string format, params object[] args);
        void SetLevel(ELogLevel logLevel);
    }

    public sealed class Logger : ILogger
    {
        public static Logger Default;

        private FlipQueue<string> _logQueue;
        private EventWaitHandle _eventWait;
        private string _logFileName;
        private string _logFileFullPath;
        private Thread _thread;
        private ELogLevel _logLevel;
        private bool _running;
        private bool _useConsoleLog = false;
        private static string _crashReportUrl;
        ~Logger()
        {
            Destroy();
        }
        public void Destroy()
        {
            _running = false;
            if (_eventWait != null)
            {
                _eventWait.Set();
            }
        }
        public void Create(bool useConsole, string prefix, string folder = "/log")
        {
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + folder);
            _useConsoleLog = useConsole;
            _logFileName = String.Format("{0},{1}", prefix + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd_HH_mm_ss"), "log");
            _logFileFullPath = System.IO.Path.Combine(Environment.CurrentDirectory + folder, _logFileName);

            _logQueue = new FlipQueue<string>();
            _thread = new Thread(Run);
            _thread.Start();
        }
        public void Log(ELogLevel level, string format, params object[] args)
        {
            if (level < _logLevel || _running == false)
                return;

            string log = string.Format(format, args);

            if (_useConsoleLog)
            {
                PrintConsole(level, log);
            }

            WriteFileLog(level, log);

            if (level == ELogLevel.Fatal)
                throw new FatalException(log);
        }
        private static void PrintConsole(ELogLevel level, string log)
        {
            switch (level)
            {
                case ELogLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ELogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ELogLevel.Err:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ELogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case ELogLevel.Always:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

            }
            Console.WriteLine(log);
        }
        private void Run()
        {
            _running = true;
            _eventWait = new EventWaitHandle(false, EventResetMode.AutoReset);

            while (_running)
            {
                if (_eventWait.WaitOne(1000))
                {
                    Queue<string> queue = _logQueue.GetQueue();

                    if (queue.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.EnsureCapacity((queue.Count + 1) * 64);
                        foreach (string task in queue)
                        {
                            sb.Append(task);
                            sb.Append("\r\n");
                        }
                        System.IO.File.AppendAllText(_logFileFullPath, sb.ToString());
                        queue.Clear();
                    }
                }
            }
        }
        public void Log(ELogLevel level, string log)
        {
            if (level < _logLevel)
                return;

            DateTime now = DateTime.Now;
            log = string.Format("[{0}/{1}/{2}-{3}:{4}:{5}] : {6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, log);
            if (_useConsoleLog)
                PrintConsole(level, log);

            WriteFileLog(level, log);

            if (level == ELogLevel.Fatal)
            {
                throw new FatalException(log);
            }
        }
        public void SetLevel(ELogLevel level)
        {
            _logLevel = level;
        }
        public void SetCrashReportUrl(string url)
        {
            _crashReportUrl = url;
        }
        public ELogLevel GetLogLevel()
        {
            return _logLevel;
        }
        private void WriteFileLog(ELogLevel level, string log)
        {
            if (_logQueue != null && _eventWait != null)
            {
                _logQueue.Enqueue(LogFormat(level, log));
                _eventWait.Set();
            }
        }
        private string LogFormat(ELogLevel level, string log)
        {
            return String.Format("[{0}][{1}] : {2}", DateTime.Now.ToString("hh:mm:ss"), level, log) + Environment.NewLine;
        }
        public static void WriteExceptionLog(Exception e, string folder = "/exception")
        {
            using (System.IO.StreamWriter Writer = _GetLogWriter("Exception", folder))
            {
                Writer.WriteLine("=============Error Logging ===========");
                Writer.WriteLine("Time : " + DateTime.Now);
                Writer.WriteLine("Source Name : " + e.Source);
                Writer.WriteLine("Exception Message : " + e.Message);
                Writer.WriteLine("Stack Trace:" + Environment.NewLine + e.StackTrace);

                PrintConsole(ELogLevel.Fatal, "=============Error Logging ===========");
                PrintConsole(ELogLevel.Fatal, "Time : " + DateTime.Now);
                PrintConsole(ELogLevel.Fatal, "Source Name : " + e.Source);
                PrintConsole(ELogLevel.Fatal, "Exception Message : " + e.Message);
                PrintConsole(ELogLevel.Fatal, "Stack Trace:" + Environment.NewLine + e.StackTrace);
            }

            SendExceptionReport(e);
        }
        public static void WriteFileLog(string msg, string folder)
        {
            using (System.IO.StreamWriter Writer = _GetLogWriter("Exception", folder))
            {
                Writer.WriteLine("=============Error Logging ===========");
                Writer.WriteLine("Time : " + DateTime.Now);
                Writer.WriteLine(msg);
                Writer.WriteLine("Stack Trace:" + Environment.NewLine + Environment.StackTrace);

                PrintConsole(ELogLevel.Fatal, "=============Error Logging ===========");
                PrintConsole(ELogLevel.Fatal, "Time : " + DateTime.Now);
                PrintConsole(ELogLevel.Fatal, msg);
                PrintConsole(ELogLevel.Fatal, "Stack Trace:" + Environment.NewLine + Environment.StackTrace);
            }
        }
        private static System.IO.StreamWriter _GetLogWriter(string prefix, string folder)
        {
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + folder);
            string FileName = string.Format("{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMdd"), prefix, "log");
            string FileFullPath = System.IO.Path.Combine(Environment.CurrentDirectory + folder, FileName);
            return System.IO.File.AppendText(FileFullPath);
        }

        public static void SendExceptionReport(Exception e)
        {
            if (_crashReportUrl == null || _crashReportUrl.Length <= 0)
                return;

            JObject crash_data = new JObject();
            crash_data.Add("reg_date", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
            crash_data.Add("ip", GetLocalIPAddress());
            crash_data.Add("error_source", e.Source);
            crash_data.Add("error_message", e.Message);
            crash_data.Add("error_stack", e.StackTrace);

            string serialized_data = JsonConvert.SerializeObject(crash_data);

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(serialized_data);

            WebRequest request = WebRequest.Create(_crashReportUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            request.Timeout = 30 * 1000;

            System.IO.Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Response 대기
            request.GetResponse();
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
    }
}
