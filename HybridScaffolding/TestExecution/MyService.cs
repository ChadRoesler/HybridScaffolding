using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Management;
using System.IO;

public class MyService : ServiceBase
{
    protected override void OnStart(string[] args)
    {
        var processName = GetProcessName();
        var parentProcessName = GetParentProcessName();

        // Log or use the process information
        File.AppendAllText("C:\\temp\\log.txt", "==============in==========\n");
        File.AppendAllText("C:\\temp\\log.txt", processName + "\n");
        File.AppendAllText("C:\\temp\\log.txt", parentProcessName + "\n");
        File.AppendAllText("C:\\temp\\log.txt", "==============out==========\n");
    }

    private static string GetProcessName()
    {
        using (Process process = Process.GetCurrentProcess())
        {
            return process.ProcessName;
        }
    }

    private static string GetParentProcessName()
    {
        using (Process process = Process.GetCurrentProcess())
        {
            int parentId = 0;
            using (ManagementObject managementObject = new ManagementObject($"win32_process.handle='{process.Id}'"))
            {
                managementObject.Get();
                parentId = Convert.ToInt32(managementObject["ParentProcessId"]);
            }

            using (Process parentProcess = Process.GetProcessById(parentId))
            {
                return parentProcess.ProcessName;
            }
        }
    }
}
