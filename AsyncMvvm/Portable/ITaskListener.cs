namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Interface for task listeners.
    /// </summary>
    public interface ITaskListener
    {
        /// <summary>
        /// Notifies the listener on task start.
        /// </summary>
        void NotifyTaskStarting();

        /// <summary>
        /// Notifies the listener on task completion.
        /// </summary>
        /// <param name="isSuccess"><value>true</value> if successful, <value>false</value> if faulted,
        /// or <value>null</value> if canceled.</param>
        void NotifyTaskCompleted(bool? isSuccess);
    }
}
