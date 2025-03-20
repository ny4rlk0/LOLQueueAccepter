using System.Threading;

namespace LOLQueueAccepter
{
    class TaskHandler
    {
        public static void runTasks()
        {
            Thread a = new Thread(() => Main.lolStatus());
            a.Start();
            Thread b = new Thread(() => QueueAccept.accept());
            b.Start();
        }
    }
}
