using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HybridScaffolding.Constants;
using HybridScaffolding.Enums;
using HybridScaffolding.Models;

namespace HybridScaffolding.Workers
{
    /// <summary>
    /// Provides functionality to determine and interact with the parent process of the current process.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct ParentProcess
    {
        private readonly IntPtr Reserved1;
        private readonly IntPtr PebBaseAddress;
        private readonly IntPtr Reserved2_0;
        private readonly IntPtr Reserved2_1;
        private readonly IntPtr UniqueProcessId;
        private readonly IntPtr InheritedFromUniqueProcessId;

        /// <summary>
        /// Calls the Windows API to retrieve information about a process.
        /// </summary>
        /// <param name="processHandle">The handle to the process.</param>
        /// <param name="processInformationClass">The class of information to retrieve.</param>
        /// <param name="processInformation">The process information that is returned.</param>
        /// <param name="processInformationLength">The length of the process information.</param>
        /// <param name="returnLength">The length of the data returned.</param>
        /// <returns>An NTSTATUS code indicating the result of the call.</returns>
        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcess processInformation, int processInformationLength, out int returnLength);

        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>true if the function succeeds; otherwise, false.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        /// <summary>
        /// Attaches the calling process to the console of the specified process.
        /// </summary>
        /// <param name="dwProcessId">The identifier of the process whose console is to be used.</param>
        /// <returns>true if the function succeeds; otherwise, false.</returns>
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// Gets the parent process of the current process.
        /// </summary>
        /// <returns>The parent process.</returns>
        private static Process GetParentProcess()
        {
            return GetParentProcess(Process.GetCurrentProcess().Handle);
        }

        /// <summary>
        /// Gets the parent process of a specified process by ID.
        /// </summary>
        /// <param name="id">The process ID.</param>
        /// <returns>The parent process.</returns>
        private static Process GetParentProcess(int id)
        {
            try
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the parent process of a specified process by handle.
        /// </summary>
        /// <param name="handle">The process handle.</param>
        /// <returns>The parent process.</returns>
        private static Process GetParentProcess(IntPtr handle)
        {
            ParentProcess pbi = new ParentProcess();
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out _);
            if (status != 0)
                throw new Win32Exception(status);

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Determines the appropriate console scaffolding based on the parent process and the default run type.
        /// </summary>
        /// <param name="defaultRunType">The default run type.</param>
        /// <returns>Information about the process and its run type.</returns>
        internal static ProcessInfo ConsoleScaffolding(RunType defaultRunType)
        {
            var processInfo = new ProcessInfo(defaultRunType);
            try
            {
                var command = GetParentProcess();
                var process = GetParentProcess(command?.Id ?? 0);
                RunType runType = DetermineRunType(command, process, defaultRunType);

                processInfo = new ProcessInfo
                {
                    RunType = runType,
                    CommandName = command?.ProcessName,
                    ProcessName = process?.ProcessName
                };
            }
            catch
            {
                if (defaultRunType != RunType.Gui)
                {
                    AttachConsole(-1);
                }
            }
            return processInfo;
        }

        /// <summary>
        /// Determines the run type based on the parent process and command process.
        /// </summary>
        /// <param name="command">The command process.</param>
        /// <param name="process">The parent process.</param>
        /// <param name="defaultRunType">The default run type.</param>
        /// <returns>The determined run type.</returns>
        private static RunType DetermineRunType(Process command, Process process, RunType defaultRunType)
        {
            if (process != null && process.ProcessName == ResourceStrings.CmdProcessName ||
                command?.ProcessName == ResourceStrings.CmdProcessName)
            {
                AttachConsole(process?.Id ?? -1);
                return RunType.Console;
            }
            if (process != null && (process.ProcessName.Contains(ResourceStrings.PowerShellProcessName) ||
                                    process.ProcessName.Contains(ResourceStrings.PwshProcessName)) ||
                (command != null && (command.ProcessName.Contains(ResourceStrings.PowerShellProcessName) ||
                                     command.ProcessName.Contains(ResourceStrings.PwshProcessName))))
            {
                AttachConsole(process?.Id ?? -1);
                return RunType.Powershell;
            }

            if (process != null && IsGuiProcess(process.ProcessName) || IsGuiProcess(command?.ProcessName))
            {
                return RunType.Gui;
            }

            if (process == null && command?.ProcessName == ResourceStrings.ServicesProcessName)
            {
                return RunType.Service;
            }

            if (defaultRunType == RunType.Console || defaultRunType == RunType.Powershell)
            {
                AllocConsole();
            }

            return defaultRunType;
        }

        /// <summary>
        /// Checks if a process name represents a GUI process.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <returns>true if the process is a GUI process; otherwise, false.</returns>
        private static bool IsGuiProcess(string processName)
        {
            return processName == ResourceStrings.ExplorerProcessName ||
                   processName == ResourceStrings.SvcHostProcessName ||
                   processName == ResourceStrings.UserInitProcessName ||
                   processName == ResourceStrings.DevEnvProcessName ||
                   processName == ResourceStrings.IisExpressProcessName ||
                   processName == ResourceStrings.MsVsMonProcessName ||
                   processName == ResourceStrings.VsIisLaucherProcessName ||
                   processName == ResourceStrings.W3wpProcessName;
        }
    }
}
