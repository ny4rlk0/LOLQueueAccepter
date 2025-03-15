using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;

namespace LOLQueueAccepter
{
    class TaskHandler
    {
        public static void runTasks()
        {
            //var taskStatus = new Task(Main.lolStatus);
            //taskStatus.Start();
            //var taskAccept = new Task(QueueAccept.accept);
            //taskAccept.Start();

            //var tasks = new[] { taskStatus,taskAccept };
            //Task.WaitAll(tasks);

            Thread a = new Thread(() => Main.lolStatus());
            a.Start();
            Thread b = new Thread(() => QueueAccept.accept());
            b.Start();
        }

    }
}
