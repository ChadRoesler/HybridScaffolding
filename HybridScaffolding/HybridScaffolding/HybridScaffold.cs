using System.Windows.Forms;

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
        /// Runs pre Console Execution, also allows for manipulation of the arguments as needed.
        /// </summary>
        /// <param name="arguments">The console arguments.</param>
        /// <returns>Returns the console arguments passed.</returns>
        public virtual string[] PreConsoleExec(string[] arguments, RunTypes runType)
        {
            return arguments;
        }

        /// <summary>
        /// Runs pre Gui Execution, also allows for manipuation of the forms as needed.
        /// </summary>
        /// <param name="mainForm">The form to display.</param>
        /// <returns>Returns the Form passed.</returns>
        public virtual Form PreGuiExec(Form mainForm)
        {
            return mainForm;
        }

        /// <summary>
        /// This is what the Console will execute.
        /// </summary>
        /// <param name="arguments">The console arguments.</param>
        /// <returns>Void</returns>
        public virtual void ConsoleMain(string[] arguments, RunTypes runType)
        {
        }
    }
}
