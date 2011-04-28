using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;

namespace SubGadgetDownloader
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                bool hidden = false;
                try
                {                   
                    double currentVersion = 1.3D;
                    string downloadURL = args[0];
                    string username = args[1];
                    string password = args[2];
                    string fileName = args[3];
                    string location = args[4];
                    hidden = Convert.ToBoolean(args[5]);
                    Console.Title = "SubGadget Downloader - "+fileName;
                    if (hidden)
                    {
                        Console.WriteLine("Preparing to hide...");
                        Thread.Sleep(1000);
                        IntPtr theWindow = FindWindow(null, Console.Title);
                        if (theWindow != IntPtr.Zero)
                        {
                            ShowWindow(theWindow, 0);
                        }
                    }
                    Console.WriteLine("SubGadget Downloader");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Version: "+currentVersion.ToString());
                    Console.WriteLine(checkVersion(currentVersion));                    
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Downloading track: "+ fileName);                    
                    byte[] buffer = new byte[4096];
                    HttpWebRequest wr = (HttpWebRequest) WebRequest.Create(downloadURL);
                    //set the timeout to 30 minutes
                    wr.Timeout = 1800000;
                    string usernamePassword = username + ":" + password;
                    CredentialCache mycache = new CredentialCache();
                    mycache.Add(new Uri(downloadURL), "Basic", new NetworkCredential(username, password));
                    wr.Credentials = mycache;
                    wr.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));
                    using (WebResponse response = wr.GetResponse())
                    {
                        //change the fileName to what is shown in the Content-Disposition header
                        string contentDispositionHeader = response.Headers["Content-Disposition"];
                        int indexOfFileName = contentDispositionHeader.IndexOf("filename") + 10;
                        fileName = contentDispositionHeader.Substring(indexOfFileName, (contentDispositionHeader.Length-1) - indexOfFileName);
                        Console.WriteLine("Saving as: " + location+"\\"+fileName);
                        //get the length
                        double contentLengthHeader = Convert.ToDouble(response.Headers["Content-Length"]);
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Completed:");
                        //get and save the response
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                int count = 0;
                                do
                                {
                                    count = responseStream.Read(buffer, 0, buffer.Length);
                                    memoryStream.Write(buffer, 0, count);
                                    Console.Write("\r{0:0.0%}",(memoryStream.Length) / (contentLengthHeader));
                                } while (count != 0);
                                FileStream outStream = File.OpenWrite(location + "\\" + fileName);
                                memoryStream.WriteTo(outStream);
                                outStream.Flush();
                                outStream.Close();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!hidden)
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("!!Error Downloading!!");
                        Console.WriteLine("--------------------------");
                        //
                        //Console.WriteLine("Reason: " + e.ToString());
                        //
                        Console.WriteLine("Please update if a new version is available");
                        Console.WriteLine("Press Enter to Exit");
                        Console.ReadLine();
                    }
                }
            }
        }
        public static string checkVersion(double currentVersion)
        {
            try
            {
                string xmlURL = "http://subgadget.googlecode.com/svn/trunk/versions.xml"; 
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(xmlURL);
                wr.Timeout = 6000;
                string status = "";
                using (WebResponse response = wr.GetResponse())
                {
                   using (XmlReader reader = XmlReader.Create(response.GetResponseStream()))
                    {
                        while (reader.ReadToFollowing("currentDownloaderVersion"))
                        {
                            double serverVersion = reader.ReadElementContentAsDouble();                                                                               
                            if (serverVersion > currentVersion)
                            {
                                status = "+++" + serverVersion.ToString() + " update available! Please visit http://code.google.com/p/subgadget/ +++";
                            }
                            else
                            {
                                status = "Up to date";
                            }
                        }
                    }
                }
                return status;
            }
            catch (Exception e)
            {
                return "Unable to check for update";
            }
        }
    }
}
