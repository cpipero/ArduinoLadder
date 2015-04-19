using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Gtk;

namespace LadderLogic.Presentation
{
	public class UpdateDialog
	{
		Dialog _thisDialog;

		public UpdateDialog(Window parent)
		{
			Initialize(parent);
		}

		private void Initialize(Window parent)
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("LadderLogic.Presentation.UpdateDialog.glade");

			var glade = new Glade.XML(stream, "dialog1", null);
			if (stream != null)
			{
				stream.Close();
			}

			//Glade.XML glade = Glade.XML.FromAssembly("UnhandledExceptionDialog.glade","UnhandledExceptionDialog", null);
			//stream.Close();
			glade.Autoconnect(this);
			_thisDialog = ((Dialog)(glade.GetWidget("dialog1")));
			_thisDialog.SetPosition(WindowPosition.Center);
		}

		public int Run()
		{
			_thisDialog.ShowAll();
			return 0;
		}

		public int ShowDialog()
		{
			return Run();
		}
	}
}
