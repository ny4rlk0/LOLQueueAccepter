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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace LOLQueueAccepter
{
    public partial class Form1: Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Activated += AfterLoading;
        }
        private void AfterLoading(object sender, EventArgs e)
        {
            this.Activated -= AfterLoading;
            runAsAdministrator();
            //Write your code here.
            Thread a = new Thread(() => TaskHandler.runTasks());
            a.Start();

            notifyIcon1.Icon = Properties.Resources.red_heart_256x235;
            notifyIcon1.ContextMenu = new ContextMenu();
            MenuItem Exit = new MenuItem("Exit / Çıkış");
            notifyIcon1.ContextMenu.MenuItems.Add(Exit);
            Exit.Click += ExitClick;
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "LoL Auto Queue Accepter\ngithub.com/ny4rlk0";
            notifyIcon1.BalloonTipText = "LoL Auto Queue Accepter has been started!\ngithub.com/ny4rlk0";
            notifyIcon1.ShowBalloonTip(1000);
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            this.WindowState= FormWindowState.Minimized;
            this.Hide();
            #region No Multiple Instances
            System.Diagnostics.Process pThis = System.Diagnostics.Process.GetCurrentProcess();
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                if (p.Id != pThis.Id && p.ProcessName == pThis.ProcessName)
                    Environment.Exit(0);
            #endregion
        }
        private void ExitClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        #region runAsAdministrator()
        static private void runAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);
            if (!runAsAdmin)
            {
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
                try { Process.Start(processInfo); }
                catch (Exception) { MessageBox.Show("Run As Administrator!"); }
                System.Windows.Forms.Application.Exit();
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
