using System;
using HybridScaffolding.Constants;
using HybridScaffolding.Enums;
using HybridScaffolding.Workers;

namespace HybridScaffolding
{
    /// <summary>
    /// Provides functionality to determine the execution type (Console, GUI, Service) of a scaffolded application and execute it accordingly.
    /// </summary>
    public static class HybridExecutor
    {
        /// <summary>
        /// Dispatches the execution of the scaffolded application based on the determined execution type.
        /// </summary>
        /// <param name="scaffold">The scaffold object representing the application to be executed.</param>
        /// <param name="arguments">The arguments passed to the application, if applicable.</param>
        /// <param name="type">The type of the object to be used in GUI or Service execution, if applicable.</param>
        /// <param name="defaultRunType">The default execution type (Console, GUI, Service) to use if no specific type is determined. Defaults to Console.</param>
        /// <exception cref="ArgumentNullException">Thrown when the scaffold parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when both arguments and type are null, indicating an invalid operation.</exception>
        public static void DispatchExecutor(HybridScaffold scaffold, string[] arguments, Type type, RunType defaultRunType = RunType.Console)
        {
            if (scaffold == null) throw new ArgumentNullException(nameof(scaffold));

            var processInfo = ParentProcess.ConsoleScaffolding(defaultRunType);
            scaffold.RunType = processInfo.RunType;
            scaffold.ProcessName = processInfo.ProcessName;
            scaffold.CommandName = processInfo.CommandName;

            if (arguments == null && type == null && scaffold.RunType != RunType.Service)
            {
                throw new InvalidOperationException(ErrorStrings.InvalidOpperationError);
            }

            switch (scaffold.RunType)
            {
                case RunType.Console:
                case RunType.Powershell:
                    ExecuteConsole(scaffold, arguments);
                    break;
                case RunType.Gui:
                    ExecuteGui(scaffold, arguments, type);
                    break;
                case RunType.Service:
                    ExecuteService(scaffold, arguments);
                    break;
            }
        }

        /// <summary>
        /// Executes the scaffolded application in console mode.
        /// </summary>
        /// <param name="scaffold">The scaffold object.</param>
        /// <param name="arguments">The arguments for the console execution.</param>
        private static void ExecuteConsole(HybridScaffold scaffold, string[] arguments)
        {
            var outputArguments = scaffold.PreConsoleExec(arguments, scaffold.RunType);
            scaffold.ConsoleMain(outputArguments, scaffold.RunType);
        }

        /// <summary>
        /// Executes the scaffolded application in GUI mode.
        /// </summary>
        /// <param name="scaffold">The scaffold object.</param>
        /// <param name="arguments">The arguments for the GUI execution.</param>
        /// <param name="type">The type of the GUI object.</param>
        /// <exception cref="ArgumentNullException">Thrown when the type parameter is null.</exception>
        private static void ExecuteGui(HybridScaffold scaffold, string[] arguments, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var outputObject = scaffold.PreGuiExec(arguments, Activator.CreateInstance(type));
            scaffold.GuiMain(arguments, outputObject);
        }

        /// <summary>
        /// Executes the scaffolded application in service mode.
        /// </summary>
        /// <param name="scaffold">The scaffold object.</param>
        /// <param name="arguments">The arguments for the service execution.</param>
        private static void ExecuteService(HybridScaffold scaffold, string[] arguments)
        {
            var outputArguments = scaffold.PreServiceExec(arguments);
            scaffold.ServiceMain(outputArguments);
        }
    }
}
