using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;

namespace Synapse_Z
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check for existing instance of Synapse Launcher.exe
            var currentProcess = Process.GetCurrentProcess();
            var runningProcesses = Process.GetProcessesByName(currentProcess.ProcessName)
                                          .Where(p => p.Id != currentProcess.Id && p.MainModule.FileName.Equals(currentProcess.MainModule.FileName, StringComparison.OrdinalIgnoreCase));

            foreach (var process in runningProcesses)
            {
                // Terminate the existing instance
                process.Kill();
                process.WaitForExit();
            }

            // Add the event handler for resolving assemblies
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SynapseZ());
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Define the path to the lib folder
            string libFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib");

            // Get the assembly name
            string assemblyName = new AssemblyName(args.Name).Name + ".dll";
            string assemblyPath = Path.Combine(libFolderPath, assemblyName);

            // Load the assembly if it exists in the lib folder
            if (File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }

            // If the assembly is not found, return null
            return null;
        }
    }
}
