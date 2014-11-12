using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LadderLogic.CTool.FunctionRule
{
	using Controller;
	using File.DrawingFile;
	using Surface;

	public static class FunctionRuleExtension
	{
		public static string GetFormat(this FunctionType rule)
		{
			var elements = AppController
				.Instance
				.GetPalette ()
				.Select (p => p.Get ().FirstOrDefault ())
				.Where (s => s != null)
				.Select (s => s.Primitives.FirstOrDefault (p => p.Container != null && p.Container.Functions != null))
				.Where(e => e != null)
				.ToList();

			var el = elements.FirstOrDefault (e => e.Container.Functions.Any (f => f.Function == rule));

			if (el == null) return string.Empty;
			var func = el.Container.Functions.FirstOrDefault (f => f.Function == rule);
			return func != null ? func.Format : string.Empty;
		}


		public static uint GetVariablesCount(this FunctionType rule)
		{
			var elements = AppController
				.Instance
				.GetPalette ()
				.Select (p => p.Get ().FirstOrDefault ())
				.Where (s => s != null)
				.Select (s => s.Primitives.FirstOrDefault (p => p.Container != null && p.Container.Functions != null))
				.Where(e => e != null)
				.ToList();

			var el = elements.FirstOrDefault (e => e.Container.Functions.Any (f => f.Function == rule));

			if (el != null) {
				var func = el.Container.Functions.FirstOrDefault (f => f.Function == rule);
				var regFormat = Regex.Matches (func.Format, "\\{\\d+\\}");
				var max = 0;
				for (var i = 0; i < regFormat.Count; i++) {
					var v = regFormat [i].Value.Replace ("{", string.Empty).Replace ("}", string.Empty);
					int res;
					if (int.TryParse (v, out res) && res > max) {
						max = res;
					}
				}
				return (uint)max + 1;
			}

			return 0;
		}


		public static List<FunctionType> GetFunctionsForElement(this ElementType type)
		{
			var seg = AppController
				.Instance
				.GetPalette ()
				.Select(p => p.Get().FirstOrDefault())
				.Where(s => s != null)
				.FirstOrDefault (s => s.Type == type);

			if (seg != null) {
				var pr = seg.Primitives.FirstOrDefault (p => p.Container != null);
				if (pr != null) {
					return pr.Container.Functions.Select (f => f.Function).ToList();
				}
			}

			return new List<FunctionType> ();
		}


		public static FunctionRule GetRuleForElement(this ElementType type)
		{
			return CController.Instance.Rules.FirstOrDefault (r => r.ElementType == type);
		}


		public static uint GetVariablesCount(this Segment seg)
		{
			string comment;
			return (seg.OverrideFunction ? seg.Function : seg.Type.GetRuleForElement ().Resolve(seg, out comment)).GetVariablesCount();
		}
	}
}

