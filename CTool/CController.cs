using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using LadderLogic.Reader;

namespace LadderLogic.CTool
{
	using Surface;
	using Arduino;
	using File.DrawingFile;
	using FunctionRule;
	using File;
	using Controller;
	using File.Config;

	public class CController
	{
		static CController _instance;


		public readonly List<FunctionRule.FunctionRule> Rules;


		public LocalConfig Cfg;


		CController ()
		{
			Rules = new List<FunctionRule.FunctionRule>{
				new CoilRule(),
				new LatchRule(),
				new NcContactRule(),
				new NoContactRule(),
				new NotCoilRule(),
				new OffTimerRule(),
				new OnTimerRule(),
				new CycleTimerRule(),
				new PulseTimerRule(),
				new SetCoilRule(),
				new ResetCoilRule()
			};

			Cfg = AppController.Instance.Config;
		}


		public static CController Instance 
		{
			get 
			{
				return _instance = _instance ?? new CController ();
			}
		}


		public void CreateCode(object sender, EventArgs args)
		{
			var templateFile = Path.Combine ((@"CTool"+  Path.DirectorySeparatorChar), "CodeTemplate.c").GetAbsolutePath ();
			string template = System.IO.File.ReadAllText (templateFile);

			string variables = string
				.Join(
					Environment.NewLine,
					AppController
					.Instance
					.GetUserVariablesWithDefault()
					.Select(v => string.Format("unsigned long {0};", v)));
			
			template = template.Replace("#{UserVariables}", variables);

			var workflow = new StringBuilder ();
			// function sequence alhorithm.

			var surface = AppController.Instance.Surface;

			var segments = surface.Get ();

			var queue = new Stack<Segment> ();
			string comment;
			queue.Push(segments
				.FirstOrDefault (s => {
					var rule = s.Type.GetRuleForElement();
					if(rule != null)
					{
						var func = rule.Resolve(s, out comment); 
						return func == FunctionType.In || 
							func == FunctionType.InAnalog || 
							func == FunctionType.InNot || 
							func == FunctionType.TimerOff ||
							func == FunctionType.TimerOn ||
							func == FunctionType.TimerCycle ||
							func == FunctionType.TimerPulse ||
							func == FunctionType.Latch ||
							func == FunctionType.LatchKey;
					}
					return false;
				}));

			var parsed = new List<Segment> ();

			while (queue.Any()) {
				var current = queue.Pop ();

				if (current == null) {
					continue;
				}

				//do not use already parsed elements in workflow.
				if (parsed.Any (p => p.Identifier == current.Identifier)) {
					continue;
				}

				// add connected segments to queue.
				var siblings = current
					.Connectors
					.SelectMany(con => con
						.ConnectedTo
						.ToList ()
						.Select (id => segments.FirstOrDefault(s => s.Identifier.ToString() == id)))
					.Where(s => s != null && 
						parsed.All(p => p.Identifier != s.Identifier) &&
						queue.All(p => p.Identifier != s.Identifier))
					.ToList(); // exclude already parsed segments.

				var childQueue = new Stack<Segment> ();

				siblings
					.Where (s => s.Position.X <= current.Position.X)
					.ToList()
					.ForEach(childQueue.Push);

				siblings
					.Where (s => s.Type == ElementType.Latch)
					.ToList ()
					.ForEach (s2 => {
						if (s2.Join.Any()) { // add connected segments to joined segment in latch
							s2.Join.ForEach (c1 => {
								var s1 = c1.Connectors
									.SelectMany (con => con
										.ConnectedTo
										.Select (id => segments.FirstOrDefault (s => s.Identifier.ToString () == id)))
									.Where (s => s != null &&
										s.Position.X < s2.Position.X &&
										parsed.All(p => p.Identifier != s.Identifier) &&
										queue.All(p => p.Identifier != s.Identifier))
									.ToList (); // exclude already parsed segments.

								var latchQueue = new Stack<Segment>();
								s1.ForEach(latchQueue.Push);
								var latchParsed = new List<Segment> ();

								latchParsed.Add(c1); // do not use joined element in child loop
								PushChild(latchQueue, 
									segments, 
									parsed, 
									latchParsed,
									queue,
									current);

								latchParsed.Reverse();
								latchParsed
									.Where(s3 => s3.Identifier != c1.Identifier)
									.ToList()
									.ForEach(childQueue.Push);

								parsed.Add(c1); // joined element changed to persed state.
							});
						}
				});



				var childParsed = new List<Segment> ();

				PushChild(childQueue, 
					segments, 
					parsed, 
					childParsed,
					queue,
					current);

				// add sequent elements to queue
				siblings
					.Where (s => s.Position.X > current.Position.X &&
						s.Position.Y == current.Position.Y)
					.ToList()
					.ForEach(queue.Push);

				// add paralel element to quieie
				childParsed.ForEach(queue.Push); // add last elements first.

				parsed.Add (current);

				//find next ladding layer.
				if (!queue.Any()) {

					var maxY = parsed.Max (p => p.Position.Y);

					queue.Push(segments.Where(s => s.Position.Y > maxY)
						.FirstOrDefault (s => {
							var rule = s.Type.GetRuleForElement();
							if(rule != null)
							{
								var func = rule.Resolve(s, out comment); 
								return func == FunctionType.In || 
									func == FunctionType.InAnalog || 
									func == FunctionType.InNot ||
									func == FunctionType.TimerOff ||
									func == FunctionType.TimerOn ||
									func == FunctionType.TimerCycle ||
									func == FunctionType.TimerPulse ||
									func == FunctionType.Latch ||
									func == FunctionType.LatchKey;
							}
							return false;
						}));
				}

				//append new element to workflow.
				if (current.Type != ElementType.None) {
					workflow.Append ("\t");
					var command = current.Type.GetRuleForElement ().GetFormatted (current);
					if (!string.IsNullOrWhiteSpace (command)) {
						workflow.Append (command);
						if (queue.Any()) {
							workflow.Append (Environment.NewLine);
						}
						if (queue.Any() &&
						   current.Type == ElementType.Coil ||
						   current.Type == ElementType.NotCoil ||
						   current.Type == ElementType.SetCoil ||
						   current.Type == ElementType.ResetCoil) {
							workflow.Append (Environment.NewLine);
						}
					}
				}
			}


			template = template.Replace ("#{Workflow}", workflow.ToString());

			// todo: use texteditor api.
			//SyntaxModeService.LoadStylesAndModes ("G:\\Disk_H\\Projects1\\ladderlogic\\LadderLogic\\Libs\\Styles");
			//var t = new TextEditor (new TextDocument (template), new TextEditorOptions());
			//t.Show ();

			//private TextEditor edytor;

			//public Window2 () :
			//base (Gtk.WindowType.Toplevel)
			//{
			//	this.Build ();

			//	edytor = new TextEditor ();

			//	vbox1.Add (edytor);
			//	edytor.ShowAll ();
			//	edytor.Text = GenerujText ();

			//	edytor.Options.EnableSyntaxHighlighting = true;
			//	edytor.Options.HighlightCaretLine = true;
			//	edytor.Options.HighlightMatchingBracket = true;

			//	WlaczTrybAdam ();
			//}

			//void WlaczTrybAdam()
			//{
			//	var typMime = "text/adam";
			//	edytor.Document.MimeType = typMime;
			//}
			//TextEditor CreateTextEditor()
			//{
			//	TextEditor editor = new TextEditor ();
			//	TextEditorOptions options = new TextEditorOptions ();
			//	options.EnableSyntaxHighlighting = true;
			//	options.ShowFoldMargin = true;
			//	options.ShowWhitespaces = ShowWhitespaces.Selection;
			//	options.DrawIndentationMarkers = true;
			//	options.EnableAnimations = true;
				//options.ColorScheme = "Visual Studio";
			//	editor.Options = options;
			//	editor.Text = System.IO.File.ReadAllText
			//		("/home/pablo/workspace/TestTextEditor/TestTextEditor/MainWindow.cs");
			//	editor.Document.MimeType = "text/x-csharp";
			//	return editor;
			//}


			//show only code with workflow.
			if (workflow.Length > 0) {
				MainClass._main.OpenSourceDialog (template);
			}
		}


