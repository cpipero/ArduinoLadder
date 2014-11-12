using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace LadderLogic.Surface
{
	using Controller;
	using CTool;
	using File.DrawingFile;
	using File.Config;

	[Serializable]
	public class Segment
	{
		List<Drawable> _primitives = new List<Drawable>();


		readonly LocalConfig _cfg = AppController.Instance.Config;


		bool _selected;


		ElementType _type;


		public Segment ()
		{
			Join = new List<Segment> ();
			Connectors = new List<Connector> ();
			Variables = new List<Variable> ();
			Values = new List<string> ();
		}


		public Segment (Position position, PrimitivesSurface surface) : this()
		{
			Position = position;
			Surface = surface;
			Identifier = Guid.NewGuid ();
		}

		/// <summary>
		/// Gets or sets the join for Latch.
		/// </summary>
		/// <value>The join.</value>
		[XmlIgnore]
		public List<Segment> Join{ get; set; }


		[XmlIgnore]
		public bool IsPalette { get; set; }


		public FunctionType Function { get; set; }


		public bool OverrideFunction { get; set; }


		public string FunctionText { get; set; }


		public List<Variable> Variables { get; set; }


		public List<string> Values { get; set; }


		public Segment (Position position, List<Drawable> primitives, PrimitivesSurface surface) : this(position, surface)
		{
			_primitives.AddRange(primitives.Where(p => !(p is Connector)));
			Connectors.AddRange(primitives.Where(p => p is Connector).Cast<Connector>());
		}


		public Position Position { get;	set; }

		[XmlIgnore]
		public bool Selected {
			get
			{ 
				return _selected; 
			} 
			set
			{ 
				_selected = value;
				var markers = _primitives.Where (p => p.Marker == _cfg.Selected).ToList ();
				if (_selected) {
					markers.ForEach (p => p.Alpha = _cfg.SelectionOpacity);

					//select latch
					if (Join.Any ()) {
						foreach (var seg2 in Join.Where(seg3 => !seg3.Selected)) {
							seg2.Selected = _selected;
						}
					}
				} else {
					markers.ForEach (p => p.Alpha = 0);

					//select latch
					if (Join.Any ()) {
						foreach (var seg2 in Join.Where(seg3 => seg3.Selected)) {
							seg2.Selected = _selected;
						}
					}
				}
			} 
		}


		[XmlIgnore]
		public PrimitivesSurface Surface { get; set; }


		[XmlIgnore]
		public List<Drawable> Primitives 
		{
			get 
			{ 
				return _primitives.OrderBy (p => p.ZIndex).ToList();
			}

			set {
				_primitives = value;
			}
		}


		public List<Connector> Connectors {	get; set; }


		public ElementType Type 
		{ 
			get
			{ 
				if ( _type == ElementType.None && Join.Any ())
				{
					var parent = Join.FirstOrDefault ();
					if (parent != null) return parent.Type;
				}
				return _type; 
			} 

			set
			{
				_type = value; 
			} 
		}


		public Guid Identifier { get; set; }


		public void AddPrimitives (IEnumerable<Drawable> primitives)
		{
			var enumerable = primitives as Drawable[] ?? primitives.ToArray();

			_primitives.AddRange (enumerable.Where(p => !(p is Connector)));
			Connectors.AddRange(enumerable.Where(p => p is Connector).Cast<Connector>().Where(c => c.Center.Y < 1).ToList());

			var j = Join.FirstOrDefault (s => s.Position.X == Position.X && s.Position.Y == Position.Y + 1);

			if (j != null) {
				j.Connectors.AddRange(enumerable.Where(p => p is Connector).Cast<Connector>().Where(c => c.Center.Y >= 1).ToList());
			}
		}


		public void RemoveElement(bool join = false)
		{
			_primitives.RemoveAll (obj => obj.Container != null);
			if (Connectors.RemoveAll (obj => true) > 0) {
				DisconnectSiblings ();
				RemovePowerLines ();
			}

			if (Join.Any ()) {
				if (!join) {
					foreach (var j in Join) {
						j.RemoveElement (true);
					}
				}
				Join.Clear ();
			}

			Variables.Clear ();
			OverrideFunction = false;

			Type = ElementType.None;
		}


		public Segment ConnectPower()
		{
			if (Primitives.Any (p => p.Marker == _cfg.LeftPower)) {
				var leftC = Connectors
					.FirstOrDefault(p => p.Marker == _cfg.LeftConnector );
				if(leftC != null)
				{
					leftC.ConnectedTo.Add(_cfg.LeftPower);
				}
			}

			if (Primitives.Any (p => p.Marker == _cfg.RightPower)) {
				var leftC = Connectors
					.FirstOrDefault(p => p.Marker == _cfg.RightConnector );
				if(leftC != null)
				{
					leftC.ConnectedTo.Add(_cfg.RightPower);
				}
			}

			return this;
		}


		public Segment ConnectSiblings()
		{
			//todo use one func.
			var left = Surface.Get ().FirstOrDefault (s => s.Position.X == (Position.X - 1) 
				&& s.Position.Y == Position.Y);

			var right = Surface.Get ().FirstOrDefault (s => s.Position.X == (Position.X + 1) 
				&& s.Position.Y == Position.Y);

			if (left != null && left.Type != ElementType.None) {

				var l = (Connectors
					.FirstOrDefault (p => p.Marker == _cfg.LeftConnector) ?? Connectors
						.FirstOrDefault (p => p.Marker == _cfg.ConR)) ?? Connectors
							.FirstOrDefault (p => p.Marker == _cfg.ConS);

				var leftR = left
					.Connectors
					.FirstOrDefault (p => p.Marker == _cfg.RightConnector) ?? left
						.Connectors
						.FirstOrDefault (p => p.Marker == _cfg.ConQ);

				if (leftR != null)
					{
						leftR.ConnectedTo.Add(Identifier.ToString ());
					}
					if (l != null) {
					l.ConnectedTo.Add(left.Identifier.ToString ());
				}
			}

			if (right != null && right.Type != ElementType.None) {

				var r = Connectors
					.FirstOrDefault (p => p.Marker == _cfg.RightConnector) ?? Connectors
						.FirstOrDefault (p => p.Marker == _cfg.ConQ);

				var rightL = (right
					.Connectors
					.FirstOrDefault (p => p.Marker == _cfg.LeftConnector) ?? right
						.Connectors
						.FirstOrDefault (p => p.Marker == _cfg.ConR)) ?? right
							.Connectors
							.FirstOrDefault (p => p.Marker == _cfg.ConS);

				if (rightL != null)
					{
						rightL.ConnectedTo.Add(Identifier.ToString ());
					}
					if (r != null)
					{
						r.ConnectedTo.Add(right.Identifier.ToString ());
					}
			}

			return this;
		}


		public Segment DisconnectSiblings()
		{
			//todo use one func.
			var left = Surface.Get ().FirstOrDefault (s => s.Position.X == (Position.X - 1) 
				&& s.Position.Y == Position.Y);

			var right = Surface.Get ().FirstOrDefault (s => s.Position.X == (Position.X + 1) 
				&& s.Position.Y == Position.Y);

			if (left != null && left.Type != ElementType.None 
				&& left.Connectors.Any (p => p.Marker == _cfg.RightConnector)) {

				var leftR = left
					.Connectors
					.FirstOrDefault (p => p.Marker == _cfg.RightConnector);

					if (leftR != null)
					{
						leftR.ConnectedTo.Remove(Identifier.ToString());
					}
				}

			if (right != null && right.Type != ElementType.None 
				&& right.Connectors.Any (p => p.Marker == _cfg.LeftConnector)) {

				var rightL = right
					.Connectors
					.FirstOrDefault (p => p.Marker == _cfg.LeftConnector);

					if (rightL != null)
					{
						rightL.ConnectedTo.Remove(Identifier.ToString());
					}
				}

			return this;
		}


		public void RemovePowerLines()
		{
			var lines = AppController.Instance.Surface.Lines ().ToList();
			foreach (var line in lines) {
				if(line.Input == Position)
				{
					Surface.RemovePowerLine (line);
				}

				if(line.Output == Position)
				{
					Surface.RemovePowerLine (line);
				}
			}
		}
	}
}

