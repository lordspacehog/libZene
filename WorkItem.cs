using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DirectoryListing {
    class WorkItem {
        //Private variables
        private string _strWork;
        private List<string> _lstFiles;
        private ManualResetEvent _doneEvent;

        //Public interfaces
        public string StrWork { get { return _strWork; } }
        public List<string> FileList { get { return _lstFiles; } }

        public WorkItem(string strWork, ManualResetEvent doneEvent) {
            _strWork = strWork;
            _doneEvent = doneEvent;

            //initialize File list
            _lstFiles = new List<string>();
        }

        public void ThreadPoolCallback(Object threadContext) {
            int threadIndex = (int)threadContext;
            Console.WriteLine("thread {0} started...", threadIndex);
            
            _lstFiles = ListFiles(_strWork);

            if (_lstFiles != null) {
                foreach (string strFile in _lstFiles) {
                    Console.WriteLine(strFile);
                }
            }
            else {
                Console.WriteLine("no files in dir");
            }
            Console.WriteLine("thread {0} finished...", threadIndex);
            _doneEvent.Set();
        }

        public List<string> ListFiles(string strProcessDir) {
            List<string> lstWorking = new List<string>();

            if (Directory.GetFiles(strProcessDir, "*.*").Count() != 0) {

                foreach (string strFileName in Directory.GetFiles(strProcessDir, "*.*")) {
                    lstWorking.Add(strFileName);
                }
                return lstWorking;
            }
            else {
                return null;
            }
        }
    }
}
