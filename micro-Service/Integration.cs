using MQCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace micro_Service {
  internal class Integration {
    MQCore.Core oMQgateway = new MQCore.Core();
    List<string> sQueues = new List<string>() { "Verify-Name" };
    string sHostName = "localhost";
    internal bool bRun = true;


    internal void ManageQueues() {
      Console.WriteLine("Service Started");
      Console.WriteLine("Type /help for list of commands.");

      Task tCListener = Task.Factory.StartNew(() => ConsoleListener(), new System.Threading.CancellationToken(!bRun));
      Task tListener = null;
      while (bRun) {

        foreach (string sQueue in sQueues) {
          if (oMQgateway.lQueues.Find(x => x.Contains(sQueue)) != sQueue) {
            oMQgateway.CreateQueue(sQueue, sHostName);
            Console.WriteLine(oMQgateway.sMessage);
            tListener = Task.Factory.StartNew(() => CreateListener(sQueue, sHostName), new System.Threading.CancellationToken(!bRun));
          }
        }
        System.Threading.Thread.Sleep(5000);
      }
      Console.WriteLine("Stopping all services");
      tListener.Wait();
      Console.WriteLine("Services Stopped");
    }

    internal void CreateListener(string sQueue, string sHostname) {
      while (bRun) {
        string sMessage = oMQgateway.GetMessage(sQueue, ref bRun, sHostname);
        if (sMessage.Contains("Hello my name is, ")) {
          Console.WriteLine(String.Format("Hello {0}, I am your father!", sMessage.Substring(sMessage.IndexOf(",") + 1).Trim()));
        }
      }
    }

    internal void ConsoleListener() {
      while (bRun) {
        string scommand = Console.ReadLine();
        switch (scommand.ToUpper()) {
          case "/HELP": {
              Console.WriteLine("********Available Commands********");
              Console.WriteLine("Exit - Stops all services and exit the console application");
              break;
            }
          case "/EXIT": {
              bRun = false;
              break;
            }
        }
      }
    }
  }
}
