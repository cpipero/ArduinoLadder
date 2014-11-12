using System;

namespace LadderLogic.File.DrawingFile
{
	[Serializable]
	public class RectangleElement : Drawable
	{
		public Point LeftBottom { get; set; }


		public double Width { get; set; }


		public double Height { get; set; }


		public double GeometryWidth { get; set; }


		public double GeometryHeight { get; set; }
	}
}

