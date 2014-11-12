using System.Reflection;
using System.Threading;
using Gtk;

namespace LadderLogic.Presentation
{
	using CTool;
	using CTool.Arduino;

	public class EnvironmentVariablesDialog
	{
		Dialog _thisDialog;

		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etPath;
		// ReSharper restore InconsistentNaming

		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etBoard;
		// ReSharper restore InconsistentNaming

		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etPort;
		// ReSharper restore InconsistentNaming

		public EnvironmentVariablesDialog(Window parent)
		{
			Initialize(parent);
		}


		void Initialize(Window parent)
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("LadderLogic.Presentation.EnvironmentVariablesDialog.glade");

			var glade = new Glade.XML(stream, "UnhandledExceptionDialog", null);
			if (stream != null)
			{
				stream.Close();
			}

			//Glade.XML glade = Glade.XML.FromAssembly("UnhandledExceptionDialog.glade","UnhandledExceptionDialog", null);
			//stream.Close();
			glade.Autoconnect(this);
			_thisDialog = ((Dialog)(glade.GetWidget("UnhandledExceptionDialog")));

			//_thisDialog = ((Dialog)(glade.GetWidget(AppController.Instance.Config.UnhandledExceptionDialogName)));
			//_thisDialog.Modal = true;
			//_thisDialog.TransientFor = parent;
			_thisDialog.SetPosition (WindowPosition.Center);

			var env = CController.Instance.GetEnvironmentVariables ();

			if (env != null) {
				etBoard.Text = env.ArduinoBoard;
				etPort.Text = env.ArduinoPort;
				etPath.Text = env.ArduinoPath;
			}
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
				var env = new EnvironmentVariables {
					ArduinoPath = etPath.Text,
					ArduinoBoard = etBoard.Text,
					ArduinoPort = etPort.Text
				};

				CController.Instance.SetEnvironmentVariables (env);
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

