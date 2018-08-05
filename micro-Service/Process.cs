using MQCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace micro_Service {
  internal class Process {
    MQCore.Core oMQgateway = new MQCore.Core();
    string sQueue = "Verify-Name";
    string sHostName = "localhost";
    internal bool bRun = true;


    internal void ManageQueues() {
      while (bRun) {

        if (oMQgateway.lQueues.Find(x => x.Contains(sQueue)) != sQueue) {
          oMQgateway.CreateQueue(sQueue, sHostName);
          Console.WriteLine(oMQgateway.sMessage);
          Task tListener = Task.Factory.StartNew(() => CreateListener(sQueue, sHostName), new System.Threading.CancellationToken(!bRun));
        }

        System.Threading.Thread.Sleep(5000);
      }
    }

    internal void CreateListener(string sQueue, string sHostname) {
      while (bRun) {
        string sMessage = oMQgateway.GetMessage(sQueue, ref bRun, sHostname);
        if (sMessage.Contains("Hello my name is, ")) {
          Console.WriteLine(String.Format("Hello {0}, I am your father!", sMessage.Substring(sMessage.IndexOf(",")+1).Trim()));
        }
      }      
    }
  }
}
