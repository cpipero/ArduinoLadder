namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class NotCoilRule : FunctionRule
	{
		public NotCoilRule()
		{
			ElementType = File.DrawingFile.ElementType.NotCoil;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.OutNot;
		}
	}
}

