using System;
using System.Linq;
using Cairo;

namespace LadderLogic.Brushes
{
	using Controller;
	using File.DrawingFile;

	public static class ConnectorBrush
	{
		public static void Draw(Context grw, Connector con)
		{
			var c = con.Selected ? AppController.Instance.Config.SelectedConnectorColor : 
				!con.ConnectedTo.Any() ? 
				con.Foregraund : 
				AppController.Instance.Config.ConnectedColor;

			if ( c != null) {
				grw.SetSourceRGB (
					c.Red, 
					c.Green, 
					c.Blue);
			}

			grw.Arc (
				con.Center.GeometryX, 
				con.Center.GeometryY, 
				con.GeometryRadius, 
				0, 2 * Math.PI);

			grw.StrokePreserve ();
			grw.Fill ();
		}
	}
}

