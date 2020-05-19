#include <IRremote.h>

int IR_PIN = 2;

IRrecv irDetect(IR_PIN);

decode_results irIn;

void setup()
{
  Serial.begin(9600);
  Serial.println("Starting...");
  irDetect.enableIRIn(); // Start the Receiver
}

void loop() {
  if (irDetect.decode(&irIn)) {
    Serial.println(irIn.value, HEX);
    irDetect.resume(); // Receive the next value
  }
}
