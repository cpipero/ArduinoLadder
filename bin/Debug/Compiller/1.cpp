#include <plcLib.h>



void setup()
{
	setupPLC();
}


void loop()
{
	in(X0);
	orNotBit(X1);
		latch(X0,X1);
	out(Y0);


}