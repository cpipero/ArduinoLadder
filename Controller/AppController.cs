using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using LadderLogic.CTool.FunctionRule;

namespace LadderLogic.Controller
{
	using State;
	using File;
	using File.Config;
	using File.DrawingFile;
	using Reader;
	using Surface;
	using CTool;

	public class AppController
	{
		static AppController _instance;


		public LocalConfig Config;


		readonly List<PrimitivesSurface> _palette = new List<PrimitivesSurface> ();


		readonly List<PrimitivesSurface> _arduino = new List<PrimitivesSurface> ();


		State.State _currentState;


		uint _contactsCount;


		uint _rowsCount;


		AppController ()
		{
			CurrentState = new InitState ();
		}


		public static AppController Instance 
		{
			get 
			{
				return _instance = _instance ?? new AppController ();
			}
		}


		public PrimitivesSurface Surface { get; set; }


		public Segment PrevSegment { get; set; } 


		public Segment NewSegment { get; set ; }


		public string FileName { get; set; }


		public State.State CurrentState 
		{
			get
			{
				return _currentState; 
			} 

			set
			{ 
				var previousState = _currentState;
				if (previousState != null) {
					previousState.Leave ();
				}

				_currentState = value;
				var lineState = _currentState as LineState;

				if (_currentState.Handle(previousState, PrevSegment, NewSegment, lineState != null && lineState.Left))
				{
					return;
				}

				if (previousState != null) {
					//throw new ArgumentException (string.Format("Previous {0} New {1}",
					//	Enum.GetName (typeof(StateType), previousState.Type),
					//	Enum.GetName (typeof(StateType), _currentState.Type)));
				}
			} 
		}


		public void SetCurrentState(bool left)
		{
			var p = _palette.FirstOrDefault(surf => surf.Get().Any(seg => seg.Selected));
			if (p != null) {
				var pType = p.Get ().First ().Type;
				switch (pType) {
				case ElementType.Line:
					CurrentState = new LineState {Left = left};
					break;
				case ElementType.Cursor:
					CurrentState = new CursorState ();
					break;
				case ElementType.Properties:
					CurrentState = new PropertiesState ();
					break;
				case ElementType.None:
					break;
				default:
					CurrentState = new PaletteState ();
					break;
				}
			} else {

				var pos = Surface.Get ().FirstOrDefault (seg => seg.Selected);
 				
				var lineState = CurrentState as LineState;
				if ( lineState != null && (lineState.LeftSegment == null || lineState.RightSegment == null)) {
					lineState.Handle (CurrentState, PrevSegment, NewSegment, left);
					return;
				}

				if (pos != null) {
					var posType = pos.Type;
					switch (posType) {
					case ElementType.None:
						CurrentState = new EmptyState ();
						break;
					case ElementType.Coil:
					case ElementType.Latch:
					case ElementType.NcContact:
					case ElementType.NoContact:
					case ElementType.NotCoil:
					case ElementType.OffTimer:
					case ElementType.OnTimer:
					case ElementType.PulseTimer:
					case ElementType.CycleTimer:
					case ElementType.SetCoil:
					case ElementType.ResetCoil:
						CurrentState = new ElementState ();
						break;
					case ElementType.Line:
						CurrentState = new LineState ();
						break;
					default:
						throw new ArgumentException (Enum.GetName (typeof(ElementType), posType));
					}
				}
			}
		}


		public void DeleteCurrentElement()
		{
			Surface.Remove (NewSegment.Position);
			Surface.QueueDraw ();
		}


		public List<PrimitivesSurface> GetPalette()
		{
			return _palette.OrderBy(p => {
				var s = p.Get().FirstOrDefault();
				if(s != null)
				{
					var pr = s.Primitives.FirstOrDefault(p1 => p1.Container != null);
					if(pr!= null)
					{
						return pr.Container.Order;
					}
				}

				return 0;
			}).ToList();
		}


		public List<PrimitivesSurface> GetArduino()
		{
			return _arduino;
		}


		public Segment ClearSelection()
		{
			Segment selected = null;
			_palette.ForEach (s => 
				{
					var el1 = s.ClearSelection ();
					if(el1 != null)
					{
						selected = el1;
					}
				});
			var el = Surface.ClearSelection ();
			if (el != null) {
				selected = el;
			}
			return selected;
		}


