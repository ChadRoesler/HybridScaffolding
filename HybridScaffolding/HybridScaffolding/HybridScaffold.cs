using System.Windows.Forms;

namespace HybridScaffolding
{
    public abstract class HybridScaffold
    {
        /// <summary>
        /// Instantiate the Scaffold
        /// </summary>
        public HybridScaffold()
        {
        }

        /// <summary>
        /// Used for Returning the Run type in for any specific error handling.
        /// </summary>
        public RunTypes RunType { get; set; }

        /// <summary>
        /// Runs pre Console Execution, also allows for manipulation of the arguments as needed.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public virtual string[] PreConsoleExec(string[] arguments)
        {
            return arguments;
        }

        /// <summary>
        /// Runs pre Gui Execution, also allows for manipuation of the forms as needed.
        /// </summary>
        /// <param name="formToUse"></param>
        /// <returns></returns>
        public virtual Form PreGuiExec(Form formToUse)
        {
            return formToUse;
        }

        /// <summary>
        /// This is what the Console will execute.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public virtual void ConsoleMain(string[] arguments)
        {
        }
    }
}
