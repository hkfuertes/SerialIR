using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using WindowsInput;
using WindowsInput.Native;


namespace SerialIR.Common
{
    public class SerialIRProcess { 
        // Create the serial port with basic settings
        private SerialPort Port;
        InputSimulator Sim = new InputSimulator();
        Configuration Config;

        bool Verbose = false;
        bool ReadOnly = false;
        bool Stop = false;

        public  SerialIRProcess(string comport, string path, bool verbose)
        {
            try
            {
                if(verbose)
                    Console.WriteLine("[i] Listening on port: " + comport);
                this.Verbose = verbose;
                if (path != null)
                {
                    this.Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
                    this.ReadOnly = false;
                    if (verbose)
                    {
                        Console.WriteLine("[+] Configuration: " + path);
                        Console.WriteLine("[|] Remote: " + Config.Remote);
                        Console.WriteLine("[+] Profile: " + Config.Name);
                    }
                        
                }
                else
                {
                    this.Config = new Configuration();
                    this.Config.Keys = new Dictionary<string, int>();
                    this.ReadOnly = true;
                    if (verbose)
                        Console.WriteLine("[!] No configuration, going to readonly mode.");
                }

                this.Port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
                this.Port.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                //this.Port.Open();

            }
            catch (Exception ex)
            {
                this.Stop = true;
                Console.WriteLine("[!] Something went wrong!");
            }
        }

        public void RestartLoop()
        {
            this.Stop = false;
        }

        public void StartLoop()
        {
            if (!this.Stop)
            {
                this.Port.Open();
                while (!this.Stop);
                this.Port.Close();
            }
        }

        public void EndLoop()
        {
            this.Stop = true;
        }


        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = this.Port.ReadExisting().TrimEnd(Environment.NewLine.ToCharArray());
            if (data != "FFFFFFFF")
            {
                if (this.Verbose || this.ReadOnly)
                    Console.WriteLine("[i] "+ data + ": received!");
                if (Config.Keys.ContainsKey(data) && !this.ReadOnly)
                {
                    if (this.Verbose && !this.ReadOnly)
                        Console.WriteLine("[i] Executing " + Config.Keys?[data] + " key!");
                    Sim.Keyboard.KeyPress((VirtualKeyCode)Config.Keys?[data]);
                }

            }
        }

    }
}
