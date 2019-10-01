using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HybridScaffolding.Constants;
using HybridScaffolding.Enums;
using HybridScaffolding.Models;

namespace HybridScaffolding.Workers
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
        /// Always pick the process over the command
        /// </summary>
        /// <returns>The ProcessInfo</returns>
        internal static ProcessInfo ConsoleScaffolding()
        {
            var command = GetParentProcess();
            var process = GetParentProcess(command.Id);
            var runType = RunTypes.Console;


            try
            {
                if (process != null && process.ProcessName == ResourceStrings.CmdProcessName)
                {
                    AttachConsole(process.Id);
                    runType = RunTypes.Console;
                }
                else if (command.ProcessName == ResourceStrings.CmdProcessName)
                {
                    AttachConsole(-1);
                    runType = RunTypes.Console;
                }
                else if (process != null && process.ProcessName.Contains(ResourceStrings.PowerShellProcessName))
                {
                    AttachConsole(process.Id);
                    runType = RunTypes.Powershell;
                }
                else if (command.ProcessName.Contains(ResourceStrings.PowerShellProcessName))
                {
                    AttachConsole(-1);
                    runType = RunTypes.Powershell;
                }
                else if (process != null && process.ProcessName.Contains(ResourceStrings.PwshProcessName))
                {
                    AttachConsole(process.Id);
                    runType = RunTypes.Powershell;
                }
                else if (command.ProcessName.Contains(ResourceStrings.PwshProcessName))
                {
                    AttachConsole(process.Id);
                    runType = RunTypes.Powershell;
                }
                else if (process != null && (process.ProcessName == ResourceStrings.ExplorerProcessName || process.ProcessName == ResourceStrings.SvcHostProcessName || process.ProcessName == ResourceStrings.UserInitProcessName || process.ProcessName == ResourceStrings.DevEnvProcessName || process.ProcessName == ResourceStrings.IisExpressProcessName))
                {
                    runType = RunTypes.Gui;
                }
                else if (command.ProcessName == ResourceStrings.ExplorerProcessName || command.ProcessName == ResourceStrings.SvcHostProcessName || command.ProcessName == ResourceStrings.UserInitProcessName || command.ProcessName == ResourceStrings.MsVsMonProcessName || command.ProcessName == ResourceStrings.VsIisLaucherProcessName || command.ProcessName == ResourceStrings.W3wpProcessName)
                {
                    runType = RunTypes.Gui;
                }
                else
                {
                    AllocConsole();
                    runType = RunTypes.Console;
                }
            }
            catch
            {
                AttachConsole(-1);
                runType = RunTypes.Console;
            }
            var processInfo = new ProcessInfo();
            processInfo.RunType = runType;
            processInfo.CommandName = command.ProcessName;
            processInfo.ProcessName = process.ProcessName;

            return processInfo;
        }
    }
}
