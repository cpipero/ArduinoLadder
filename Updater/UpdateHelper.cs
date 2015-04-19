using System;
using System.IO;
using Gtk;

namespace LadderLogic.Updater
{
	public static class UpdateHelper
	{
		private static Window _parent;

		public static string RemoteVersion = "http://localhost/ArduinoLadder/updateVersion.txt";
		public static string RemoteFile = "http://localhost/ArduinoLadder/ArduinoLadder.exe";
		public static string LocalFile = "ArduinoLadder.exe";

		public static void CompareVersions(Window parent)
		{
			_parent = parent;
			var downloadToPath = Path.GetTempPath();
			var localVersion = Versions.LocalVersion();
			var remoteVersion = Versions.RemoteVersion(RemoteVersion);
			if (string.IsNullOrWhiteSpace(remoteVersion)) //prevent to reload first version
			{
				return;
			}
			var c = 0;
			try
			{
				c = Version.Parse(localVersion).CompareTo(Version.Parse(remoteVersion));
			}
			catch (Exception ex)
			{
                
			}
			
			if (c < 0)
			{
				BeginDownload(RemoteFile, downloadToPath, remoteVersion, LocalFile);
			}
		}

		private static void BeginDownload(string remoteUrl, string downloadToPath, string version, string executeTarget)
		{
			var filePath = Versions.CreateTargetLocation(downloadToPath, version);

			filePath = Path.Combine(filePath, executeTarget);

			var remoteUri = new Uri(remoteUrl);
			var downloader = new System.Net.WebClient();

			downloader.DownloadFileCompleted += downloader_DownloadFileCompleted;

			downloader.DownloadFileAsync(remoteUri, filePath,
				new[] { version, downloadToPath, executeTarget });
		}


		private static void downloader_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				return;
			}
			var us = (string[])e.UserState;
			var currentVersion = us[0];
			var downloadToPath = us[1];
			var executeTarget = us[2];

			if (!downloadToPath.EndsWith("\\")) // Give a trailing \ if there isn't one
				downloadToPath += "\\";

			var exePath = downloadToPath + currentVersion + "\\" + executeTarget; // Download folder\version\ + executable
			var md = new MessageDialog(_parent,
				DialogFlags.DestroyWithParent, MessageType.Info,
				ButtonsType.YesNo, "New version available. Do you want to install?");
			if ((ResponseType) md.Run() != ResponseType.Yes) return;
			System.Diagnostics.Process.Start(exePath);
			Environment.Exit(0);
		}
	}
}
