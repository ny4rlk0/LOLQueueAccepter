using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Text;
using System.Management;
using System.IO;
using System.Runtime.CompilerServices;

namespace LOLQueueAccepter
{
    class Main
    {
        private static string[] lolAuth;
        private static int pid = 0;
        public static bool isLolRunning=false;
        static Form1 form1;


        public static void lolStatus()
        {
            while (true)
            {
                Process cli = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
                if (cli!=null)
                {
                    lolAuth = getAuth(cli);
                    isLolRunning = true;
                    if (pid!=cli.Id) pid = cli.Id;
                }
                else
                {
                    isLolRunning = false;
                    Thread.Sleep(1000);
                }
            }
        }
        #region getAuth
        private static string[] getAuth(Process cli)
        {
            Process lol = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
            string lolPath = lol.MainModule.FileName;
            lolPath = lolPath.Replace("LeagueClientUx.exe", "lockfile");
            string secret;
            using (FileStream fileStream = new FileStream(
                lolPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                    secret = streamReader.ReadToEnd();
            }
            string[] info = secret.Split(':');
            string port = info[2];
            string authToken = info[3];
            string auth = "riot:" + authToken;
            string authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
            return new string[] { authBase64, port };
        }
        #endregion
        #region isLoLOpen
        public static bool isLoLOpen() {
            Process cli = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
            if (cli != null) return true;
            else return false;
        }
        #endregion
        #region cliRequest
        public static string[] cliRequest(string method, string url, string body = null)
        {
            var handler = new HttpClientHandler() {ServerCertificateCustomValidationCallback=HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
            try
            {
                if (isLolRunning)
                {
                    using (HttpClient cli2 = new HttpClient(handler))
                    {
                        cli2.BaseAddress = new Uri("https://127.0.0.1:" + lolAuth[1] + "/");
                        cli2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", lolAuth[0]);
                        HttpRequestMessage req = new HttpRequestMessage(new HttpMethod(method), url);
                        if (!string.IsNullOrEmpty(body)) req.Content = new StringContent(body, Encoding.UTF8, "application/json");
                        HttpResponseMessage resp = cli2.SendAsync(req).Result;
                        if (resp == null) return new string[] { "999", "" };
                        int httpStatus = (int)resp.StatusCode;
                        string httpStatusStr = httpStatus.ToString();
                        string receivedResponse = resp.Content.ReadAsStringAsync().Result;
                        resp.Dispose();
                        return new string[] { httpStatusStr, receivedResponse };
                    }
                }
                else return new string[] { "999", "" };

            }
            catch { return new string[] { "999", "" }; }
        }
        #endregion
        #region cliRequestUntilSucc
        public static string[] cliRequestUntilSucc(string method, string url, string body = null)
        {
            string[] req = { "999", "" };
            while (req[0].Substring(0,1)!="2")
            {
                req = cliRequest(method, url, body);
                if (req[0].Substring(0, 1) == "2") return req;
                else
                {
                    if (isLoLOpen()) Thread.Sleep(1000);
                    else return req;
                }
            }
            return req;
        }
        #endregion
    }
}
