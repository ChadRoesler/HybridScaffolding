using HybridScaffolding.Constants;
using HybridScaffolding.Enums;
using HybridScaffolding.Workers;
using System;

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
        /// <param name="defaultRunType">The default run type the application will fall back to</param>
        /// <param name="scaffold">The scaffold built.</param>
        /// <param name="arguments">The console arguments.</param>
        /// <param name="type">The object to pass.</param>
        public static void DispatchExecutor(HybridScaffold scaffold, string[] arguments, Type type, RunType defaultRunType = RunType.Console)
        {
            var processInfo = ParentProcess.ConsoleScaffolding(defaultRunType);
            scaffold.RunType = processInfo.RunType;
            scaffold.ProcessName = processInfo.ProcessName;
            scaffold.CommandName = processInfo.CommandName;

            if (arguments == null && type == null)
            {
                throw new InvalidOperationException(ErrorStrings.InvalidOpperationError);
            }
            else
            {
                switch (scaffold.RunType)
                {
                    case RunType.Console:
                    case RunType.Powershell:
                        {
                            var outputArguments = scaffold.PreConsoleExec(arguments, scaffold.RunType);
                            scaffold.ConsoleMain(outputArguments, scaffold.RunType);
                        }
                        break;
                    case RunType.Gui:
                        {
                            var outputObject = scaffold.PreGuiExec(arguments, (object)Activator.CreateInstance(type));
                            scaffold.GuiMain(arguments, outputObject);
                        }
                        break;
                }
            }
        }
    }
}