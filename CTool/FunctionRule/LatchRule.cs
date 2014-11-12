namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class LatchRule : FunctionRule
	{
		public LatchRule()
		{
			ElementType = File.DrawingFile.ElementType.Latch;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.Latch;
		}
	}
}

