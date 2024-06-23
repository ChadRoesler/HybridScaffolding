using HybridScaffolding.Enums;

namespace HybridScaffolding.Models
{
    /// <summary>
    /// Represents information about a process, including its run type, process name, and command name.
    /// </summary>
    internal class ProcessInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInfo"/> class.
        /// </summary>
        internal ProcessInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInfo"/> class with a specified default run type.
        /// </summary>
        /// <param name="defaultRunType">The default run type of the process.</param>
        internal ProcessInfo(RunType defaultRunType)
        {
            RunType = defaultRunType;
        }

        /// <summary>
        /// Gets or sets the run type of the process.
        /// </summary>
        internal RunType RunType { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        internal string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the command name associated with the process.
        /// </summary>
        internal string CommandName { get; set; }
    }
}