		private void PushChild(Stack<Segment> childQueue, 
			IEnumerable<Segment> segments, 
			List<Segment> parsed, 
			List<Segment> childParsed,
			Stack<Segment> queue,
			Segment current)
		{
			while (childQueue.Any()) {
				var child = childQueue.Pop ();

				var childSiblings = child
					.Connectors
					.SelectMany(con => con
						.ConnectedTo
						.ToList ()
						.Select (id => segments.FirstOrDefault(s => s.Identifier.ToString() == id)))
					.Where(s => s!= null && 
						parsed.All(p => p.Identifier != s.Identifier) &&
						childParsed.All(p => p.Identifier != s.Identifier) &&
						queue.All(p => p.Identifier != s.Identifier) &&
						child.Position.X < s.Position.X &&
						current.Identifier != s.Identifier
					).ToList();

				if (child.Join.Any()) { // add connected segments to joined segment in latch
					child.Join.ForEach (c1 => {
						var s1 = c1.Connectors
							.SelectMany (con => con
								.ConnectedTo
								.ToList ()
								.Select (id => segments.FirstOrDefault (s => s.Identifier.ToString () == id)))
							.Where (s => s != null &&
								parsed.All(p => p.Identifier != s.Identifier) &&
								queue.All(p => p.Identifier != s.Identifier))
							.ToList (); // exclude already parsed segments.
						var latchQueue = new Stack<Segment>();
						s1.ForEach(latchQueue.Push);
						var latchParsed = new List<Segment> ();
						PushChild(latchQueue, 
							segments, 
							parsed, 
							latchParsed,
							queue,
							current);

						latchParsed.ForEach(childQueue.Push);
					});
				}

				childSiblings.ForEach (childQueue.Push);
				childParsed.Add (child);
			}
		}

