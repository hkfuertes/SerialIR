using CommandLine;
using System.Threading;
using SerialIR.Common;

namespace SerialIR
{

    class Options
    {
        [Option('p', "port", Required = true, HelpText = "COM port with Arduino.")]
        public string Port { get; set; }

        [Option('c', "config", Required = true, HelpText = "Configuration/Mapping file.")]
        public string ConfigPath { get; set; }

        [Option('r', "read", Default = false, HelpText = "Just read code, not execute, ideal to learn key numbers.")]
        public bool ReadOnly { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
                string test_path = @"C:\Users\hkfuertes\source\repos\SerialIR\apple_silver_media.json";
                string test_com = "COM5";
                args = new string[] { "-p","", "-c", "" };
            */
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       //var process = new SerialIRProcess(test_com, test_path, o.Verbose, o.ReadOnly);
                       var process = new SerialIRProcess(o.Port, o.ConfigPath, o.Verbose, o.ReadOnly);
                       Thread mainthread = new Thread(process.StartLoop);
                       mainthread.Start();
                       mainthread.Join();
                   });
        }
    }
}
