# Simple Serial IR-Arduino Bridge
The main goal of this project is to provide a serial connection betwen an Arduino (using now an Arduino Nano) and a Windows (or any .Net Core) system.

## Command Line Application
To run the application simply open a terminal an run the `SerialIR.exe` with the required `-c` and `-p` parameters.
```
> SerialIR.exe
SerialIR 1.0.0
Copyright (C) 2020 SerialIR

ERROR(S):
  Required option 'p, port' is missing.
  Required option 'c, config' is missing.

  -p, --port       Required. COM port with Arduino.

  -c, --config     Required. Configuration/Mapping file.

  -r, --read       (Default: false) Just read code, not execute, ideal to learn key numbers.

  -v, --verbose    (Default: false) Prints all messages to standard output.

  --help           Display this help screen.

  --version        Display version information.
```

For the configuration/mapping file here is an example for the Apple Silver Remote:
```json
{
	"name": "Multimedia",
	"remote": "Apple Remote Silver",
	"keys": {
		"77E150A4": 175,
		"77E130A4": 174,
		"77E1A0A4": 179,
		"77E160A4": 176,
		"77E190A4": 177
	}
}
```

The HEX value in the `keys` array is the code registered by the Arduino IR receiver. You can run `SerialIR.exe -p <COMPORT> -c not-used -r` to just read the codes.

The decimal value in the `keys` array is the numeric representation of the key, see [VirtualKeyCode.cs](VirtualKeyCode.cs) for the complete list of keycodes.

You can add as much keys as you want, and use _(theorically)_ whatever remote you have as long as it is IR. 