using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using LadderLogic.CTool.FunctionRule;

namespace LadderLogic.Geometry
{
	using Controller;
	using File.DrawingFile;
	using Surface;
	using File.Config;
	using CTool;

	public class Proection
	{
		readonly PrimitivesSurface _surface;


		readonly LocalConfig _cfg = AppController.Instance.Config;


		Position _maxPosition;


		uint _hMargin = AppController.Instance.Config.GridHMargin;


		uint _vMargin = AppController.Instance.Config.GridVMargin;


		public Proection (PrimitivesSurface surface)
		{
			_surface = surface;
			CalculateSurfaceAxis ();
			CreateGridLines ();
			if (!surface.IsPalette && !surface.IsButton) {
				CreatePowerLines ();
				CreateAxisLabels ();
			}
		}


		public uint Width { get; private set; }


		public uint Height { get; private set; }


		public void SetSize(uint width, uint height, uint hMargin, uint vMargin)
		{
			_hMargin = hMargin;
			_vMargin = vMargin;
			Width = width - hMargin * 2;
			Height = height - vMargin * 2;
			Calculate ();
		}


		public void Calculate()
		{
			CalculateSurfaceAxis ();
			CalculatePrimitives ();
		}


		public IEnumerable<Drawable> Get()
		{
			return _surface.Get().SelectMany(seg=> seg
				.Primitives
				.Where(p => !_surface.IsPalette
					|| (p.Marker == _cfg.Caption || !(p is TextElement)) ))
					.OrderBy(p => p.ZIndex)
					.ToList();
		}


		public IEnumerable<Connector> Connectors()
		{
			if (_surface.IsPalette && !_surface.IsButton) {
				return new List<Connector> ();
			}
			return _surface.Get().SelectMany(seg=> seg
				.Connectors
				.OrderBy(p => p.ZIndex))
					.ToList();
		}


		public IEnumerable<Line> Lines()
		{
			return _surface.Lines ();
		}


		void CalculateSurfaceAxis ()
		{
			_maxPosition = new Position ( _surface.Width, _surface.Height );

			var xOffset =  (_maxPosition.X > 0 ? 
				Width / (_maxPosition.X + 1) : 
				Width);

			var yOffset =  (_maxPosition.Y > 0 ? 
				Height / (_maxPosition.Y + 1) : 
				Height);

			foreach (var seg in _surface.Get()) {
				seg.Position.GeometryXStart = xOffset * seg.Position.X + _hMargin;
				seg.Position.GeometryXEnd = xOffset * (seg.Position.X + 1) + _hMargin;
				seg.Position.GeometryYStart = yOffset * seg.Position.Y + _vMargin;
				seg.Position.GeometryYEnd = yOffset * (seg.Position.Y + 1) + _vMargin;
			}
		}


		void CreatePowerLines()
		{
			foreach (var seg in _surface.Get().Where(seg=> !seg.IsPalette)) {
				// Plas line
				if (seg.Position.X == 0) {
					seg.AddPrimitives (new List<Drawable>
						{ 
							new LineElement{ 
								Foregraund = _cfg.PowerColor,
								Start = new Point{ X = 0, Y = 0 },
								End = new Point{ X = 0,	Y = 1 },
								ZIndex = 25,
								Marker = _cfg.LeftPower
							} 
						});
				}

				// Minus line
				if (seg.Position.X == _surface.Width) {
					seg.AddPrimitives (new List<Drawable>
						{ 
							new LineElement{ 
								Foregraund = _cfg.PowerColor,
								Start = new Point{ X = 1, Y = 0 },
								End = new Point{ X = 1,	Y = 1 },
								ZIndex = 25,
								Marker = _cfg.RightPower
							} 
						});
				}
			}
		}


