using System;
using System.Configuration;
using GLib;
using Gtk;

namespace LadderLogic
{
    using Controller;
    using Presentation;
    using Reader;
    using Surface;

    class MainClass
	{
		public static AppWindow _main;


		public static void Main (string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += 
				(sender, e) => new UnhandledExceptionDialog(_main, e.ExceptionObject as Exception).ShowDialog();


			ExceptionManager.UnhandledException += 
				e => new UnhandledExceptionDialog(_main, e.ExceptionObject as Exception).ShowDialog();


			var isElementsView = ConfigurationManager
				.AppSettings ["ElementsView"]
				.ToLowerInvariant () == "true";

			if (isElementsView) {
				DrawingAreaController.Instance.Initialize ();
			} else {
				AppController.Instance.Initialize ();
				if (args.Length > 0 && System.IO.File.Exists (args [0])) {
				
						AppController.Instance.FileName = args [0];
						var saved = ConfigManager.Read<PrimitivesSurface> (AppController.Instance.FileName);
						AppController.Instance.ResetSurface ();
						AppController.Instance.ReloadSurface (saved);

				}
			}

			Application.Init ();
			if (isElementsView) {
				//_main = new DrawingAreaWindow ();
			} else {
				_main = new AppWindow ();
			}

			Application.Run ();
		}
	}
}