		public void Initialize()
		{
			InitConfig ();

			InitPalette ();
			InitArduino ();
			InitSurface ();
		}


		public void InitConfig()
		{
			var fileName = ("File" + Path.DirectorySeparatorChar + "Config" //FIXME Move path to appSettings
			                  + Path.DirectorySeparatorChar + "ladderlogic.conf.xml").GetAbsolutePath ();
			/*config = new LocalConfig ();
			var result = ConfigManager.Write (config, fileName);
			if (!result) {
				throw new Exception ("write error for config");
			}*/
			Config = ConfigManager.Read<LocalConfig>(fileName);
		}


		void InitPalette()
		{
			_palette.Clear ();
			/*
			var elementNames = new [] { 
				"NcContact.xml", 
				"NoContact.xml", 
				"Coil.xml", 
				"NotCoil.xml", 
				"OnTimer.xml", 
				"OffTimer.xml", 
				"CycleTimer.xml",
				"SetCoil.xml",
				"ResetCoil.xml",
				"Line.xml", 
				"Properties.xml",
				"Cursor.xml",
				"Latch.xml"};
				*/

			var elementNames = Directory.GetFiles ((@"Element"+  Path.DirectorySeparatorChar).GetAbsolutePath ());

			foreach (var el in elementNames
				.Select(name => 
					!string.IsNullOrEmpty(name) ? 
					ConfigManager.Read<DrawingElement> (name)
					: new DrawingElement{ Type = ElementType.None, Primitives = new List<Drawable>()}))
			{
				el.SetupContainer ();
				var surface = new PrimitivesSurface () {IsPalette = true};
				if (el.Type == ElementType.Latch) {
					surface.Add (new Position{ X = 0, Y = 0 });
					surface.Add (new Position{ X = 0, Y = 1 });
					surface.Add (el, new Position{ X = 0, Y = 0 });
				} else {
					surface.Add (el, new Position{ X = 0, Y = 0 });
				}
				surface.Tooltip = el.Tooltip;
				_palette.Add (surface);
			}
		}


		void InitArduino()
		{
			_arduino.Clear ();


			var elementNames = Directory.GetFiles ((@"Buttons"+  Path.DirectorySeparatorChar).GetAbsolutePath ());

			foreach (var el in elementNames
				.Select(name => 
					!string.IsNullOrEmpty(name) ? 
					ConfigManager.Read<DrawingElement> (name)
					: new DrawingElement{ Type = ElementType.None, Primitives = new List<Drawable>()}))
			{
				el.SetupContainer ();
				var surface = new PrimitivesSurface () { IsButton = true};
				surface.Add (el, new Position{ X = 0, Y = 0 });
				surface.Tooltip = el.Tooltip;
				_arduino.Add (surface);
			}
		}


		public void InitSurface ()
		{
			Surface = new PrimitivesSurface ();

			_contactsCount = _instance.Config.ContactsCount;
			_rowsCount = _instance.Config.RowsCount;

			for (uint j = 0; j < _rowsCount; j++) {
				for (uint i = 0; i < _contactsCount + 1; i++) {
					Surface.Add (new Position{ X = i, Y = j });
				}
			}
		}


		public string SelectLeftConnector(Segment selected)
		{
			if (selected.Connectors.Any (c => c.Marker == _instance.Config.LeftConnector)) {
				SelectConnector (selected, _instance.Config.LeftConnector);
				return _instance.Config.LeftConnector;
			}

			if (selected.Connectors.Any (c => c.Marker == _instance.Config.ConS)) {
				SelectConnector (selected, _instance.Config.ConS);
				return _instance.Config.ConS;
			}

			if (selected.Connectors.Any (c => c.Marker == _instance.Config.ConR)) {
				SelectConnector (selected, _instance.Config.ConR);
				return _instance.Config.ConR;
			}

			return _instance.Config.LeftConnector;
		}


