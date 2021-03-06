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
using System.Collections;

namespace SubGadgetDownloaderGUI
{
    public partial class downloaderForm : Form
    {
        public downloaderForm()
        {
            InitializeComponent();
        }

        bool cancelDownload;
        FileStream FileStreamer;
        int successfullyDownloaded = 0;
        string location;        

        private void downloaderForm_Load(object sender, EventArgs e)
        {
            //double currentVersion = 1.1D;
            //lblVersion.Text = "v" + currentVersion.ToString();
            //checkVersion(currentVersion, lnkUpdate, lblVersion);
        }

        private void downloaderForm_Shown(object sender, EventArgs e)
        {            
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 5)
            {
                try
                {
                    string username = args[1];
                    string password = args[2];
                    string filePath = args[3];
                    location = args[4];
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNodeList nodeList = doc.SelectNodes("SubGadgetDownloader/tracks/track/id");
                    XmlNodeList nodeListName = doc.SelectNodes("SubGadgetDownloader/tracks/track/name");
                    int trackCount = 0;
                    ArrayList queue = new ArrayList();
                    foreach (XmlNode track in nodeList)
                    {
                        queue.Add(track.InnerText.ToString());
                        ListViewItem trackItem = new ListViewItem(new[] {nodeListName[trackCount].InnerText.ToString(), "Waiting..." });
                        lstTracks.Items.Add(trackItem);
                        trackCount++;
                    }
                    File.Delete(filePath);
                    string locationTemp = location;
                    if (locationTemp.Length > 50)
                    {
                        locationTemp = locationTemp.Substring(0, 47) + "...";
                    }
                    lnkSaveTo.Text = locationTemp;
                    processQueue(queue, username, password);
                    lblCurrentTrack.Text = "Downloading Complete. Total: " + successfullyDownloaded + " out of "+ queue.Count.ToString() + " tracks.";
                    progressBarTest.Value = 0;
                }
                catch (Exception ex)
                {
                    Application.Exit();
                }
            }
            else
            {

                Application.Exit();
            }
        }

        public void processQueue(ArrayList theQueue, string uname, string pword)
        {
            int count = 0;
            foreach (string downloadURL in theQueue)
            {
                count++;
                lblTrackCountStatus.Text = count + "/" + theQueue.Count.ToString();
                //lstTracks.EnsureVisible(count - 1);
                if (lstTracks.Items[count - 1].SubItems[1].Text == "Waiting...")
                {
                    lstTracks.Items[count - 1].SubItems[1].Text = "Downloading...";
                    downloadTrack(downloadURL, uname, pword, location, progressBarTest, count);
                }
            }
            if (revisitQueue())
            {
                processQueue(theQueue, uname, pword);
            }
        }

        public bool revisitQueue()
        {
            bool goAgain = false;
            for (int i = 0; i < lstTracks.Items.Count; i++)
            {
                if (lstTracks.Items[i].Selected && (lstTracks.Items[i].SubItems[1].Text == "Waiting..."))
                {
                    goAgain = true;
                }               
            }
            return goAgain;
        }

        public void downloadTrack(string downloadURL, string username, string password, string location, ProgressBar trackProgressBar, int count)
        {
            try
            {
                cancelDownload = false;
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
                        using (FileStreamer = new FileStream(location + "\\" + fileName, System.IO.FileMode.Create))
                        {
                            do
                            {
                                iBytesRead = responseStream.Read(bBuffer, 0, 4096);
                                FileStreamer.Write(bBuffer, 0, iBytesRead);
                                if (trackProgressBar.Value + iBytesRead <= trackProgressBar.Maximum)
                                {
                                    trackProgressBar.Value += iBytesRead;
                                    //Application.DoEvents() is a poor choice here but it's a quick solution
                                    Application.DoEvents();
                                    if (cancelDownload)
                                    {                                        
                                        FileStreamer.Close();
                                        File.Delete(FileStreamer.Name);
                                        break;
                                    }
                                }
                                else
                                {
                                    trackProgressBar.Value = trackProgressBar.Maximum;                                    
                                }
                            }
                            while (iBytesRead != 0);
                            if (cancelDownload)
                            {
                                lstTracks.Items[count - 1].SubItems[1].Text = "Cancelled";
                            }
                            else
                            {
                                lstTracks.Items[count - 1].SubItems[1].Text = "Complete";
                                successfullyDownloaded++;
                            }
                        }
                    }
                }
             }
             catch (Exception ex)
             {
                 //MessageBox.Show(ex.ToString());
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

        private void btnCancelDownload_Click(object sender, EventArgs e)
        {
            cancelDownload = true;
        }

        private void downloaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelDownload = true;
        }

        private void lnkSaveTo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = location;
                prc.Start();
            }
            catch (Exception ex) { }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {            
            for (int i = 0; i < lstTracks.Items.Count; i++)
            {
                if (lstTracks.Items[i].Selected && (lstTracks.Items[i].SubItems[1].Text == "Waiting..."))
                {                    
                    lstTracks.Items[i].SubItems[1].Text = "Removed";
                }
                else if (lstTracks.Items[i].Selected && (lstTracks.Items[i].SubItems[1].Text != "Downloading...") && (lstTracks.Items[i].SubItems[1].Text != "Complete"))
                {                    
                    lstTracks.Items[i].SubItems[1].Text = "Waiting...";
                }
            }
        }

        
        
    }
}
