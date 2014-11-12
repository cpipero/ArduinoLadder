/*
  plcLib Version 0.7, last updated 31st August, 2014.
  
  A simple Programmable Logic Controller (PLC) library for the
  Arduino and compatibles.

  Author:    W. Ditch
  Publisher: www.electronics-micros.com

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details, available from:
  <http://www.gnu.org/licenses/>
*/

#include "Arduino.h"
#include "plcLib.h"

extern unsigned int scanValue = 0;

// Define default pin directions and initial output levels.
void setupPLC() {

	// Basic input pins
	pinMode(X0, INPUT);
	pinMode(X1, INPUT);
	pinMode(X2, INPUT);
	pinMode(X3, INPUT);
	
	// Basic output pins
	pinMode(Y0, OUTPUT);
	pinMode(Y1, OUTPUT);
	pinMode(Y2, OUTPUT);
	pinMode(Y3, OUTPUT);

	// Additional pins for Mega, Mega 2560 and Due boards
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__) || defined(__SAM3X8E__)
		pinMode(X4, INPUT);
		pinMode(X5, INPUT);
		pinMode(X6, INPUT);
		pinMode(X7, INPUT);
		pinMode(Y4, OUTPUT);	
		pinMode(Y5, OUTPUT);
		pinMode(Y6, OUTPUT);	
		pinMode(Y7, OUTPUT);
	#endif
	
	//Motor Shield pins
	pinMode(DIRA, OUTPUT);
	pinMode(DIRB, OUTPUT);
	pinMode(PWMA, OUTPUT);
	pinMode(PWMB, OUTPUT);
	pinMode(BRAKEA, OUTPUT);	
	pinMode(BRAKEB, OUTPUT);
	pinMode(CURRENTA, INPUT);	
	pinMode(CURRENTB, INPUT);
	// (The Motor Shield also has four Tinkerkit compatible pins.
	// These are inputs X2-X3/A2-A3/I2-I3 and outputs Y1-Y2/D5-D6/O4-O3.)
	
	// Default output port values
	digitalWrite(Y0, LOW);
	digitalWrite(Y1, LOW);
	digitalWrite(Y2, LOW);
	digitalWrite(Y3, LOW);
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__) || defined(__SAM3X8E__)
	digitalWrite(Y4, LOW);
	digitalWrite(Y5, LOW);
	digitalWrite(Y6, LOW);
	digitalWrite(Y7, LOW);
	#endif
	digitalWrite(DIRA, LOW);
	digitalWrite(DIRB, LOW);
	digitalWrite(PWMA, LOW);
	digitalWrite(PWMB, LOW);
	digitalWrite(BRAKEA, LOW);		// Channel A Brake off
	digitalWrite(BRAKEB, LOW);		// Channel B Brake off
}

// Read an input pin (pin number supplied as integer)
unsigned int in(int input) {
	scanValue = digitalRead(input);
	return(scanValue);
}

// Read an auxiliary input (variable supplied as unsigned integer)
unsigned int in(unsigned int input) {
	scanValue = input;
	return(scanValue);
}

// Read an inverted input (pin number supplied as integer)
unsigned int inNot(int input) {
	if (digitalRead(input) == 1) {
		scanValue = 0;
	}
	else {
		scanValue = 1;
	}
	return(scanValue);
}

// Read an inverted auxiliary input (variable supplied as unsigned integer)
unsigned int inNot(unsigned int input) {
	if (input == 1) {
		scanValue = 0;
	}
	else {
		scanValue = 1;
	}
	return(scanValue);
}

// Read an analogue input (input pin supplied as an integer)
unsigned int inAnalog(int input) {
	scanValue = analogRead(input);
	return(scanValue);
}

// Read an analogue auxiliary value (variable supplied as an unsigned integer)
unsigned int inAnalog(unsigned int input) {
	scanValue = input;
	return(scanValue);
}

// Output to an output pin
unsigned int out(int output) {
	if (scanValue == 1) {
		digitalWrite(output, HIGH);
	}
	else {
		digitalWrite(output, LOW);
	}
	return(scanValue);
}

// Output to an auxiliary variable (variable type = unsigned integer)
unsigned int out(unsigned int &output) {
	if (scanValue == 1) {
		output = 1;
	}
	else {
		output = 0;
	}
	return(scanValue);
}

// Output to an output pin (inverted)
unsigned int outNot(int output) {
	if (scanValue == 1) {
		digitalWrite(output, LOW);
	}
	else {
		digitalWrite(output, HIGH);
	}
	return(scanValue);
}

