using Cairo;

namespace LadderLogic.Brushes
{
	using File.DrawingFile;

	public static class LineBrush
	{
		public static void Draw(Context grw, LineElement line)
		{
			if (line.Foregraund != null) {
				grw.SetSourceRGB (
					line.Foregraund.Red, 
					line.Foregraund.Green, 
					line.Foregraund.Blue);
			}

			grw.MoveTo(
				line.Start.GeometryX, 
				line.Start.GeometryY);
			grw.LineTo(
				line.End.GeometryX, 
				line.End.GeometryY);    

			grw.Stroke();
		}
	}
}

