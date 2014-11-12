using System;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class ArcElement : Drawable
	{
		public Point Center { get; set; }


		public double GeometryRadius { get; set; }


		public double Radius { get; set; }


		public double ArcStart { get; set; }


		public double ArcStop { get; set; }
	}
}

