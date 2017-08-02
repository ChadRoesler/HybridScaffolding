namespace HybridScaffolding
{
    /// <summary>
    /// The determined RunTypes
    /// </summary>
    public enum RunTypes
    {
        /// <summary>
        /// Run from the Command Prompt
        /// </summary>
        Console = 0,

        /// <summary>
        /// Run from Powershell
        /// </summary>
        Powershell = 1,

        /// <summary>
        /// Either run by svchost or explorer.
        /// </summary>
        Gui = 2
    }
}
