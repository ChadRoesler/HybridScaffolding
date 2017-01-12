namespace HybridScaffolding
{
    /// <summary>
    /// The determined RunTypes
    /// </summary>
    public enum RunTypes
    {
        /// <summary>
        /// Either run from the Command Prompt or PowerShell
        /// </summary>
        Console = 0,

        /// <summary>
        /// Either run by svchost or explorer.
        /// </summary>
        Gui = 1
    }
}
