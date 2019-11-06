using System;
using HybridScaffolding.Constants;
using HybridScaffolding.Enums;
using HybridScaffolding.Workers;

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
        /// <param name="type">The object to pass.</param>
        /// <param name="defaultBehavior">The default RunType to use if unable to determine.</param>
        public static void DispatchExecutor(HybridScaffold scaffold, string[] arguments, Type type, RunTypes defaultBehavior = RunTypes.Console)
        {
            var processInfo = ParentProcess.ConsoleScaffolding(defaultBehavior);
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
                    case RunTypes.Console:
                    case RunTypes.Powershell:
                        {
                            var outputArguments = scaffold.PreConsoleExec(arguments, scaffold.RunType);
                            scaffold.ConsoleMain(outputArguments, scaffold.RunType);
                        }
                        break;
                    case RunTypes.Gui:
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