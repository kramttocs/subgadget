using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace SubGadgetDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {                
                try
                {
                    string downloadURL = args[0];
                    string username = args[1];
                    string password = args[2];
                    string fileName = args[3];
                    string location = args[4];
                    Console.WriteLine("SubGadget Downloader");
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
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("!!Error Downloading!!");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Reason: " + e.ToString());
                    Console.WriteLine("Press Enter to Exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
