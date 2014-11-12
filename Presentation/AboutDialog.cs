using Gtk;
using System.Threading;
using System.Reflection;

namespace LadderLogic.Presentation
{	
	using Controller;

	public class AboutDialog
	{
		Dialog _thisDialog;
	
			
		public AboutDialog(Window parent)
		{
		    Initialize(parent);
		}


		void Initialize(Window parent)
		{
			var gladeRes = AppController.Instance.Config.AboutDialog;
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(gladeRes);

			var glade = new Glade.XML(stream, AppController.Instance.Config.AboutDialogName, null);
			if (stream != null)
			{
				stream.Close();
			}

			glade.Autoconnect(this);
			_thisDialog = ((Dialog)(glade.GetWidget(AppController.Instance.Config.AboutDialogName)));
			_thisDialog.Modal = true;
			_thisDialog.TransientFor = parent;
			_thisDialog.SetPosition (WindowPosition.Center);
		}

		
		public int Run()
		{
			_thisDialog.ShowAll();
			
			int result;
			for (;; ) 
			{
				result = _thisDialog.Run();
				if ((result != ((int)(ResponseType.None))) && result != ((int)(ResponseType.Apply)))
				{
					break;
				}
				Thread.Sleep(500);
			}
			_thisDialog.Destroy();
			return result;
		}


		public int ShowDialog()
		{
			return Run();
		}
	}
}