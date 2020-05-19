using CommandLine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

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
        // Create the serial port with basic settings
        private SerialPort Port;
        InputSimulator Sim = new InputSimulator();
        Configuration Config;

        bool Verbose = false;
        bool ReadOnly = false;

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       new Program(o.Port, o.ConfigPath, o.Verbose, o.ReadOnly);
                   });
        }

        private Program(string comport, string path, bool verbose, bool ronly)
        {
            try
            {
                Console.WriteLine("[i] Listening on port: " + comport);
                this.Verbose = verbose;
                this.ReadOnly = ronly;
                this.Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));

                this.Port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
                this.Port.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                this.Port.Open();

                // Enter an application loop to keep this thread alive
                while (true) ;
            }catch(Exception ex)
            {
                Console.WriteLine("[!] Something went wrong!");
            }
        }


        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = this.Port.ReadExisting().TrimEnd(Environment.NewLine.ToCharArray());
            if (data != "FFFFFFFF")
            {
                if(this.Verbose || this.ReadOnly)
                    Console.WriteLine(data+": received!");
                if (Config.Keys.ContainsKey(data) && !this.ReadOnly)
                {
                    Sim.Keyboard.KeyPress((VirtualKeyCode)Config.Keys[data]);
                }
                
            }
        }

    }
}
