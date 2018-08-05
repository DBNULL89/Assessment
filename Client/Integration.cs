using System;
using System.IO;

namespace Client {
  class Integration {
    
    static void Main(string[] args) {
      try {
        MQCore.Core oMQgateway = new MQCore.Core();
        string sMessage;
        string sQueue = "Verify-Name";
        string sHost = "localhost";
        bool bRun = true;

        Console.WriteLine("Please enter name");
        sMessage = Console.ReadLine();       

        sMessage = oMQgateway.SendMessage("Hello my name is, " + sMessage, sQueue,sHost);
        Console.WriteLine(sMessage);

      } catch (Exception oEx) {
        Console.WriteLine("unable to process. " + oEx.Message);
        Console.WriteLine("Press anykey to terminate session.");
        Console.ReadLine();
      }
    }

    internal void ConsoleListener() {
      while (bRun) {
        string scommand = Console.ReadLine();
        switch (scommand.ToUpper()) {
          case "HELP": {
              Console.WriteLine("********Available Commands********");
              Console.WriteLine("Exit - Stops all services and exit the console application");
              break;
            }
          case "EXIT": {
              bRun = false;
              break;
            }
        }
      }
    }
  }
}
