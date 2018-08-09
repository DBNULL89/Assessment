using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client {
  internal class Integration {
    MQCore.Core oMQgateway = new MQCore.Core();
    string sMessage;
    string sQueue = "Verify-Name";
    string sHost = "localhost";
    bool bRun = true;

    internal void ManageConsole() {
      Task tCListener = Task.Factory.StartNew(() => ConsoleListener(), new System.Threading.CancellationToken(!bRun));

      tCListener.Wait();
      Console.WriteLine(sMessage);
    }

    internal void ConsoleListener() {
      Console.WriteLine("Client Started.");
      Console.WriteLine("Type /help for list of commands.");
      Console.WriteLine("Please enter name");

      while (bRun) {
        string scommand = Console.ReadLine();
        switch (scommand.ToUpper()) {
          case "/HELP": {
              Console.WriteLine("********Available Commands********");
              Console.WriteLine("/Exit - Exit the console application");
              Console.WriteLine("/UnitTest - Runs batch of 6 test cases in quick succession.");
              break;
            }
          case "/EXIT": {
              bRun = false;
              break;
            }
          case "/UNITTEST": {
              unitTest();
              break;
            }
          default : {
              //oMQgateway.SendMessage(scommand, sQueue, sHost);
              Console.WriteLine(oMQgateway.SendMessage("Hello my name is, " + scommand, sQueue, sHost));
              break;
            }
        }
      }
    }

    internal void unitTest() {
      List<string> lTestnames = new List<string>() { "~!@#$%^&*()-_=+[]\\{}|;':\",./<>? + `", "Jaco", "24564dljdf", "jlsdhfoisljkdpsdnfkhsdfbkushf;osjd;fkd", "235366589546fdf#@$%%^^hsdfrhsd", "sdf    sdygsd   #@$^   5495" };
      foreach(string sName in lTestnames) {
        Console.WriteLine(oMQgateway.SendMessage("Hello my name is, " + sName, sQueue, sHost));
      }
      

    }
  }
}
