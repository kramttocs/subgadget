using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;

namespace SubGadgetDownloaderGUI
{
    public partial class downloaderForm : Form
    {
        public downloaderForm()
        {
            InitializeComponent();
        }        

        private void downloaderForm_Shown(object sender, EventArgs e)
        {
            double currentVersion = 1.3D;
            string username;
            string password;
            string location;
            string filePath;
            string[] args = Environment.GetCommandLineArgs();
            username = args[1];
            password = args[2];
            filePath = args[3];
            location = args[4];
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList nodeList = doc.SelectNodes("SubGadgetDownloader/tracks/track/id");
            int trackCount = 0;
            string[] queue = new string[nodeList.Count];
            foreach (XmlNode track in nodeList)
            {
                queue[trackCount] = track.InnerText.ToString();
                trackCount++;
            }
            label1.Text = queue.Length.ToString();
            File.Delete(filePath);
            foreach (string downloadURL in queue)
            {
                downloadTrack(downloadURL, username, password, location, progressBarTest);
            }
        }

        public void downloadTrack(string downloadURL, string username, string password, string location, ProgressBar trackProgressBar)
        {
            //try
            //{
                string fileName;
                System.IO.FileStream FileStreamer;
                byte[] bBuffer = new byte[1024];
                int iBytesRead = 0;
                
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(downloadURL);
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
                    fileName = contentDispositionHeader.Substring(indexOfFileName, (contentDispositionHeader.Length - 1) - indexOfFileName);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        trackProgressBar.Maximum = Convert.ToInt32(response.ContentLength);
                        trackProgressBar.Value = 0;
                        FileStreamer = new FileStream(location + "\\" + fileName, System.IO.FileMode.Create);
                        do
                        {
                            iBytesRead = responseStream.Read(bBuffer, 0, 1024);
                            FileStreamer.Write(bBuffer, 0, iBytesRead);
                            if (trackProgressBar.Value + iBytesRead <= trackProgressBar.Maximum)
                            {
                                trackProgressBar.Value += iBytesRead;
                                Application.DoEvents();
                            }
                            else
                            {
                                trackProgressBar.Value = trackProgressBar.Maximum;
                            }
                        }
                        while (iBytesRead != 0);
                    }
                }
            /* }
             catch (Exception e)
             {
                 
             }*/
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

        private void downloaderForm_Load(object sender, EventArgs e)
        {

        }

        
        
    }
}
