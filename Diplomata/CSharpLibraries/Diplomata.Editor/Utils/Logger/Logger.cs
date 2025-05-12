using System;
using System.Collections.Generic;

namespace Diplomata.Editor.Utils
{
    public class Logger : IUtil
    {
        private static Logger _instance = null;
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
        }

        private List<ITransport> _transports = new List<ITransport>();

        public Logger(ITransport[] transports = null)
        {
            if (transports != null)
            {
                _transports.AddRange(transports);
            }
        }

        public void AddTransport(ITransport transport)
        {
            _transports.Add(transport);
        }

        public void Log(string message)
        {
            foreach (var transport in _transports)
            {
                transport.Log(message);
            }
        }

        public void LogErr(Exception error)
        {
            foreach (var transport in _transports)
            {
                transport.LogErr(error);
            }
        }

        public void LogWarn(string message)
        {
            foreach (var transport in _transports)
            {
                transport.LogWarn(message);
            }
        }

        public void LogWarn(Exception error)
        {
            foreach (var transport in _transports)
            {
                transport.LogWarn(error);
            }
        }
    }
}