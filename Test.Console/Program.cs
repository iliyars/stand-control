using StanControl.StandaDriver.Services;
using System;


namespace Test.Console
{
    internal class Program
    {
       
        public static StandaDriverService driverService = new StandaDriverService();
        static void Main(string[] args)
        {
            driverService.ConnectStanda();
            System.Console.WriteLine("Hello, World!");
        }
    }
}