		public string SelectRightConnector(Segment selected)
		{
			if (selected.Connectors.Any (c => c.Marker == _instance.Config.RightConnector)) {
				SelectConnector(selected, _instance.Config.RightConnector);
				return _instance.Config.RightConnector;
			}
			if (selected.Connectors.Any (c => c.Marker == _instance.Config.ConQ)) {
				SelectConnector(selected, _instance.Config.ConQ);
				return _instance.Config.ConQ;
			}

			return _instance.Config.RightConnector;
		}


		public void ClearConnectorSelection()
		{
			Surface
				.Get ()
				.ToList ()
				.ForEach (seg => seg.Connectors
					.Where (p => p.Selected)
					.ToList ()
					.ForEach (c => c.Selected = false));
		}


		static void SelectConnector(Segment selected, string marker)
		{
			var c = selected.Connectors.FirstOrDefault (p => p.Marker == marker);
			if (c != null) {
				c.Selected = true;
			}
		}


		public void ReloadSurface (PrimitivesSurface saved)
		{
			foreach (var seg in Surface.Segments) {
				var savedSeg = saved.Segments.FirstOrDefault (s => s.Position == seg.Position);
				if (savedSeg == null || seg.Join.Any())
				{
					continue;
				}

				if (savedSeg.Type != ElementType.None) {
					var el = _palette.First (s => s.Get ()
					.First ().Type == savedSeg.Type)
					.Get ()
					.First ()
					.Primitives
					.First (p => p.Container != null)
					.Container;
					var restored = Surface.Add ((DrawingElement)el.Clone (), seg.Position)
					.ConnectPower ()
					.ConnectSiblings ();
				} else {
					seg.Connectors.AddRange (savedSeg.Connectors);
				}
			}

			foreach (var line in saved.PowerLines) {
				Surface.AddPowerLine (line);
			}

			foreach (var seg in Surface.Segments) {
				var savedSeg = saved.Segments.FirstOrDefault (s => s.Type != ElementType.None &&
					s.Position == seg.Position);

				if (savedSeg == null)
				{
					continue;
				}

				seg.Variables.AddRange(savedSeg.Variables);
				seg.OverrideFunction = savedSeg.OverrideFunction;
				seg.Function = savedSeg.Function;
				seg.FunctionText =seg.Type.GetRuleForElement()
					.GetFormatted(seg);			
				seg.ConnectPower ().ConnectSiblings ();
			}
		}


		public void ResetSurface()
		{
			foreach(var seg in Surface.Segments)
			{
				seg.RemoveElement();
				seg.Variables.Clear ();
			}

			foreach(var line in Surface.Lines().ToList())
			{
				Surface.RemovePowerLine(line);
			}
		}


		public string [] GetPlcInputs()
		{
			return Config.PlcInputs.Split (';');
		}


		public string [] GetPlcOutputs()
		{
			return Config.PlcOutputs.Split (';');
		}



		public string [] GetUserVariables()
		{
			return Surface
				.Segments
				.SelectMany(s => s.Variables)
				.Where (v => v.Type == VariableType.New)
				.Select (v => v.Value.GetExactlyVariable())
				.Where(v => !string.IsNullOrEmpty(v))
				.ToArray();
		}


		public string [] GetUserVariablesWithDefault()
		{
			return Surface
				.Segments
				.SelectMany(s => s.Variables)
				.Where (v => v.Type == VariableType.New && v.Value.HasVaraiable())
				.Select(v => v.Value)
				.ToArray();
		}


		public void CheckUserVariables()
		{
			var userVariables = GetUserVariables();
			foreach (var seg in Surface.Segments) {
				seg.Variables = seg
					.Variables
					.Where (v => v.Type != VariableType.User || userVariables.Contains (v.Value))
					.ToList();
			}
		}


		public void DisableProperties()
		{
			MainClass._main.DisableProperties ();
		}


		public void EnableProperties(Segment seg)
		{
			MainClass._main.EnableProperties (seg);
		}


		public void SetElementVariable(List<Variable> vars)
		{
			var state = CurrentState as PropertiesState;
			if (state != null) {
				state.Element.Variables.Clear ();
				state.Element.Variables.AddRange (vars); // Regex.Match (variable, "[a-zA-Z_][a-zA-Z0-9]+").Length > 0			

				state.Element.FunctionText = state.Element.Type.GetRuleForElement ()
						.GetFormatted (state.Element);
					Surface.QueueDraw ();
				}
		}
	}
}

