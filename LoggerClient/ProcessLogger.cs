using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using LoggerClient;

namespace Logger
{
    class ProcessLogger
    {
        private static StringBuilder processStartStr = new StringBuilder();

        public static void run()
        {
            StringBuilder processStartStr = new StringBuilder();

            HashSet<Process> processesAtStart = new HashSet<Process>(Process.GetProcesses());
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

            while (true)
            {
                HashSet<Process> currentProcesses = new HashSet<Process>(Process.GetProcesses());
                HashSet<Process> buffSet = new HashSet<Process>(currentProcesses);
                currentProcesses.ExceptWith(processesAtStart);
                processesAtStart = buffSet;

                if (currentProcesses.Count >= 1)
                {
                    foreach (var process in currentProcesses)
                    {
                        DateTime processStartTimeUTC = process.StartTime.ToUniversalTime();
                        string processName = process.ProcessName;
                        string fullPath = process.MainModule.FileName;

                        processStartStr.AppendFormat("[{0}|{1}] | [PROCESS START] | " +
                            "Process : {2} | Path : {3}",
                            processStartTimeUTC.ToShortDateString(),
                            processStartTimeUTC.ToShortTimeString(),
                            processName,
                            fullPath
                           );

                        processStartStr.Append("\n");
                    }
                }

                Client.appendToBuffer(processStartStr);
                processStartStr.Clear();
                Thread.Sleep(500);
            }
        }

    }
}
