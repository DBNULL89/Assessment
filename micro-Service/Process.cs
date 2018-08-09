using System;
using System.Threading.Tasks;
using MQCore;

namespace micro_Service {
  class Process {
    static void Main(string[] args) {
      Integration oIntegration = new Integration();
      try {                
        oIntegration.ManageQueues();
        Environment.Exit(0);
      } catch (Exception oEx) {                
        Console.WriteLine("Critical error: " + oEx.Message);
        Console.WriteLine("Press anykey to terminate service.");
        Console.ReadKey();
        Console.WriteLine("Stopping all services");
        oIntegration.bRun = false;
        Task.WaitAll();
        Console.WriteLine("Services Stopped");
      }
    }
  }

}
