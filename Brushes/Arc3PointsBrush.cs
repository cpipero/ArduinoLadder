using System;
using Cairo;

namespace LadderLogic.Brushes
{
	using File.DrawingFile;

	public static class Arc3PointsBrush
	{
		public static void Draw(Context grw, Arc3PointsElement arc)
		{
			const double eps = 0.000001;
			double arcStart;
			double arcEnd;

			if (arc.Foregraund != null)
			{
				grw.SetSourceRGB(
					arc.Foregraund.Red,
					arc.Foregraund.Green,
					arc.Foregraund.Blue);
			}

			var arcCenter = new PointD(0, 0);
			
			var yDeltaA = arc.Middle.GeometryY - arc.Start.GeometryY;
			var xDeltaA = arc.Middle.GeometryX - arc.Start.GeometryX;

			var yDeltaB = arc.End.GeometryY - arc.Middle.GeometryY;
			var xDeltaB = arc.End.GeometryX - arc.Middle.GeometryX;

			//common case - no perpendicular & collinear lines
			if ((Math.Abs(xDeltaA) > eps)
				&& (Math.Abs(xDeltaB) > eps)
				&& (Math.Abs(yDeltaA) > eps)
				&& (Math.Abs(yDeltaB) > eps))
			{

				var aSlope = yDeltaA / xDeltaA;
				var bSlope = yDeltaB / xDeltaB;

				if (Math.Abs(aSlope - bSlope) <= eps)
				{
					//throw new ArgumentException("3 points lie at one line");
					return;
				}

				arcCenter.X = (aSlope * bSlope * (arc.Start.GeometryY - arc.End.GeometryY)
					+ bSlope * (arc.Start.GeometryX + arc.Middle.GeometryX)
					- aSlope * (arc.Middle.GeometryX + arc.End.GeometryX)) / (2 * (bSlope - aSlope));

				arcCenter.Y = -1 * (arcCenter.X - (arc.Start.GeometryX + arc.Middle.GeometryX) / 2)
					/ aSlope + (arc.Start.GeometryY + arc.Middle.GeometryY) / 2;
			}
			else
			{
				//vertical or horizontal cases
				if (Math.Abs(xDeltaA) <= eps)
				{
					//1st is vertical
					if (Math.Abs(xDeltaB) <= eps)
					{
						//2nd is vertical too
						//throw new ArgumentException("Both lines are vertical");
						return;
					}

					if (Math.Abs(yDeltaB) > eps)
					{
						// 2nd is not horizontal
						//throw new NotImplementedException("Only first vertical");
						return;
					}

					//square angle
					arcCenter.X = 0.5 * (arc.Middle.GeometryX + arc.End.GeometryX);
					arcCenter.Y = 0.5 * (arc.Start.GeometryY + arc.Middle.GeometryY);
				}

				if (Math.Abs(yDeltaA) <= eps)
				{
					//1st is horizontal
					if (Math.Abs(yDeltaB) <= eps)
					{
						//2nd is horizontal too
						//throw new ArgumentException("Both line are horizontal");
						return;
					}

					if (Math.Abs(xDeltaB) > eps)
					{
						//1st is not horizontal
						//throw new NotImplementedException("Only first horizontal");
						return;
					}

					//square angle
					arcCenter.X = 0.5 * (arc.Start.GeometryX + arc.Middle.GeometryX);
					arcCenter.Y = 0.5 * (arc.Middle.GeometryY + arc.End.GeometryY);
				}
			}

			//radius
			var arcRadius = Math.Sqrt(Math.Pow(arc.Start.GeometryX - arcCenter.X, 2)
			                             + Math.Pow(arc.Start.GeometryY - arcCenter.Y, 2));

			//arc angles
			var xStartDelta = arc.Start.GeometryX - arcCenter.X;
			var yStartDelta = arc.Start.GeometryY - arcCenter.Y;

			var xEndDelta = arc.End.GeometryX - arcCenter.X;
			var yEndDelta = arc.End.GeometryY - arcCenter.Y;

			//start of arc
			if (Math.Abs(xStartDelta) < eps)
			{
				if (yStartDelta < 0.0)
				{
					arcStart = -0.5 * Math.PI;
				}
				else
				{
					arcStart = 0.5 * Math.PI;
				}
			}
			else
			{
				arcStart = Math.Atan2(yStartDelta, xStartDelta);
			}

			//end of arc
			if (Math.Abs(xEndDelta) < eps)
			{
				if (yEndDelta < 0.0)
				{
					arcEnd = -0.5 * Math.PI;
				}
				else
				{
					arcEnd = 0.5 * Math.PI;
				}
			}
			else
			{
				arcEnd = Math.Atan2(yEndDelta, xEndDelta);
			}

			if (Math.Sign((arc.Middle.GeometryX - arc.Start.GeometryX)
				* (arc.Middle.GeometryY - arc.End.GeometryY)
				- (arc.Middle.GeometryY - arc.Start.GeometryY)
				* (arc.Middle.GeometryX - arc.End.GeometryX)) < 0)
			{
				grw.Arc(
					arcCenter.X,
					arcCenter.Y,
					arcRadius,
					arcStart,
					arcEnd);
			}
			else
			{
				grw.ArcNegative(
					arcCenter.X,
					arcCenter.Y,
					arcRadius,
					arcStart,
					arcEnd);
			}

			/*			grw.MoveTo (
							arc.Start.GeometryX, 
							arc.Start.GeometryY);
						grw.LineTo (
							arc.Middle.GeometryX, 
							arc.Middle.GeometryY);
						grw.LineTo (
							arc.End.GeometryX, 
							arc.End.GeometryY);*/

			grw.Stroke();
		}
	}
}

