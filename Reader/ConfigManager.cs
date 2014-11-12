using System.IO;
using System.Xml.Serialization;

namespace LadderLogic.Reader
{
	using File.Config;

	public class ConfigManager
	{
		public static T Read<T> (string file)
			where T : class
		{
			var ser = new XmlSerializer (typeof(T));
			using (var f = new FileStream (file, FileMode.Open)) {
				return ser.Deserialize (f) as T;
			}
		}


		public static bool Write<T> (T config, string file)
			where T : class
		{
			var ser = new XmlSerializer (typeof(T));
			using (var w = new StreamWriter (file)) {
				ser.Serialize (w, config);
			}
			return true;
		}
	}
}

