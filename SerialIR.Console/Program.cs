using CommandLine;
using System.Threading;
using SerialIR.Common;
using System.IO.Ports;

namespace SerialIR.Console
{

    class Options
    {
        [Option('p', "port", Default = null, HelpText = "COM port with Arduino.", SetName = "port")]
        public string Port { get; set; }

        [Option('l', "list", Default = false, HelpText = "List current COM ports.", SetName ="port")]
        public bool List { get; set; }

        [Option('c', "config", Default = null, HelpText = "Configuration/Mapping file.")]
        public string ConfigPath { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "-l"};

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       if (o.List)
                       {
                           System.Console.WriteLine("[i] Listing COM ports:");
                           foreach (string port in SerialPort.GetPortNames())
                           {
                               System.Console.WriteLine("[*] - " + port);
                           }
                       }
                       else if (o.Port != null)
                       {
                           var process = new SerialIRProcess(o.Port, o.ConfigPath, o.Verbose);
                           Thread mainthread = new Thread(process.StartLoop);
                           mainthread.Start();
                           mainthread.Join();
                       }
                       else
                       {
                           System.Console.WriteLine("[!] expecting '-l' or '-p <comport>', none received!");
                       }
                   });
        }
    }
}