// Output to an auxiliary variable (inverted) (variable type = unsigned integer)
unsigned int outNot(unsigned int &output) {
	if (scanValue == 1) {
		output = 0;
	}
	else {
		output = 1;
	}
	return(scanValue);
}

// Output a PWM value to an output pin (scanValue in range 0-1023)
unsigned int outPWM(int output) {
	analogWrite(output, scanValue / 4);
	return(scanValue);
}

// AND scanValue with input (pin number supplied as integer)
unsigned int andBit(int input) {
	scanValue = scanValue & digitalRead(input);
	return(scanValue);
}

// AND scanValue with auxiliary variable (variable supplied as unsigned integer)
unsigned int andBit(unsigned int input) {
	scanValue = scanValue & input;
	return(scanValue);
}

// AND scanValue with inverted input (pin number supplied as integer)
unsigned int andNotBit(int input) {
	scanValue = scanValue & ~digitalRead(input);
	return(scanValue);
}

// AND scanValue with inverted auxiliary variable (variable supplied as unsigned integer)
unsigned int andNotBit(unsigned int input) {
	scanValue = scanValue & ~input;
	return(scanValue);
}

// OR scanValue with input (pin number supplied as integer)
unsigned int orBit(int input) {
	scanValue = scanValue | digitalRead(input);
	return(scanValue);
}

// OR scanValue with auxiliary variable (variable supplied as unsigned integer)
unsigned int orBit(unsigned int input) {
	scanValue = scanValue | input;
	return(scanValue);
}

// OR scanValue with inverted input (pin number supplied as integer)
unsigned int orNotBit(int input) {
	if (scanValue == 1) {
	}
	else {
		if (digitalRead(input) == 0) {
			scanValue = 1;
		}
		else {
			scanValue = 0;
		}
	}
	return(scanValue);
}

// OR scanValue with inverted auxiliary variable (variable supplied as unsigned integer)
unsigned int orNotBit(unsigned int input) {
	if (scanValue == 1) {
	}
	else {
		if (input == 0) {
			scanValue = 1;
		}
		else {
			scanValue = 0;
		}
	}
	return(scanValue);
}

// XOR scanValue with input (pin number supplied as integer)
unsigned int xorBit(int input) {
	scanValue = scanValue ^ digitalRead(input);
	return(scanValue);
}

// XOR scanValue with auxiliary variable (variable supplied as unsigned integer)
unsigned int xorBit(unsigned int input) {
	scanValue = scanValue ^ input;
	return(scanValue);
}

// Set - Reset latch (output and reset pin numbers supplied as integers)
unsigned int latch(int output, int reset) {
	scanValue = scanValue | digitalRead(output);		// Self latch by ORing with Output pin (Q)
	scanValue = scanValue & ~digitalRead(reset);		// AND-Not with Reset Pin
	if (scanValue == 1) {
		digitalWrite(output, HIGH);
	}
	else {
		digitalWrite(output, LOW);
	}
	return(scanValue);
}

// Set - Reset latch (output pin number supplied as integer, reset as unsigned integer variable)
unsigned int latch(int output, unsigned int reset) {
	scanValue = scanValue | digitalRead(output);		// Self latch by ORing with Output pin (Q)
	scanValue = scanValue & ~reset;						// AND-Not with Reset variable
	if (scanValue == 1) {
		digitalWrite(output, HIGH);
	}
	else {
		digitalWrite(output, LOW);
	}
	return(scanValue);
}

// Set - Reset latch (output as unsigned integer variable and reset pin as integer)
unsigned int latch(unsigned int &output, int reset) {
	scanValue = scanValue | output;						// Self latch by ORing with Output pin (Q)
	scanValue = scanValue & ~digitalRead(reset);		// AND-Not with Reset Pin
	if (scanValue == 1) {
		output = 1;
	}
	else {
		output = 0;
	}
	return(scanValue);
}

// Set - Reset latch (output and reset values are unsigned integer auxiliary variables)
unsigned int latch(unsigned int &output, unsigned int reset) {
	scanValue = scanValue | output;				// Self latch by ORing with Output variable (Q)
	scanValue = scanValue & ~reset;				// AND-Not with Reset variable
	if (scanValue == 1) {
		output = 1;
	}
	else {
		output = 0;
	}
	return(scanValue);
}

