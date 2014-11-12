using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace LadderLogic.Surface
{
	using File.DrawingFile;
	using Controller;

	[Serializable]
	public class PrimitivesSurface
	{
		public List<Segment> Segments{ get; set; }


		public List<Line> PowerLines { get; set; }


		public PrimitivesSurface ()
		{		
			Segments = new List<Segment>();
			PowerLines = new List<Line>();
		}



		public uint Width { get; set; }


		public uint Height { get; set; }


		[XmlIgnore]
		public Action QueueDraw { get; set; }


		[XmlIgnore]
		public bool IsPalette { get;set; }


		[XmlIgnore]
		public bool IsButton { get;set; }


		public string Tooltip { get; set; }


		public Segment Add (DrawingElement el, Position p)
		{
			Remove(p);

			var seg = Segments.FirstOrDefault (s => s.Position == p);
			if (seg != null) {
				seg.AddPrimitives (el.Primitives);
			} else {
				seg = new Segment (p, el.Primitives, this) { IsPalette = IsPalette };
				Segments.Add (seg);
			}
			seg.Type = el.Type;
			CalculateSize ();


			//join latch

			if(el.Primitives.Any(pr => pr is LineElement && ((pr as LineElement).End.Y > 1)))
			{
				var pos2 = new Position (seg.Position.X, seg.Position.Y + 1);
				var seg2 = Segments.FirstOrDefault (s => s.Position == pos2);
				if (seg2 != null)
				{
					seg2.Join.Add (seg);
					seg.Join.Add (seg2);

					var conR = el
						.Primitives
						.Where (p1 => p1 is Connector)
						.Cast<Connector> ()
						.Where (c => c.Center.Y > 1);

					seg2.Connectors.AddRange (conR);

					if (seg.Selected) {
						seg2.Selected = true;
					}
				}
			}

			return seg;
		}


		public Segment Add (Position p)
		{
			var seg = Segments.FirstOrDefault (s => s.Position == p);
			if (seg == null) {
				seg = new Segment (p, this) { IsPalette = IsPalette };
				Segments.Add (seg);
				CalculateSize ();
			}

			return seg;
		}


		void CalculateSize ()
		{
			Width = 0;
			Height = 0;

			foreach (var seg in Segments) {
				if (Width < seg.Position.X) {
					Width = seg.Position.X;
				}

				if (Height < seg.Position.Y) {
					Height = seg.Position.Y;
				}
			}
		}


		public bool Remove(Position p)
		{
			if (Segments.Any (s => s.Position == p)) {

				Segments.First (s => s.Position == p).RemoveElement();
				return true;
			}

			return false;
		}


		public IEnumerable<Segment> Get()
		{
			return Segments.OrderBy(s => s.Position.X + 
				s.Position.Y * 1000); // simple order lineby line.
		}


		public IEnumerable<Line> Lines()
		{
			return PowerLines;
		}


		public Segment ClearSelection()
		{
			Segment selected = null;
			Segments.ForEach (seg => 
				{
					if(seg.Selected)
					{
						selected = seg;
					}
					seg.Selected = false;
				});
			if (QueueDraw != null) {
				Gtk.Application.Invoke (delegate {
					QueueDraw();
				});
			}

			return selected;
		}


		public void AddPowerLine(Line line)
		{
			//todo use one func
			if (PowerLines.Any(l => l.Input == line.Input &&
			                        l.Output == line.Output &&
			                        l.InputMarker == line.InputMarker &&
			                        l.OutputMarker == line.OutputMarker)) 
				return;

			var s1 = Segments.FirstOrDefault (s => s.Position == line.Input);
			var s2 = Segments.FirstOrDefault (s => s.Position == line.Output);

			if (s1 != null)
			{
				var m1 = s1.Connectors.FirstOrDefault (p => p.Marker == line.InputMarker);
				if (s2 != null)
				{
					var m2 = s2.Connectors.FirstOrDefault (p => p.Marker == line.OutputMarker);

					if (m1 != null && m2 != null) {
						m1.ConnectedTo.Add(s2.Identifier.ToString());
						m2.ConnectedTo.Add(s1.Identifier.ToString());
						PowerLines.Add (line);
					}
				}
			}
		}


		public void RemovePowerLine(Line line)
		{
			//todo use one func

			var found = PowerLines.FirstOrDefault (l => l.Input == line.Input &&
			l.Output == line.Output &&
			l.InputMarker == line.InputMarker &&
			l.OutputMarker == line.OutputMarker);
			if (found == null)
			{
				return;
			}

			var s1 = Segments.FirstOrDefault (s => s.Position == line.Input);
			var s2 = Segments.FirstOrDefault (s => s.Position == line.Output);

			if (s1 != null && s2 != null)
			{
				var toRemove = new List<Connector> ();
				s1.Connectors.Where(p => p.Marker == line.InputMarker)
					.ToList().ForEach(m => {
						m.ConnectedTo.Remove(s2.Identifier.ToString());
						if(m.Marker == AppController.Instance.Config.LeftPower ||
							m.Marker == AppController.Instance.Config.RightPower)
						{
							toRemove.Add(m);
						}
					});

				toRemove.ForEach (c => s1.Connectors.Remove(c));
				toRemove.Clear ();

				s2.Connectors.Where(p => p.Marker == line.OutputMarker)
					.ToList().ForEach(m => 
						{
							m.ConnectedTo.Remove(s1.Identifier.ToString());
							if(m.Marker == AppController.Instance.Config.LeftPower ||
								m.Marker == AppController.Instance.Config.RightPower)
							{
								toRemove.Add(m);
							}
					});

				toRemove.ForEach (c => s2.Connectors.Remove(c));

			}

			PowerLines.Remove (found);
		}


		public bool IsEmpty()
		{
			return Segments.All (s => s.Type == ElementType.None) && PowerLines.Count () == 0;
		}	
	}
}

