# Simple Serial IR-Arduino Bridge
The main goal of this project is to provide a serial connection betwen an Arduino (using now an Arduino Nano) and a Windows (or any .Net Core compatible) system.

## Arduino Sketch
You can find the sketch [here](SerialIR.ino). In order to compile it you need the __*Arduino-IRremote*__ library.

 >Arduino-IRremote: https://github.com/z3t0/Arduino-IRremote or also via _Arduino IDE Library Manager_.

## Command Line Application
To run the application simply open a terminal an run the `SerialIR.Console.exe` with the required `-c` and `-p` parameters.
```
> .\SerialIR.Console.exe --help
SerialIR.Console 1.0.0.0
Copyright c  2020

  -p, --port       COM port with Arduino.

  -l, --list       (Default: false) List current COM ports.

  -c, --config     Configuration/Mapping file.

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
		"77E150A4": "VOLUME_UP",
		"77E130A4": 174,
		"77E1A0A4": "MEDIA_PLAY_PAUSE",
		"77E160A4": "MEDIA_NEXT_TRACK",
		"77E190A4": "MEDIA_PREV_TRACK"
	}
}
```

The HEX value in the `keys` array is the code registered by the Arduino IR receiver. You can run `SerialIR.exe -p <COMPORT> -v` to just read the codes.

The decimal value in the `keys` array is the computer key to be pressed, see [VirtualKeyCode.cs](VirtualKeyCode.cs) for the complete list of keycodes. You can either put the value (`174`) or the name of the value (`VOLUME_DOWN`).

You can add as much keys as you want, and use _(theorically)_ whatever remote you have as long as it is IR. 