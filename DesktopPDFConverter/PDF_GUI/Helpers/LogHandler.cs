using System;
using System.IO;

namespace PDF_GUI.Helpers
{
    class LogHandler
    {
        public void WriteToLog(int fileCount)
        {
            var logText = $"{Environment.UserName} has converted {fileCount} file(s) to pdf on date: {DateTime.Now}";
            File.AppendAllText($"C:\\Users\\{Environment.UserName}\\Desktop\\log.txt", logText);

        }
    }
}
