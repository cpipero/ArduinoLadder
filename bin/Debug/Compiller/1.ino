#include <G:\Disk_H\Projects1\ladderlogic\LadderLogic\bin\Debug\Compiller\plcLib.h>

unsigned long timer0 = 0;
unsigned long timer1 = 0;

void setup()
{
	setupPLC();
}


void loop()
{
	in(X0);
	timerOn(timer0,2000);
	out(Y0);

	in(X1);
	timerOff(timer1,4000);
	out(Y1);


}