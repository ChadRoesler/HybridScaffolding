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

        public virtual int? PreGuiExec()
        {
            return null;
        }

        public virtual int? ConsoleMain(string[] arguments)
        {
            return null;
        }
    }
}
