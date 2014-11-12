using System.Collections.Generic;

namespace LadderLogic.Controller.State
{
	using File.DrawingFile;
	using Surface;
	using File.Config;

	public class LineState : State
	{
		public LineState () : base(StateType.LineState)
		{
		}


		public Segment LeftSegment { get; set; } 


		public Segment RightSegment { get; set ; }


		public string LeftMarker { get; set; }


		public string RightMarker { get; set; }


		public bool Left { get; set; }


		readonly LocalConfig _cfg = AppController.Instance.Config;

		#region implemented abstract members of State

		public override bool Handle (State previous, Segment prevSegment, Segment newSegment, bool left)
		{
			// disable not ladder lines 
 			if (RightSegment != null && newSegment != null) {

				if (RightSegment.Position.X > newSegment.Position.X &&
					RightSegment.Position.Y == newSegment.Position.Y) {
					return true;
				}

				if (RightSegment.Position.X == newSegment.Position.X &&
					RightSegment.Position.Y < newSegment.Position.Y) {
					return true;
				}

				if (RightSegment.Position == newSegment.Position) {
					return true;
				}
			}

			if (newSegment != null && newSegment.Type == ElementType.None) {
				if (RightSegment == null) {
					if (newSegment.Position.X == 0) { // left power line
						var con = new Connector {
							Center = new Point {
								X = 0,
								Y = 0.5
							},
							Marker = _cfg.LeftPower,
							Radius = 0.03,
							Foregraund = new Color {
								Red = 1,
								Green = 0,
								Blue = 0
							},
							ConnectedTo = new HashSet<string> ()
						};
						newSegment.Connectors.Add(con);
						RightSegment = newSegment;
						RightMarker = _cfg.LeftPower;
						con.Selected = true;
					}
				} 
			}

			if (newSegment != null && (!newSegment.IsPalette 
			                           && newSegment.Type != ElementType.None)) {
				if (RightSegment == null)
				{
					RightSegment = newSegment;

					RightMarker = left ? 
						AppController.Instance.SelectLeftConnector (RightSegment) :
						AppController.Instance.SelectRightConnector (RightSegment);
				}
				else
				{
					LeftSegment = newSegment;

					LeftMarker = left ? AppController.Instance.SelectLeftConnector (LeftSegment) :
						AppController.Instance.SelectRightConnector (LeftSegment);

					AppController.Instance.Surface.AddPowerLine (
						new Line (RightSegment.Position, LeftSegment.Position, RightMarker, LeftMarker));
				}
			}
 			
			base.Handle (previous, prevSegment, newSegment, left);
			return true;
		}

		public override bool Leave()
		{
			AppController.Instance.ClearConnectorSelection ();
			return true;
		}

		#endregion
	}
}