﻿using HybridScaffolding.Constants;
using HybridScaffolding.Enums;
using HybridScaffolding.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HybridScaffolding.Workers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ParentProcess
    {
        // These members must match PROCESS_BASIC_INFORMATION
        private readonly IntPtr Reserved1;
        private readonly IntPtr PebBaseAddress;
        private readonly IntPtr Reserved2_0;
        private readonly IntPtr Reserved2_1;
        private readonly IntPtr UniqueProcessId;
        private readonly IntPtr InheritedFromUniqueProcessId;

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
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out _);
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
        internal static ProcessInfo ConsoleScaffolding(RunType defaultRunType)
        {
            var processInfo = new ProcessInfo(defaultRunType);
            try
            {
                var command = GetParentProcess();
                var process = GetParentProcess(command.Id);
                RunType runType;
                if (process != null && process.ProcessName == ResourceStrings.CmdProcessName)
                {
                    AttachConsole(process.Id);
                    runType = RunType.Console;
                }
                else if (command.ProcessName == ResourceStrings.CmdProcessName)
                {
                    AttachConsole(-1);
                    runType = RunType.Console;
                }
                else if (process != null && (process.ProcessName.Contains(ResourceStrings.PowerShellProcessName) || process.ProcessName.Contains(ResourceStrings.PwshProcessName)))
                {
                    AttachConsole(process.Id);
                    runType = RunType.Powershell;
                }
                else if (command.ProcessName.Contains(ResourceStrings.PowerShellProcessName) || command.ProcessName.Contains(ResourceStrings.PwshProcessName))
                {
                    AttachConsole(-1);
                    runType = RunType.Powershell;
                }
                else if (process != null && process.ProcessName.Contains(ResourceStrings.PwshProcessName))
                {
                    AttachConsole(process.Id);
                    runType = RunType.Powershell;
                }
                else if (command.ProcessName.Contains(ResourceStrings.PwshProcessName))
                {
                    AttachConsole(process.Id);
                    runType = RunType.Powershell;
                }
                else if (process != null && (process.ProcessName == ResourceStrings.ExplorerProcessName || process.ProcessName == ResourceStrings.SvcHostProcessName || process.ProcessName == ResourceStrings.UserInitProcessName || process.ProcessName == ResourceStrings.DevEnvProcessName || process.ProcessName == ResourceStrings.IisExpressProcessName))
                {
                    runType = RunType.Gui;
                }
                else if (command.ProcessName == ResourceStrings.ExplorerProcessName || command.ProcessName == ResourceStrings.SvcHostProcessName || command.ProcessName == ResourceStrings.UserInitProcessName || command.ProcessName == ResourceStrings.MsVsMonProcessName || command.ProcessName == ResourceStrings.VsIisLaucherProcessName || command.ProcessName == ResourceStrings.W3wpProcessName || command.ProcessName == ResourceStrings.DevEnvProcessName)
                {
                    runType = RunType.Gui;
                }
                else
                {
                    if (defaultRunType == RunType.Console || defaultRunType == RunType.Powershell)
                    {
                        AllocConsole();
                    }
                    runType = defaultRunType;
                }
                processInfo = new ProcessInfo
                {
                    RunType = runType,
                    CommandName = command.ProcessName,
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
    }
}
