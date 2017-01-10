using System;
using System.Windows.Forms;

namespace HybridScaffolding
{
    public static class HybridExecutor
    {

        /// <summary>
        /// Executes the built scaffold.
        /// </summary>
        /// <param name="scaffold"></param>
        /// <param name="arguments"></param>
        /// <param name="formToRun"></param>
        public static void DispatchExecutor(HybridScaffold scaffold, string[] arguments, Type formToRun)
        {
            scaffold.RunType = ParentProcess.ConsoleScaffolding();
            if (arguments != null && arguments.Length > 0)
            {
                var outputArguments = scaffold.PreConsoleExec(arguments);
                scaffold.ConsoleMain(outputArguments);
            }
            else if (formToRun != null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var outputForm = scaffold.PreGuiExec((Form)Activator.CreateInstance(formToRun));
                Application.Run(outputForm);
            }
            else
            {
                throw new InvalidOperationException("Unable to locate arguments to execute against or a form to display.\r\nPlease check the variables passed.");
            }
        }

    }
}
