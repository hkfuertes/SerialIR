#include <IRremote.h>

int IR_PIN = 6;
int V_PIN = 7;
int G_PIN = 8;

IRrecv irDetect(IR_PIN);

decode_results irIn;

void setup()
{
  Serial.begin(9600);
  Serial.println("Starting...");

  pinMode(V_PIN, OUTPUT);
  pinMode(G_PIN, OUTPUT);

  digitalWrite(G_PIN, LOW);
  digitalWrite(V_PIN, HIGH);
  
  irDetect.enableIRIn(); // Start the Receiver
}

void loop() {
  if (irDetect.decode(&irIn)) {
    Serial.println(irIn.value, HEX);
    irDetect.resume(); // Receive the next value
  }
}
