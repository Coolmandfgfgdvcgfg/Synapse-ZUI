using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
                    if (GlobalVariables.AutoInject && !GlobalVariables.injecting && !GlobalVariables.isDone)
                    {
                        if (IsRobloxPlayerBetaRunning())
                        {
                            var paths = form1Instance.GetOtherExecutablePaths();
                            if (paths != null && paths.Length > 0)
                            {
                                foreach (var path in paths)
                                {
                                    form1Instance.Inject(path, true);
                                }
                            }
                            else
                            {
                                MessageBox.Show("no paths found");
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

        private static bool IsRobloxPlayerBetaRunning()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Length > 0;
        }
    }
}
