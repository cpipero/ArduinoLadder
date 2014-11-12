using System;
using System.Reflection;
using Gtk;

namespace LadderLogic.Presentation
{
	using Controller;
	using CTool;

	public class SourceDialog
	{
		Dialog _thisDialog;


		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		TextView textview1;
// ReSharper restore InconsistentNaming


		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		Button btnContinue;
// ReSharper restore InconsistentNaming


		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		Button btnSave;
// ReSharper restore InconsistentNaming


		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		Button btnUpload;
// ReSharper restore InconsistentNaming


		[Glade.Widget]
// ReSharper disable once UnassignedField.Compiler
// ReSharper disable InconsistentNaming
		TextView tvOutput;
// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  miSave;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  miSaveAs;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  miQuit;
		// ReSharper restore InconsistentNaming



		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  miAbout;
		// ReSharper restore InconsistentNaming


		string _fileName;


		public SourceDialog(Window parent, string text)
		{
			Initialize(parent, text);
		}


		void Initialize(Window parent, String text)
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(AppController.Instance.Config.SourceDialog);

			var glade = new Glade.XML(stream, "UnhandledExceptionDialog", null);
			if (stream != null)
			{
				stream.Close();
			}

			//Glade.XML glade = Glade.XML.FromAssembly("UnhandledExceptionDialog.glade","UnhandledExceptionDialog", null);
			//stream.Close();
			glade.Autoconnect(this);
			_thisDialog = ((Dialog)(glade.GetWidget("UnhandledExceptionDialog")));
			_thisDialog.Modal = true;
			_thisDialog.TransientFor = parent;
			_thisDialog.SetPosition (WindowPosition.Center);


			textview1.Buffer.Text = text;
			btnContinue.Clicked += (sender, e) => _thisDialog.HideAll();

			btnUpload.Clicked += OnUpload;

			miSave.Activated += OnSave;
			miSaveAs.Activated += OnSaveAs;
			miQuit.Activated += (object sender, EventArgs e) => _thisDialog.HideAll();
			miAbout.Activated += (object sender, EventArgs e) => new AboutDialog (_thisDialog).ShowDialog();
		}

		void OnUpload (object sender, EventArgs e)
		{
			tvOutput.Buffer.Text = CController.Instance.Upload(textview1.Buffer.Text, _fileName);		
		}


		public int Run()
		{
			_thisDialog.ShowAll();
			return 0;
		}

		void OnSave (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (_fileName) || !System.IO.File.Exists (_fileName)) {
				OnSaveAs (sender, e);
			} else {
				System.IO.File.WriteAllText (_fileName, textview1.Buffer.Text);
			}
		}

		void OnSaveAs (object sender, EventArgs e)
		{
			var fc=
				new FileChooserDialog("Choose the file to save",
					_thisDialog,
					FileChooserAction.Save,
					"Cancel",ResponseType.Cancel,
					"Save",ResponseType.Accept);

			fc.SetPosition (WindowPosition.Center);
			if (fc.Run() == (int)ResponseType.Accept) 
			{
				_fileName = fc.Filename;
				System.IO.File.WriteAllText (_fileName, textview1.Buffer.Text);
			}

			//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
			fc.Destroy();
		}


		public int ShowDialog()
		{
			return Run();
		}
	}
}

