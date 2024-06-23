namespace HybridScaffolding.Enums
{
    /// <summary>
    /// Defines the various types of execution contexts for the application.
    /// </summary>
    public enum RunType
    {
        /// <summary>
        /// Indicates the application is running from the Command Prompt.
        /// </summary>
        Console = 0,

        /// <summary>
        /// Indicates the application is running from PowerShell.
        /// </summary>
        Powershell = 1,

        /// <summary>
        /// Indicates the application is running with a Graphical User Interface (GUI), typically initiated by the user through a graphical desktop environment.
        /// </summary>
        Gui = 2,

        /// <summary>
        /// Indicates the application is running as a service, typically managed by the operating system's service control manager.
        /// </summary>
        Service = 3
    }
}
