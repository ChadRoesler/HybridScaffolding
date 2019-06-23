using HybridScaffolding.Enums;

namespace HybridScaffolding
{
    /// <summary>
    /// The abstract class used for implmenting the overrides for pre and console execution.
    /// </summary>
    public abstract class HybridScaffold
    {
        /// <summary>
        /// Returns and Sets the determined runtype.
        /// </summary>
        public RunTypes RunType { get; internal set; }

        /// <summary>
        /// Returns and Sets the determined processname.
        /// </summary>
        public string ProcessName { get; internal set; }

        /// <summary>
        /// Returns and Sets the determined commandname.
        /// </summary>
        public string CommandName { get; internal set; }

        /// <summary>
        /// Runs pre Console Execution, also allows for manipulation of the arguments as needed.
        /// </summary>
        /// <param name="arguments">The console arguments.</param>
        /// <param name="runType">Powershell or cmd.</param>
        /// <returns>Returns the console arguments passed.</returns>
        public virtual string[] PreConsoleExec(string[] arguments, RunTypes runType)
        {
            return arguments;
        }

        /// <summary>
        /// Runs pre Gui Execution, also allows for manipuation of the forms as needed.
        /// </summary>
        /// /// <param name="arguments">The console arguments.</param>
        /// <param name="passableObject">The object to manage.</param>
        /// <returns>Returns the Form passed.</returns>
        public virtual object PreGuiExec(string[] arguments, object passableObject)
        {
            return passableObject;
        }

        /// <summary>
        /// This is what the Console will execute.
        /// </summary>
        /// <param name="arguments">The console arguments.</param>
        /// <param name="runType">Powershell or cmd.</param>
        /// <returns>Void</returns>
        public virtual void ConsoleMain(string[] arguments, RunTypes runType)
        {
        }

        /// <summary>
        /// This is what the GUI will execute.
        /// </summary>
        /// <param name="arguments">The console arguments.</param>
        /// <param name="passableObject">The object to manage.</param>
        /// <returns>Void</returns>
        public virtual void GuiMain(string[] arguments, object passableObject)
        {
        }
    }
}
