using System.Linq;

namespace LadderLogic.CTool.FunctionRule
{
	using Controller;
	using File.Config;
	using File.DrawingFile;
	using Surface;

	public class ContactRule : FunctionRule
	{
		readonly LocalConfig _conf = AppController.Instance.Config;

		public FunctionType First { get; set; }


		public FunctionType Next { get; set; }


		public FunctionType Sibling { get; set; }


		public override FunctionType Resolve(Segment s, out string commect)
		{
			//By coil we can found in.
			if (!s.Connectors.Any (c => c.Marker == _conf.RightConnector && c.ConnectedTo.Any()) ||
				!s.Connectors.Any (c => c.Marker == _conf.LeftConnector && c.ConnectedTo.Any())) {
				commect = "Contact connectors have not mapped correctly.";
				return FunctionType.Error;
			}

			var leftCon = s.Connectors.First (c => c.Marker == _conf.LeftConnector);
			var rightCon = s.Connectors.First (c => c.Marker == _conf.RightConnector);

			if (leftCon.ConnectedTo.Contains (_conf.LeftPower)) {
				commect = string.Empty;
				return First;
			}

			var leftSegments = leftCon
				.ConnectedTo
				.ToList()
				.Select(id => AppController.Instance.Surface.Segments.FirstOrDefault(s1 => s1.Identifier.ToString() == id))
				.Where(s1 => s1!= null);

			var firstSegments = leftSegments.Where(s1 => s1.Type == ElementType.None && s1.Connectors.Any(c => c.Marker == _conf.LeftPower && c.ConnectedTo.Contains(s.Identifier.ToString())));
			if (firstSegments.Any ()) {
				commect = string.Empty;
				return First;
			}

			var nextSegments = leftSegments.Where(s1 => s1.Type != ElementType.None && s1.Position.Y < s.Position.Y);


			if(nextSegments.Any())
			{
				commect = string.Empty;
				return Sibling;
			}

			commect = string.Empty;
			return Next;
		}
	}
}

