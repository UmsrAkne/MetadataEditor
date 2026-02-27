using System;
using System.IO;

namespace MetadataEditor.Utils
{
    public static class LogWriter
    {
        private const string LogFileName = "log.txt";

        public static void Write(string text)
        {
            // Ensure directory exists
            Directory.CreateDirectory(AppPaths.LocalDataDirectory);

            var logPath = Path.Combine(AppPaths.LocalDataDirectory, LogFileName);

            using TextWriter tw = new StreamWriter(logPath, true);
            tw.WriteLine(string.Empty);
            tw.WriteLine("----------");
            tw.WriteLine($"TimeStamp: {DateTime.Now}");
            tw.WriteLine(text);
        }

        private static class AppPaths
        {
            public static string LocalDataDirectory => Path.Combine(BaseDirectory, "local_data");

            private static string BaseDirectory => AppContext.BaseDirectory;
        }
    }
}