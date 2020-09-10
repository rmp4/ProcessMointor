using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace processmointor
{
    class Program
    {
        static void Main(string[] args)
        {
            string cmd =string.Join(" ", args); 
            
            var escapedArgs = cmd.Replace("\"", "\\\"");

            Console.WriteLine("Hello World!");
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = "/bin/bash",

                Arguments = $"-c \"{escapedArgs}\"",
                CreateNoWindow = true
            };

            using (var proc = new Process {StartInfo = info})
            {
                proc.Start();


                PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", proc.ProcessName);
                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", proc.ProcessName);
                while (true)
                {
                    Thread.Sleep(500);
                    double ram = ramCounter.NextValue();
                    double cpu = cpuCounter.NextValue();
                    Console.WriteLine("RAM: " + (ram / 1024 / 1024) + " MB; CPU: " + (cpu) + " %");
                }
            }
        }
    }
}
