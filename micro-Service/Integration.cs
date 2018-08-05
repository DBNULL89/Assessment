using System;
using System.Threading.Tasks;
using MQCore;

namespace micro_Service {
  class Integration {
    static void Main(string[] args) {
      Process oProcess = new Process();
      try {        
        Console.WriteLine("Service Started");

        oProcess.ManageQueues();
      } catch (Exception oEx) {                
        Console.WriteLine("Critical error: " + oEx.Message);
        Console.WriteLine("Press anykey to terminate service.");
        Console.ReadKey();
        Console.WriteLine("Stopping all services");
        oProcess.bRun = false;
      }
    }
  }

}
