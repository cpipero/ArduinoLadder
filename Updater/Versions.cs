using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace LadderLogic.Updater
{
	public static class Versions
	{
		public static string RemoteVersion(string url)
		{
			var rv = "";

			try
			{
				var req = (System.Net.HttpWebRequest)
				System.Net.WebRequest.Create(url);
				var response = (System.Net.HttpWebResponse)req.GetResponse();
				var receiveStream = response.GetResponseStream();
				if (receiveStream != null)
				{
					var readStream = new StreamReader(receiveStream, Encoding.UTF8);
					var s = readStream.ReadToEnd();
					response.Close();
					if (ValidateFile(s))
					{
						rv = s;
					}
				}
			}
			catch (Exception)
			{
				rv = null;
			}
			return rv;
		}

		public static string LocalVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public static bool ValidateFile(string contents)
		{
			if (string.IsNullOrEmpty(contents)) return false;
			const string pattern = @"^\d*\.\d*\.\d*\.\d*$";
			var re = new System.Text.RegularExpressions.Regex(pattern);
			var val = re.IsMatch(contents);
			return val;
		}

		public static string CreateLocalVersionFile(string folderPath, string fileName, string version)
		{
			if (!new DirectoryInfo(folderPath).Exists)
			{
				Directory.CreateDirectory(folderPath);
			}

			var path = folderPath + "\\" + fileName;

			if (new FileInfo(path).Exists)
			{
				new FileInfo(path).Delete();
			}

			if (!new FileInfo(path).Exists)
			{
				System.IO.File.WriteAllText(path, version);
			}
			return path;
		}

		public static string CreateTargetLocation(string downloadToPath, string version)
		{
			if (!downloadToPath.EndsWith("\\")) // Give a trailing \ if there isn't one
				downloadToPath += "\\";

			var filePath = Path.Combine(downloadToPath, version);

			var newFolder = new DirectoryInfo(filePath);
			newFolder.Create();
				
			return filePath;
		}
	}
}