		public void UploadCode(object sender, EventArgs args)
		{
		}


		public void SetEnvironmentVariables(EnvironmentVariables env )
		{
			ConfigManager.Write<EnvironmentVariables> (env, "Environment.xml".GetAbsolutePath ());
		}

		public EnvironmentVariables GetEnvironmentVariables()
		{
			try
			{
			return ConfigManager.Read<EnvironmentVariables> ("Environment.xml".GetAbsolutePath ());
			}
			catch {
				return new EnvironmentVariables {
					ArduinoPath = @"C:\Program Files (x86)\Arduino",
					ArduinoBoard = "arduino:avr:uno",
					ArduinoPort = "COM1"
				};
			}
		}


		public string Upload(string code, string fileName)
		{
			var env = GetEnvironmentVariables ();

			var p = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					FileName = Path.Combine(env.ArduinoPath, "arduino"),
					WorkingDirectory = env.ArduinoPath
				}
			};

			if (string.IsNullOrEmpty (fileName)) {
				Directory.CreateDirectory ("programm".GetAbsolutePath ());
				fileName = (@"programm" +  Path.DirectorySeparatorChar + "programm.ino").GetAbsolutePath();
				System.IO.File.WriteAllText (fileName, code);
			}

			p.StartInfo.Arguments = string.Format("--board {0} --port {1} {2} \"{3}\"",
				env.ArduinoBoard,
				env.ArduinoPort,
				AppController.Instance.Config.ArduinoParameters,
				fileName);

			p.Start();
			var r = p.StandardError.ReadToEnd ();
			r = r + Environment.NewLine;
			r = r + p.StandardOutput.ReadToEnd ();
			p.WaitForExit ();

			string output;
			if (p.ExitCode != 0)
			{
				switch (p.ExitCode) {
				case 1:
					output = "ERROR: Build failed or upload failed. Check your board configuration and your serial port.";
					break;
				case 2:
					output = "ERROR: Sketch not found";
					break;
				case 3:
					output = "ERROR: Invalid argument for command line option";
					break;
				case 4:
					output = "ERROR: Preference passed to --get-pref does not exist";
					break;
				default:
					output = "ERROR: Unknown error.";
					break;
				}
			}
			else output = string.Format("SUCCESS: code successfully uploaded to board {0} on port {1}",env.ArduinoBoard,env.ArduinoPort);

			return output;		
		}
	}
}

