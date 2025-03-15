using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Forms;
using System.IO;

namespace LOLQueueAccepter
{
    class QueueAccept
    {
        public static void accept()
        {
            while (true)
            {
                string[] gameSession = Main.cliRequest("GET", "lol-gameflow/v1/session");
                if (gameSession[0]=="200")
                {
                    //Method 1
                    //string[] ph = Regex.Split(gameSession[1], "phase");
                    //string phase = ph.Last().Split('"')[2];
                    //Method 2
                    string phase = gameSession[1].Split(new [] { "phase" }, StringSplitOptions.None).Last().Split('"')[2];
                    switch (phase)
                    {
                        case "Lobby":
                            Thread.Sleep(2000);
                            break;
                        case "Matchmaking":
                            Thread.Sleep(2000);
                            break;
                        case "ReadyCheck":
                            acceptMatchMaking();
                            Thread.Sleep(2000);
                            break;
                        case "ChampSelect":
                            Thread.Sleep(5000);
                            break;
                        case "InProgress":
                            Thread.Sleep(9000);
                            break;
                        case "WaitingForStats":
                            Thread.Sleep(9000);
                            break;
                        case "PreEndOfGame":
                            Thread.Sleep(9000);
                            break;
                        case "EndOfGame":
                            Thread.Sleep(5000);
                            break;
                        default:
                            Thread.Sleep(2000);
                            //File.WriteAllText(Application.StartupPath.ToString() + "\\UnknownPhases_SendToDeveloper.txt", phase.ToString());
                            break;
                    }
                }
                Thread.Sleep(500);
            }
        }
        private static void acceptMatchMaking()
        {
            Main.cliRequest("POST","lol-matchmaking/v1/ready-check/accept");
        }
    }
}
