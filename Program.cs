using System;
using System.Configuration;
using GLib;
using Gtk;
using LadderLogic.Updater;

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


			AppController.Instance.Initialize();
			if (args.Length > 0 && System.IO.File.Exists(args[0]))
			{

				AppController.Instance.FileName = args[0];
				var saved = ConfigManager.Read<PrimitivesSurface>(AppController.Instance.FileName);
				AppController.Instance.ResetSurface();
				AppController.Instance.ReloadSurface(saved);

			}

			var localFile = ConfigurationManager.AppSettings["LocalFile"];
			var remoteFile = ConfigurationManager.AppSettings["RemoteFile"];
			var remoteVersion = ConfigurationManager.AppSettings["RemoteVersion"];

			UpdateHelper.LocalFile = string.IsNullOrEmpty(localFile) ? UpdateHelper.LocalFile : localFile;
			UpdateHelper.RemoteFile = string.IsNullOrEmpty(remoteFile) ? UpdateHelper.RemoteFile : remoteFile;
			UpdateHelper.RemoteVersion = string.IsNullOrEmpty(remoteVersion) ? UpdateHelper.RemoteVersion : remoteVersion; 

			Application.Init ();
			_main = new AppWindow();
			Application.Run();
		}
	}
}
