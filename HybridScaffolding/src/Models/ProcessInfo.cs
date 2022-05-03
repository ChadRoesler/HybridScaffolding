using HybridScaffolding.Enums;

namespace HybridScaffolding.Models
{
    internal class ProcessInfo
    {
        internal ProcessInfo()
        {

        }
        internal ProcessInfo (RunType defaultRunType)
        {
            RunType = defaultRunType;
        }
        internal RunType RunType { get; set; }
        internal string ProcessName { get; set; }
        internal string CommandName { get; set; }
    }
}