		void CreateAxisLabels()
		{
			foreach (var seg in _surface.Get()) {
				var i = seg.Position.X;
				var j = seg.Position.Y;

				if (j == 0) {
					seg.AddPrimitives (new List<Drawable> {
						new TextElement {
							Alpha = 1,
							Foregraund = _cfg.AxisColor,
							Weight = 1,
							ZIndex = 1,
							Text = i < _maxPosition.X ? "Contact " + (i + 1) : "Coil",
							Align = _cfg.AxisAlign,
							VAlign = _cfg.AxisVAlign,
							Start = new Point {
								X = _cfg.XAxisOffset,
								Y = _cfg.YAxisOffset
							},
							FontFamily = _cfg.AxisFont,
							FontSize = _cfg.XAxisFontSize,
							FixedSize = true,
							FixedY = true,
							Marker = _cfg.ColumnLabel
						}
					});
				}

				if (i == 0) {
					seg.AddPrimitives (new List<Drawable> {
						new TextElement {
							Alpha = 1,
							Foregraund = _cfg.AxisColor,
							Weight = 1,
							ZIndex = 1,
							Text = j.ToString (CultureInfo.InvariantCulture),
							Align = _cfg.AxisAlign,
							VAlign = _cfg.AxisVAlign,
							Start = new Point {
								X = _cfg.YAxisOffset,
								Y = _cfg.XAxisOffset
							},
							FontFamily = _cfg.AxisFont,
							FontSize = _cfg.YAxisFontSize,
							FixedSize = true,
							FixedX = true,
							Marker = _cfg.ColumnLabel
						}
					});
				}
			}
		}


		void CreateGridLines()
		{
			foreach (var seg in _surface.Get()) {
				var gridLines = new List<Drawable>
				{
					new RectangleElement
					{
						Alpha = 0,
						Foregraund = _cfg.SelectedBg,
						LeftBottom = new Point {X = 0, Y = 0},
						Width = 1,
						Height = 1,
						Marker = _cfg.Selected
					},
					new LineElement
					{
						Foregraund = _cfg.GridLinesColor,
						Start = new Point {X = 0, Y = 0},
						End = new Point {X = 1, Y = 0}
					},
					new LineElement
					{
						Foregraund = _cfg.GridLinesColor,
						Weight = 1,
						Start = new Point {X = 1, Y = 0},
						End = new Point {X = 1, Y = 1}
					},
					new LineElement
					{
						Foregraund = _cfg.GridLinesColor,
						Weight = 1,
						Start = new Point {X = 1, Y = 1},
						End = new Point {X = 0, Y = 1}
					},
					new LineElement
					{
						Foregraund = _cfg.GridLinesColor,
						Weight = 1,
						Start = new Point {X = 0, Y = 1},
						End = new Point {X = 0, Y = 0}
					}
				};

				foreach (var p in gridLines) {
					CalculatePrimitive (p, seg);
				}

				seg.AddPrimitives (gridLines);
			}

		}


		void CalculatePrimitives ()
		{		
			foreach (var seg in _surface.Get()) {
				foreach (var p in seg.Primitives) {
					CalculatePrimitive (p, seg);
				}

				foreach (var p in seg.Connectors) {
					CalculatePrimitive (p, seg);
				}
			}
		}


