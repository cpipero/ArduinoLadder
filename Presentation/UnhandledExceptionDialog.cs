using System;
using System.Threading;
using Gtk;
using System.Reflection;

namespace LadderLogic.Presentation
{
	using Controller;

	public class UnhandledExceptionDialog
	{
		Dialog _thisDialog;


		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		TextView textview1;
// ReSharper restore InconsistentNaming


		public UnhandledExceptionDialog(Window parent, Exception ex)
		{
			Initialize(parent, ex);
		}


		void Initialize(Window parent, Exception ex)
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(AppController.Instance.Config.UnhandledExceptionDialog);

			var glade = new Glade.XML(stream, AppController.Instance.Config.UnhandledExceptionDialogName, null);
			if (stream != null)
			{
				stream.Close();
			}

			//Glade.XML glade = Glade.XML.FromAssembly("UnhandledExceptionDialog.glade","UnhandledExceptionDialog", null);
			//stream.Close();
			glade.Autoconnect(this);
			_thisDialog = ((Dialog)(glade.GetWidget(AppController.Instance.Config.UnhandledExceptionDialogName)));

			//_thisDialog = ((Dialog)(glade.GetWidget(AppController.Instance.Config.UnhandledExceptionDialogName)));
			_thisDialog.Modal = true;
			_thisDialog.TransientFor = parent;
			_thisDialog.SetPosition (WindowPosition.Center);


			textview1.Buffer.Text = ex.ToString ();
		}


		public int Run()
		{
			_thisDialog.ShowAll();

			int result;
			for (;; ) 
			{
				result = _thisDialog.Run();
				if ((result != ((int)(ResponseType.None))))
				{
					break;
				}

				Thread.Sleep(500);
			}

			if (result == 1) {
				//send mail
				Application.Quit ();
			}

			if (result == 2) {
				Application.Quit ();
			}

			if (result == 3) {
				//continue
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

