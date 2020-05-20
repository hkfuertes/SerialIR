using CommandLine;
using System.Threading;
using SerialIR.Common;

namespace SerialIR
{

    class Options
    {
        [Option('p', "port", Required = true, HelpText = "COM port with Arduino.")]
        public string Port { get; set; }

        [Option('c', "config", Default = null, HelpText = "Configuration/Mapping file.")]
        public string ConfigPath { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "-p","COM5", "-c", @"C:\Users\hkfuertes\source\repos\apple_silver_media_codes.json"};

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var process = new SerialIRProcess(o.Port, o.ConfigPath, o.Verbose);
                       Thread mainthread = new Thread(process.StartLoop);
                       mainthread.Start();
                       mainthread.Join();
                   });
        }
    }
}
