using Newtonsoft.Json;
using SerialIR.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using WindowsInput;
using WindowsInput.Native;

namespace SerialIR.Console
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
                    System.Console.WriteLine("[i] Listening on port: " + comport);
                this.Verbose = verbose;
                if (path != null)
                {
                    this.Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
                    this.ReadOnly = false;
                    if (verbose || true)
                    {
                        System.Console.WriteLine("[+] Configuration: " + path);
                        System.Console.WriteLine("[|] Remote: " + Config.Remote);
                        System.Console.WriteLine("[+] Profile: " + Config.Name);
                    }
                        
                }
                else
                {
                    this.Config = new Configuration();
                    this.Config.Keys = new Dictionary<string, string>();
                    this.ReadOnly = true;
                    if (verbose || true)
                        System.Console.WriteLine("[!] No configuration, going to readonly mode.");
                }

                this.Port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
                this.Port.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                //this.Port.Open();

            }
            catch (Exception ex)
            {
                this.Stop = true;
                System.Console.WriteLine("[!] Something went wrong!");
                System.Console.WriteLine("[!] Exception: "+ ex.Message);
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
                    System.Console.WriteLine("[i] "+ data + ": received!");
                if (Config.Keys.ContainsKey(data) && !this.ReadOnly)
                {
                    if (this.Verbose && !this.ReadOnly)
                        System.Console.WriteLine("[i] Executing Key: " + Config.Keys?[data]);

                    int valueToSend = getEnumValue(this.Config.Keys?[data]);
                    if(valueToSend > -1)
                    {
                        Sim.Keyboard.KeyPress((VirtualKeyCode)valueToSend);
                    } else if (this.Verbose)
                    {
                        System.Console.WriteLine("[!] Key "+ Config.Keys?[data] + " not recognized!");
                    }
                }

            }
        }

        private int getEnumValue(string value)
        {
            int val;
            
            if (Int32.TryParse(value, out val))
            {
                return val;
            }

            VirtualKeyCode retVal;
            if (Enum.TryParse(value, out retVal))
            {
                return (int)retVal;
            }

            return -1 ;
        }

    }
}
