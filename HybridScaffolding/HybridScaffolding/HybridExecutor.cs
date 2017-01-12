using System;
using System.Windows.Forms;

namespace HybridScaffolding
{
    /// <summary>
    /// The executor of the scaffolding built.
    /// </summary>
    public static class HybridExecutor
    {

        /// <summary>
        /// Determines the run type and executes the built scaffold appropriately.
        /// </summary>
        /// <param name="scaffold">The scaffold built.</param>
        /// <param name="arguments">The console arguments.</param>
        /// <param name="mainFormType">The form to display.</param>
        public static void DispatchExecutor(HybridScaffold scaffold, string[] arguments, Type mainFormType)
        {
            scaffold.RunType = ParentProcess.ConsoleScaffolding();
            if (arguments != null && mainFormType != null && arguments.Length > 0)
            {
                throw new InvalidOperationException("Unable to locate arguments to execute against or a form to display.\r\nPlease check the variables passed.");
            }
            else
            {
                switch (scaffold.RunType)
                {
                    case RunTypes.Console:
                        {
                            var outputArguments = scaffold.PreConsoleExec(arguments);
                            scaffold.ConsoleMain(outputArguments);
                        }
                        break;
                    case RunTypes.Gui:
                        {
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            var outputForm = scaffold.PreGuiExec((Form)Activator.CreateInstance(mainFormType));
                            Application.Run(outputForm);
                        }
                        break;
                }
            }
        }
    }
}
