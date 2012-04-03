using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DirectoryListing;

namespace DirectoryListing {
    class Program {
        static void Main(string[] args) {
            //who the fuck knows what this shit is doing...
            List<string> lstProcessData = new List<string>();
            int nThreadCount=4;
            ManualResetEvent[] doneEvents;
            string dir = @"c:\music\";

            getFileListing(dir, ref lstProcessData);

            doneEvents = new ManualResetEvent[lstProcessData.Count()];
            int i = 0;
            foreach (string strDir in lstProcessData) {
                doneEvents[i] = new ManualResetEvent(false);
                WorkItem wkDir = new WorkItem(strDir, doneEvents[i]);
                ThreadPool.SetMaxThreads(nThreadCount,nThreadCount);
                ThreadPool.QueueUserWorkItem(wkDir.ThreadPoolCallback, i);
                i++;
            }

            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All files Processed");
        }

        public static void getFileListing(string baseDir,ref List<string> lstDir) {

            if (Directory.GetDirectories(baseDir).Count() != 0) {
                foreach (string dirName in Directory.GetDirectories(baseDir)) {
                    getFileListing(dirName, ref lstDir);
                    lstDir.Add(dirName);
                }
            }
            else {
                lstDir.Add(baseDir);
            }
                
        }
    }
}
