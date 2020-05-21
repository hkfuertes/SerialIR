using CommandLine;
using System.Threading;

namespace SerialIR.Console
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
            //args = new string[] { "-p","COM7", "-v"};

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
