using System.IO;
using System.Reflection;
using Gtk;

namespace LadderLogic.Presentation
{
	using Controller;
	using Drawing;

	public class DrawingAreaWindow: Window
	{
		public DrawingAreaWindow () : base ("Drawings view")
		{
			InitializeComponents ();
		}


		/// <summary>
		/// Gantt.
		/// </summary>
		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		VBox vbox1;
// ReSharper restore InconsistentNaming


		void InitializeComponents()
		{
			Stream stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(AppController.Instance.Config.DrawingAreaWindow);

			var glade = new Glade.XML(stream, AppController.Instance.Config.DrawingAreaWindowName, null);
			if (stream != null)
			{
				stream.Close();
			}
			glade.Autoconnect(this);

			//todo: add icon
			//this.IconList = new Pixbuf[]{ new Pixbuf(System.Reflection.Assembly.GetExecutingAssembly(), "LadderLogic.bmp")};			
			//this.Icon = this.IconList[0];

			var drw = new ElementDrawing (DrawingAreaController.Instance.Surface, 2, 2);
			vbox1.Add(drw);
			drw.Show ();
		}


// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
		void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local
		{
			a.RetVal = true;
			Application.Quit ();
		}
	}
}

