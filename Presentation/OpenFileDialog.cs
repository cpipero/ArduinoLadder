using System;
using System.Threading;
using System.IO;
using Gtk;

namespace LadderLogic
{	
	public class OpenFileDialog
	{
		Gtk.Dialog thisDialog;


		public OpenFileDialog(Window parent)
		{
			Initialize(parent);
		}


		void Initialize(Window parent)
		{
			Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("LadderLogic.Presentation.OpenFileDialog.glade");
			Glade.XML glade = new Glade.XML(stream, "OpenFileDialog", null);
			stream.Close();
			glade.Autoconnect(this);
			thisDialog = ((Gtk.Dialog)(glade.GetWidget("OpenFileDialog")));
			thisDialog.Modal = true;
			thisDialog.TransientFor = parent;
			thisDialog.SetPosition (WindowPosition.Center);
		}


		public int Run()
		{
			thisDialog.ShowAll();

			int result = 0;
			for (; true; ) 
			{
				result = thisDialog.Run();
				if ((result != ((int)(Gtk.ResponseType.None))) && result != ((int)(Gtk.ResponseType.Apply)))
				{
					break;
				}
				Thread.Sleep(500);
			}
			thisDialog.Destroy();
			return result;
		}


		public int ShowDialog()
		{
			return Run();
		}
	}
}