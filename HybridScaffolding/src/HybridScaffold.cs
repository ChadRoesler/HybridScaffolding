using HybridScaffolding.Enums;

namespace HybridScaffolding
{
    /// <summary>
    /// Represents the base class for a scaffolded application, providing hooks for pre-execution and main execution methods across different run types (Console, GUI, Service).
    /// </summary>
    public abstract class HybridScaffold
    {
        /// <summary>
        /// Gets or sets the run type of the scaffolded application.
        /// </summary>
        public RunType RunType { get; internal set; }

        /// <summary>
        /// Gets or sets the process name of the scaffolded application.
        /// </summary>
        public string ProcessName { get; internal set; }

        /// <summary>
        /// Gets or sets the command name for the scaffolded application.
        /// </summary>
        public string CommandName { get; internal set; }

        /// <summary>
        /// Provides a hook for pre-execution logic in console mode.
        /// </summary>
        /// <param name="arguments">The arguments passed to the console application.</param>
        /// <param name="runType">The run type of the application.</param>
        /// <returns>The potentially modified arguments for the console application.</returns>
        public virtual string[] PreConsoleExec(string[] arguments, RunType runType) => arguments;

        /// <summary>
        /// Provides a hook for pre-execution logic in GUI mode.
        /// </summary>
        /// <param name="arguments">The arguments passed to the GUI application.</param>
        /// <param name="passableObject">An object that can be passed to the GUI application for further use.</param>
        /// <returns>The potentially modified passable object for the GUI application.</returns>
        public virtual object PreGuiExec(string[] arguments, object passableObject) => passableObject;

        /// <summary>
        /// Provides a hook for pre-execution logic in service mode.
        /// </summary>
        /// <param name="arguments">The arguments passed to the service application.</param>
        /// <returns>The potentially modified arguments for the service application.</returns>
        public virtual string[] PreServiceExec(string[] arguments) => arguments;

        /// <summary>
        /// The main execution method for console mode.
        /// </summary>
        /// <param name="arguments">The arguments for the console application.</param>
        /// <param name="runType">The run type of the application.</param>
        public virtual void ConsoleMain(string[] arguments, RunType runType) { }

        /// <summary>
        /// The main execution method for GUI mode.
        /// </summary>
        /// <param name="arguments">The arguments for the GUI application.</param>
        /// <param name="passableObject">An object that can be passed to the GUI application for further use.</param>
        public virtual void GuiMain(string[] arguments, object passableObject) { }

        /// <summary>
        /// The main execution method for service mode.
        /// </summary>
        /// <param name="arguments">The arguments for the service application.</param>
        public virtual void ServiceMain(string[] arguments) { }
    }
}
