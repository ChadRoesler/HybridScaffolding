using System.Windows.Forms;

namespace HybridScaffolding
{
    public abstract class HybridScaffold
    {
        public HybridScaffold()
        {
        }

        public string RunType { get; set; }
        public virtual int? PreConsoleExec(string[] arguments)
        {
            return null;
        }

        public virtual Form PreGuiExec(Form formToUse)
        {
            return null;
        }

        public virtual int? ConsoleMain(string[] arguments)
        {
            return null;
        }
    }
}
