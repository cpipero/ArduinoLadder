using System;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class LineElement : Drawable
	{
		public Point Start { get; set; }


		public Point End { get; set; }
	}
}

