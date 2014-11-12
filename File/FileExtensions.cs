using System.IO;
using System.Reflection;

namespace LadderLogic.File
{
	public static class FileExtensions
	{
		public static string GetAbsolutePath(this string file)
		{
			var location = Assembly.GetCallingAssembly().Location;
// ReSharper disable AssignNullToNotNullAttribute
			return Path.IsPathRooted(file) ? file : Path.Combine(Path.GetDirectoryName(location), file);
// ReSharper restore AssignNullToNotNullAttribute
		}
	}
}

