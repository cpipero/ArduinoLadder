using System;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class Arc3PointsElement : Drawable
	{
		public Point Start { get; set; }


		public Point Middle { get; set; }


		public Point End { get; set; }
	}
}

