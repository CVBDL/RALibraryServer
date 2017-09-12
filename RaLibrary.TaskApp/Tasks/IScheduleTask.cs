namespace RaLibrary.TaskApp
{
    public interface IScheduleTask
    {
        /// <summary>
        /// Do task for plan schedule.
        /// </summary>
        /// <returns>Indicate whether the task is completed successfully.</returns>
        bool DoTask();
    }
}
