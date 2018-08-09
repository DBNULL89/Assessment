using System;
using System.IO;

namespace Client {
  class Process {
    
    static void Main(string[] args) {
      try {
        Integration oIntegration = new Integration();
        oIntegration.ManageConsole();
        Environment.Exit(0);
      } catch (Exception oEx) {
        Console.WriteLine("unable to process. " + oEx.Message);
        Console.WriteLine("Press anykey to terminate session.");
        Console.ReadLine();
      }
    }


  }
}
