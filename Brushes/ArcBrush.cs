using Cairo;

namespace LadderLogic.Brushes
{
	using File.DrawingFile;

	public static class ArcBrush
	{
		public static void Draw (Context grw, ArcElement arc)
		{
			if (arc.Foregraund != null) {
				grw.SetSourceRGB (
					arc.Foregraund.Red, 
					arc.Foregraund.Green, 
					arc.Foregraund.Blue);
			}

			grw.Arc (
				arc.Center.GeometryX, 
				arc.Center.GeometryY, 
				arc.GeometryRadius, 
				arc.ArcStart, 
				arc.ArcStop);

			grw.Stroke ();
		}
	}
}

