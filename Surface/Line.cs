using System;

namespace LadderLogic.Surface
{
	[Serializable]
	public class Line
	{
		public Line ()
		{
		}


		public Line (Position input, Position output, string inputMarker, string outputMarker)
		{
			Input = input;
			Output = output;
			InputMarker = inputMarker;
			OutputMarker = outputMarker;
		}


		public Position Input{ get; set; } 


		public Position Output{ get; set; }


		public string InputMarker{ get; set; }


		public string OutputMarker { get; set; }
	}
}

