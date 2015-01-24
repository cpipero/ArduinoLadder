#include <plcLib.h>

unsigned long START=1;
unsigned long STEP1=0;
unsigned long STEP2=0;
unsigned long STEP3=0;

void setup()
{
	setupPLC();
}


void loop()
{
	in(START);
	andBit(X0);
	set(STEP1);

	in(STEP1);
	reset(START);

	in(STEP1);
	andBit(X1);
	set(STEP2);

	in(STEP2);
	reset(STEP1);

	in(STEP2);
	andBit(X2);
	set(STEP3);

	in(STEP3);
	reset(STEP2);

	in(STEP3);
	andBit(X3);
	set(STEP1);

	in(STEP1);
	reset(STEP3);

	in(START);
	out(Y0);

	in(STEP1);
	out(Y1);

	in(STEP2);
	out(Y2);

	in(STEP3);
	out(Y3);


}