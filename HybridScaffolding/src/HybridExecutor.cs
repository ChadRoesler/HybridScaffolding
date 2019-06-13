using System;
using System.Windows.Forms;
using HybridScaffolding.Constants;

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
            var processInfo = ParentProcess.ConsoleScaffolding();
            scaffold.RunType = processInfo.RunType;
            scaffold.ProcessName = processInfo.ProcessName;
            scaffold.CommandName = processInfo.CommandName;

            if (arguments == null && mainFormType == null)
            {
                throw new InvalidOperationException(ErrorStrings.InvalidOpperationError);
            }
            else
            {
                switch (scaffold.RunType)
                {
                    case RunTypes.Console:
                    case RunTypes.Powershell:
                        {
                            var outputArguments = scaffold.PreConsoleExec(arguments, scaffold.RunType);
                            scaffold.ConsoleMain(outputArguments, scaffold.RunType);
                        }
                        break;
                    case RunTypes.Gui:
                        {
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            var outputForm = scaffold.PreGuiExec((Form)Activator.CreateInstance(mainFormType));
                            if (outputForm == null)
                            {
                                Application.Exit();
                            }
                            else
                            {
                                Application.Run(outputForm);
                            }
                        }
                        break;
                }
            }
        }
    }
}
