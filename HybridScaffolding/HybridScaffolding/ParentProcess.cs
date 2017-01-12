using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HybridScaffolding
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ParentProcess
    {
        // These members must match PROCESS_BASIC_INFORMATION
        private IntPtr Reserved1;
        private IntPtr PebBaseAddress;
        private IntPtr Reserved2_0;
        private IntPtr Reserved2_1;
        private IntPtr UniqueProcessId;
        private IntPtr InheritedFromUniqueProcessId;

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcess processInformation, int processInformationLength, out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// Gets the parent process of the current process.
        /// </summary>
        /// <returns>An instance of the Process class.</returns>
        private static Process GetParentProcess()
        {
            return GetParentProcess(Process.GetCurrentProcess().Handle);
        }

        /// <summary>
        /// Gets the parent process of specified process.
        /// </summary>
        /// <param name="id">The process id.</param>
        /// <returns>An instance of the Process class.</returns>
        private static Process GetParentProcess(int id)
        {
            Process process = Process.GetProcessById(id);
            return GetParentProcess(process.Handle);
        }

        /// <summary>
        /// Gets the parent process of a specified process.
        /// </summary>
        /// <param name="handle">The process handle.</param>
        /// <returns>An instance of the Process class.</returns>
        private static Process GetParentProcess(IntPtr handle)
        {
            ParentProcess pbi = new ParentProcess();
            int returnLength;
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
            if (status != 0)
                throw new Win32Exception(status);

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                // not found
                return null;
            }
        }
        /// <summary>
        /// Gathers the run type and attaches the console for the selected run type.
        /// </summary>
        /// <returns>The RunType of the application</returns>
        internal static RunTypes ConsoleScaffolding()
        {
            var command = GetParentProcess();
            var process = GetParentProcess(command.Id);
            var runType = RunTypes.Console;
            try
            {
                if (process.ProcessName == "cmd" || process.ProcessName.Contains("powershell"))
                {
                    //Octopus running seems to require this
                    AttachConsole(process.Id);
                    runType = RunTypes.Console;
                }
                else if (command.ProcessName == "cmd" || command.ProcessName.Contains("powershell"))
                {
                    //running from cmd or posh locally
                    AttachConsole(-1);
                    runType = RunTypes.Console;
                }
                else if (process.ProcessName == "explorer" || process.ProcessName == "svchost")
                {
                    runType = RunTypes.Gui;
                }
                else if(command.ProcessName == "explorer" || command.ProcessName == "svchost")
                {
                    runType = RunTypes.Gui;
                }
                else
                {
                    //no console AND we're in console mode ... create a new console.
                    AllocConsole();
                    runType = RunTypes.Console;
                }
            }
            catch
            {
                AttachConsole(-1);
                runType = RunTypes.Console;
            }
            return runType;
        }
    }
}
