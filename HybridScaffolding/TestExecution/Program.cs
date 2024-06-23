using System;
using System.Diagnostics;
using HybridScaffolding;
using System.Management;
using System.ServiceProcess;

namespace TestExecution
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            File.AppendAllText("C:\\temp\\log.txt", "==============pre==========\n");
            File.AppendAllText("C:\\temp\\log.txt",GetParentProcessName() + "\n");
            File.AppendAllText("C:\\temp\\log.txt",GetProcessName() + "\n");
            File.AppendAllText("C:\\temp\\log.txt", "==============pre mock==========\n");
            var mock = new MockScaffold();
            HybridExecutor.DispatchExecutor(mock, args, typeof(MockScaffold));
            ServiceBase.Run(new MyService());
            File.AppendAllText("C:\\temp\\log.txt", "==============pst mock==========\n");
            File.AppendAllText("C:\\temp\\log.txt", mock.ProcessName + ":" + mock.CommandName + "\n");
            
            File.AppendAllText("C:\\temp\\log.txt", "==============pst ee==========\n");
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
}
