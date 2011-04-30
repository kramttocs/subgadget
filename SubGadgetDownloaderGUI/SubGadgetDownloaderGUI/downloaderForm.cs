﻿using System;
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
using System.Diagnostics;

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
            XmlNodeList nodeListName = doc.SelectNodes("SubGadgetDownloader/tracks/track/name");
            int trackCount = 0;
            string[] queue = new string[nodeList.Count];
            foreach (XmlNode track in nodeList)
            {
                queue[trackCount] = track.InnerText.ToString();
                ListViewItem trackItem = new ListViewItem(new []{nodeListName[trackCount].InnerText.ToString(),"Waiting..."});
                lstTracks.Items.Add(trackItem);
                trackCount++;
            }
            File.Delete(filePath);
            int count = 0;
            foreach (string downloadURL in queue)
            {
                count++;
                lblTrackCountStatus.Text = count + "/" + queue.Length.ToString();
                lstTracks.Items[count - 1].SubItems[1].Text = "Downloading...";
                downloadTrack(downloadURL, username, password, location, progressBarTest);
                lstTracks.Items[count - 1].SubItems[1].Text = "Complete";                
            }
            lblCurrentTrack.Text = "Downloading Complete. Total: "+ queue.Length.ToString() + " tracks.";
        }

        public void downloadTrack(string downloadURL, string username, string password, string location, ProgressBar trackProgressBar)
        {
            try
            {
                string fileName;
                byte[] bBuffer = new byte[4096];
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
                    var fileNameDisplay = fileName;
                    if (fileNameDisplay.Length > 40)
                    {
                        fileNameDisplay = fileNameDisplay.Substring(0, 37) + "...";
                    }
                    lblCurrentTrack.Text = "Downloading: "+fileNameDisplay;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        trackProgressBar.Maximum = Convert.ToInt32(response.ContentLength);
                        trackProgressBar.Value = 0;
                        using (FileStream FileStreamer = new FileStream(location + "\\" + fileName, System.IO.FileMode.Create))
                        {
                            do
                            {
                                iBytesRead = responseStream.Read(bBuffer, 0, 4096);
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
                }
             }
             catch (Exception ex)
             {
                 
             }
        }
        public static void checkVersion(double currentVersion, LinkLabel updateLabel, Label versionLabel)
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
                        while (reader.ReadToFollowing("currentDownloaderGUIVersion"))
                        {
                            double serverVersion = reader.ReadElementContentAsDouble();
                            if (serverVersion > currentVersion)
                            {
                                updateLabel.Visible = true;
                                versionLabel.Visible = false;
                            }
                            else
                            {
                                versionLabel.Text = "Up to date (v" + currentVersion.ToString() + ")";
                                versionLabel.Visible = true;
                                updateLabel.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                versionLabel.Text = "Version: "+currentVersion.ToString();
                versionLabel.Visible = true;
                updateLabel.Visible = false;
            }
        }

        private void downloaderForm_Load(object sender, EventArgs e)
        {
            double currentVersion = 1.0D;
            checkVersion(currentVersion, lnkUpdate, lblVersion);
        }

        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://code.google.com/p/subgadget/");
            }
            catch (Exception ex) { }            
        }

        private void lstTracks_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = lstTracks.Columns[e.ColumnIndex].Width;
        }

        
        
    }
}
