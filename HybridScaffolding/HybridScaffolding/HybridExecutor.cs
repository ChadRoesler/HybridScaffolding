using System;
using System.Threading;
using System.Windows.Forms;

namespace HybridScaffolding
{
    public static class HybridExecutor
    {
        
        public static void DispatchExecutor(HybridScaffold scaffold, string[] arguments, Type formToRun)
        {
            scaffold.RunType = ParentProcess.ConsoleScaffolding();
            if (arguments != null && arguments.Length > 0)
            {
                scaffold.PreConsoleExec(arguments);
                scaffold.ConsoleMain(arguments);
            }
            else if (formToRun != null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                scaffold.PreGuiExec();
                Application.Run((Form)Activator.CreateInstance(formToRun));
            }
            else
            {
                throw new InvalidOperationException("Unable to locate arguments to execute against or a form to execute.\r\nPlease check the variables passed.");
            }
        }

    }
}
