using System.Linq;
using System.Text.RegularExpressions;

namespace LadderLogic.CTool.FunctionRule
{
	using Surface;
	using Controller;
	using File.DrawingFile;

	public abstract class FunctionRule
	{
		public abstract FunctionType Resolve(Segment s, out string commect);

		public string GetFormatted(Segment s)
		{
			string comment;
			var rule = !s.OverrideFunction ? Resolve (s, out comment) : s.Function;
			var format = rule.GetFormat ();

			const string pattern = @"{(.*?)}";
			var matches = Regex.Matches(format, pattern);
			var uniqueMatchCount = matches.OfType<Match>().Select(m => m.Value).Distinct().Count();
			if (uniqueMatchCount <= s.Variables.Count ()) {
				return string.Format (format, s.Variables.Select (v => v.Value.GetExactlyVariableOrConst()).ToArray ());
			}
			return string.Empty;
		}


		public ElementType ElementType { get; protected set; }
	}
}

