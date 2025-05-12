using System;

namespace Diplomata.Editor.Utils {
    public interface ITransport {
        public void Log(string message);
        public void LogWarn(string message);
        public void LogWarn(Exception error);
        public void LogErr(Exception error);
    }
}