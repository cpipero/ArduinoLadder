using Cairo;

namespace LadderLogic.Brushes
{
	using File.DrawingFile;

	public static class RectangleBrush
	{
		public static void Draw(Context grw, RectangleElement rect)
		{
			if (rect.Foregraund != null) {
				grw.SetSourceRGBA (
					rect.Foregraund.Red, 
					rect.Foregraund.Green, 
					rect.Foregraund.Blue, 
					rect.Alpha);
			}

			grw.Rectangle (
				new Rectangle (
					rect.LeftBottom.GeometryX, 
					rect.LeftBottom.GeometryY, 
					rect.GeometryWidth, 
					rect.GeometryHeight));

			grw.StrokePreserve();
			grw.Fill();
		}
	}
}

