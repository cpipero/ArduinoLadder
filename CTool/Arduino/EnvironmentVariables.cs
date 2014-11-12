using System.IO;
using System;

namespace LadderLogic.CTool.Arduino
{
	[Serializable]
	public class EnvironmentVariables
	{
		public string ArduinoPath {get;set;}

		public string ArduinoBoard {get;set;}

		public string ArduinoPort {get;set;}
	}
}

