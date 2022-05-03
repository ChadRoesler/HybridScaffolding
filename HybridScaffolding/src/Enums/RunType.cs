namespace HybridScaffolding.Enums
{
    /// <summary>
    /// The determined RunTypes
    /// </summary>
    public enum RunType
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