unsigned int timerOn(unsigned long &timerState, unsigned long timerPeriod) {
	if (scanValue == 0) {									// timer is disabled
		timerState = 0;										// Clear timerState (0 = 'not started')
	}
	else {													// Timer is enabled
		if (timerState == 0) {								// Timer hasn't started counting yet
			timerState = millis();							// Set timerState to current time in milliseconds
			scanValue = 0;									// Result = 'not finished' (0)
		}
		else {												// Timer is active and counting
			if (millis() > (timerState + timerPeriod)) {	// Timer has finished
				scanValue = 1;								// Result = 'finished' (1)
			}
			else {											// Timer has not finished
				scanValue = 0;								// Result = 'not finished' (0)
			}
		}
	}
	return(scanValue);										// Return result (1 = 'finished',
															// 0 = 'not started' / 'not finished')
}

unsigned int timerOff(unsigned long &timerState, unsigned long timerPeriod) {
	if (scanValue == 0) {									// Timer input is off (scanValue = 0)
		if (timerState == 0) {								// Timer is not started so do nothing
		}
		else {												// Timer is active and counting
			if (millis() > (timerState + timerPeriod)) {	// Timer has finished
				scanValue = 0;								// Result = 'turn-off delay finished' (0)
			}
			else {											// Timer has not finished
				scanValue = 1;								// Result = 'turn-off delay not finished' (1)
			}
		}
	}
	else {													// Timer input is high (scanValue = 1)
		timerState = millis();								// Set timerState to current time in milliseconds
	}
	return(scanValue);										// Return result (1 = 'pulse On' / 'turn-off delay in progress',
															// 0 = 'not started' / 'finished')
}

unsigned int timerCycle(unsigned long &timer1State, unsigned long timer1Period, unsigned long &timer2State, unsigned long timer2Period) {
	if (scanValue == 0) {									// Enable input is off (scanValue = 0)
		timer2State = 0;									// Ready to start LOW pulse period when enabled
		timer1State = 1;
	}
	else {													// Enabled
		if (timer2State == 0) {								// Low pulse Active
			if (timer1State == 1) {							// LOW pulse period starting
				timer1State = millis();						// Set timerState to current time in milliseconds
			}
			else if (millis() > (timer1State + timer1Period)) {	// Low pulse period has finished
				timer1State = 0;	
				timer2State = 1;							// Ready to start HIGH pulse period
			}
			scanValue = 0;									// Result = 'Pulse LOW' (0)
		}
		if (timer1State == 0) {								// High pulse Active
			if (timer2State == 1) {							// HIGH pulse period starting
				timer2State = millis();						// Set timerState to current time in milliseconds
			}
			else if (millis() > (timer2State + timer2Period)) {	// High pulse has finished
				timer2State = 0;	
				timer1State = 1;							// Ready to start LOW pulse period
			}
			scanValue = 1;									// Result = 'Pulse HIGH' (1)
		}
	}
	return(scanValue);
}

// Test whether an analogue input is greater than a second analogue input
unsigned int compareGT(int input) {
	if (scanValue > analogRead(input)) {
		scanValue = 1;
	}
	else {
		scanValue = 0;
	}
	return(scanValue);
}

// Test whether an analogue input is greater than a fixed value
unsigned int compareGT(unsigned int input) {
	if (scanValue > input) {
		scanValue = 1;
	}
	else {
		scanValue = 0;
	}
	return(scanValue);
}

// Test whether an analogue input is less than a second analogue input
unsigned int compareLT(int input) {
	if (scanValue < analogRead(input)) {
		scanValue = 1;
	}
	else {
		scanValue = 0;
	}
	return(scanValue);
}

// Test whether an analogue input is less than a fixed value
unsigned int compareLT(unsigned int input) {
	if (scanValue < input) {
		scanValue = 1;
	}
	else {
		scanValue = 0;
	}
	return(scanValue);
}

// Set a latched output (output pin number supplied as integer)
unsigned int set(int output) {
	scanValue = scanValue | digitalRead(output);		// Self latch by ORing with Output pin
	if (scanValue == 1) {
		digitalWrite(output, HIGH);
	}
	return(scanValue);
}

// Set a latched output (output pin number supplied as unsigned integer)
unsigned int set(unsigned int &output) {
	scanValue = scanValue | output;		// Self latch by ORing with Output pin
	if (scanValue == 1) {
		output = 1;
	}
	return(scanValue);
}

// reset (or clear) a latched output (output pin number supplied as integer)
unsigned int reset(int output) {
	if (scanValue == 1) {
		digitalWrite(output, LOW);
	}
	return(scanValue);
}

// reset (or clear) a latched output (output pin number supplied as unsigned integer)
unsigned int reset(unsigned int &output) {
	if (scanValue == 1) {
		output = 0;
	}
	return(scanValue);
}

