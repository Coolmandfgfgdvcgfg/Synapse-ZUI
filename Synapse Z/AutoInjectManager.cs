using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Synapse_Z
{
    public static class AutoInjectManager
    {
        private static CancellationTokenSource autoInjectCancellationTokenSource;
        private static SynapseZ form1Instance;

        public static void Initialize(SynapseZ form)
        {
            form1Instance = form;
        }

        public static void StartAutoInjectTask()
        {
            if (autoInjectCancellationTokenSource != null)
            {
                // Stop any existing task before starting a new one
                StopAutoInjectTask();
            }

            autoInjectCancellationTokenSource = new CancellationTokenSource();
            var token = autoInjectCancellationTokenSource.Token;
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (GlobalVariables.AutoInject)
                    {
                        var robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");
                        if (robloxProcesses.Length > 0)
                        {
                            var paths = form1Instance.GetOtherExecutablePaths();
                            if (paths != null && paths.Length > 0)
                            {
                                // Load existing PIDs from the PID file
                                string binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                                string pidFilePath = Path.Combine(binDirectory, "pid.txt");
                                var existingPids = System.IO.File.Exists(pidFilePath)
                                    ? System.IO.File.ReadAllLines(pidFilePath).Select(int.Parse).ToList()
                                    : new System.Collections.Generic.List<int>();

                                var newProcessIds = robloxProcesses
                                    .Where(p => !existingPids.Contains(p.Id) && !GlobalVariables.ExecutionPIDS.ContainsKey(p.Id.ToString()))
                                    .Select(p => p.Id)
                                    .ToArray();

                                if (newProcessIds.Length > 0)
                                {
                                    string launcherPath = paths.First();
                                    form1Instance.Inject(launcherPath, true, newProcessIds);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No paths found");
                            }
                        }
                    }
                    await Task.Delay(1000); // Check every second
                }
            }, token);
        }

        public static void StopAutoInjectTask()
        {
            autoInjectCancellationTokenSource?.Cancel();
        }
    }
}
