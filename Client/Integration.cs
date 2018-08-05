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
  }
}
