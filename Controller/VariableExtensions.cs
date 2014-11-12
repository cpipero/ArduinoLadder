using System.Text.RegularExpressions;

namespace LadderLogic.Controller
{
	public static class VariableExtensions
	{
		public static string GetExactlyVariableOrConst(this string value)
		{
			var c = value.GetExactlyConst ();
			var v = value.GetExactlyVariable ();
			if (string.IsNullOrEmpty (c)) {
				return v;
			}

			return c;
		}


		public static string GetExactlyConst(this string value)
		{
			var m = Regex.Match (value, "^\\s*[0-9]+");
			if (m.Length > 0) {
				return m.Value.Trim();
			}

			return string.Empty;
		}


		public static string GetExactlyVariable(this string value)
		{
			var m = Regex.Match (value, "^\\s*[a-zA-Z_][a-zA-Z0-9]+");
			if (m.Length > 0) {
				return m.Value.Trim();
			}

			return string.Empty;
		}


		public static bool HasVaraiable(this string value)
		{
			return !string.IsNullOrEmpty (value.GetExactlyVariable ());
		}
	}
}