		void CalculatePrimitive(Drawable p, Segment seg)
		{
			var l = p as LineElement;
			if (l != null) {
				l.Start.GeometryX = GetGeometryPosition(
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					l.Start.X);

				l.Start.GeometryY = GetGeometryPosition(
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight,  
					l.Start.Y);

				l.End.GeometryX = GetGeometryPosition(
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					l.End.X);

				l.End.GeometryY = GetGeometryPosition(
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight, 
					l.End.Y);
			}

			var a = p as ArcElement;
			if (a != null) {
				a.Center.GeometryX = GetGeometryPosition(
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					a.Center.X);

				a.Center.GeometryY = GetGeometryPosition(
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight, 
					a.Center.Y);

				a.GeometryRadius = seg.Position.GeometryWidth * a.Radius;
			}

			var a3 = p as Arc3PointsElement;
			if (a3 != null) {
				a3.Start.GeometryX = GetGeometryPosition (
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					a3.Start.X);

				a3.Start.GeometryY = GetGeometryPosition (
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight, 
					a3.Start.Y);

				a3.Middle.GeometryX = GetGeometryPosition (
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					a3.Middle.X);

				a3.Middle.GeometryY = GetGeometryPosition (
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight, 
					a3.Middle.Y);

				a3.End.GeometryX = GetGeometryPosition (
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					a3.End.X);

				a3.End.GeometryY = GetGeometryPosition (
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight, 
					a3.End.Y);
			}

			var t = p as TextElement;
			if (t != null) {
				if (t.FixedX) {
					t.Start.GeometryX =  seg.Position.GeometryXStart + t.Start.X;
				} else {
					t.Start.GeometryX =  GetGeometryPosition(
						seg.Position.GeometryXStart, 
						seg.Position.GeometryWidth, 
						t.Start.X);
				}

				if (t.FixedY) {
					t.Start.GeometryY =  seg.Position.GeometryYStart  + t.Start.Y;
				} else {
					t.Start.GeometryY =  GetGeometryPosition(
						seg.Position.GeometryYStart, 
						seg.Position.GeometryHeight,
						t.Start.Y);
				}

				if (!t.FixedSize) {
					/*t.ScaledFontSize = Math.Sqrt (
						Math.Pow (seg.Position.GeometryWidth, 2) + 
						Math.Pow (seg.Position.GeometryHeight, 2)) * 
						t.FontSize / 
						200;*/
					t.ScaledFontSize = seg.Position.GeometryHeight * t.FontSize / 100;
				} else {
					t.ScaledFontSize = t.FontSize;
				}

				try
				{
					var regFormat = Regex.Matches(t.Text, "\\{\\d+\\}");
					if(regFormat.Count > 0 || t.Text.Contains("{Function}"))
					{
						if(seg.Variables.Any() && seg.Variables.Any(s => !string.IsNullOrWhiteSpace(s.Value)))
							{
								if(seg.Variables.Count() == 1)
								{
								seg.FunctionText =seg.Type.GetRuleForElement()
										.GetFormatted(seg);									
								}
								
							var values = seg.Variables.Select(v => v.Value.GetExactlyVariableOrConst()).ToList();
							values.Add(seg.FunctionText);
							var max = 0;
							for(var i = 0; i <  regFormat.Count; i++)
							{
								var v = regFormat[i].Value.Replace("{", string.Empty).Replace("}", string.Empty);
								int res;
								if(int.TryParse(v, out res) && res > max)
								{
									max = res;
								}
							}
							if(max < values.Count())
							{
								t.FormattedText = string
									.Format (
										t.Text.Replace("{Function}", "{" +(values.Count() - 1).ToString() + "}")
										, values.ToArray());
							}
						}
						else
						{
							t.FormattedText = string.Empty;
						}
					}
					else
					{
						t.FormattedText = t.Text;
					}
				}
				catch {
					t.FormattedText = string.Empty;
				}
			}		

			var r = p as RectangleElement;
			if (r != null) {
				r.LeftBottom.GeometryX = GetGeometryPosition(
					seg.Position.GeometryXStart, 
					seg.Position.GeometryWidth, 
					r.LeftBottom.X);

				r.LeftBottom.GeometryY = GetGeometryPosition(
					seg.Position.GeometryYStart, 
					seg.Position.GeometryHeight,
					r.LeftBottom.Y);

				r.GeometryWidth = seg.Position.GeometryWidth * r.Width;
				r.GeometryHeight = seg.Position.GeometryHeight * r.Height;
			}	

			var c = p as Connector;
			if (c == null)
			{
				return;
			}
			c.Center.GeometryX = GetGeometryPosition(
				seg.Position.GeometryXStart, 
				seg.Position.GeometryWidth, 
				c.Center.X);

			c.Center.GeometryY = GetGeometryPosition(
				seg.Position.GeometryYStart, 
				seg.Position.GeometryHeight, 
				c.Center.Y > 1 ? c.Center.Y - 1 : c.Center.Y);

			c.GeometryRadius = seg.Position.GeometryWidth * c.Radius;
		}


		static double GetGeometryPosition(double start, double size, double surfaceValue)
		{
			return start + size * surfaceValue;
		}
	}
}

