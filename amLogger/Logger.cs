using System.Diagnostics;

namespace amLogger
{
    public class Log
    {
        public int id = 0; // Exchange ID, default none - 0
        public int lvl = 0; // Error level, default Trace - 0
        public int type; // Type of message, reserved
        public string src = ""; // Source
        public string msg = ""; // Message
        public void Send()
        {
            Logger.Write(this);
        }
        public static void Trace(string source, string message)
        {
            Logger.Write(new Log() { src = source, msg = message });
        }
        public static void Trace(int exch_id, string source, string message)
        {
            Logger.Write(new Log() { id = exch_id, src = source, msg = message});
        }
        public static void Info(string source, string message)
        {
            Logger.Write(new Log() { lvl = 2, src = source, msg = message });
        }
        public static void Info(int exch_id, string source, string message)
        {
            Logger.Write(new Log() { id = exch_id, lvl = 2, src = source, msg = message });
        }
        public static void Error(string source, string message)
        {
            Logger.Write(new Log() { lvl = 4, src = source, msg = message });
        }
        public static void Error(int exch_id, string source, string message)
        {
            Logger.Write(new Log() { id = exch_id, lvl = 4, src = source, msg = message });
        }
        public static void Fatal(string source, string message)
        {
            Logger.Write(new Log() { lvl = 5, src = source, msg = message });
        }
        public static void Fatal(int exch_id, string source, string message)
        {
            Logger.Write(new Log() { id = exch_id, lvl = 5, src = source, msg = message });
        }
    }
    public class Logger
    {
        static Action<Log>? _write;
        public static void Write(Log msg) => _write?.Invoke(msg);

        static object _lockFlag = new object();
        static Logger? _instance;

        public static Logger Instance
        {
            get
            {
                lock (_lockFlag)
                {
                    if (_instance == null)
                        _instance = new Logger();
                }
                return _instance;
            }
        }
        public void Init(Action<Log> write)
        {
            _write = write;
        }
    }
}