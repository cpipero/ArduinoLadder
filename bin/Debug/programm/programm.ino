#include <plcLib.h>

unsigned long AUX0;

void setup()
{
	setupPLC();
}


void loop()
{
	in(X0);
	andNotBit(X1);
	out(AUX0);

	in(X2);
	andBit(X3);
	orBit(AUX0);
	out(Y0);


}