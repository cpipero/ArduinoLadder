using System.Linq;
using Cairo;

namespace LadderLogic.Brushes
{
	using Controller;
	using Surface;

	public static class SurfaceLineBrush
	{
		public static void Draw(Context grw, Line line)
		{
			var color = AppController.Instance.Config.LineColor;
			grw.SetSourceRGB (
				color.Red, 
				color.Green, 
				color.Blue);

			var segments = AppController.Instance.Surface.Segments;

			var s1 = segments.FirstOrDefault (s => s.Position.X == line.Input.X 
				&& s.Position.Y == line.Input.Y);

			var s2 = segments.FirstOrDefault (s => s.Position.X == line.Output.X 
				&& s.Position.Y == line.Output.Y);
			
			if (s1 == null || s2 == null)
			{
				return;
			}

			//todo : use one func.
			var start = s1.Connectors
				.FirstOrDefault (p => p.Marker == line.InputMarker);

			//todo : use one func.
			var stop = s2.Connectors
				.FirstOrDefault (p => p.Marker == line.OutputMarker);
			if (start == null || stop == null)
			{
				return;
			}

			grw.MoveTo (
				start.Center.GeometryX, 
				start.Center.GeometryY);

			if (line.Input.X == line.Output.X ||
			   line.Input.Y == line.Output.Y) {
				grw.LineTo (
					stop.Center.GeometryX, 
					stop.Center.GeometryY);    
			} else {
				grw.LineTo (
					stop.Center.GeometryX, 
					start.Center.GeometryY);    

				grw.LineTo (
					stop.Center.GeometryX, 
					stop.Center.GeometryY);    
			}

			grw.Stroke();
		}
	}
}

