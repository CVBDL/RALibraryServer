namespace RaLibrary.TaskApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var expiredbookTask = new ExpiredBookAlertTask();
            expiredbookTask.DoTask();
        }
    }
}
