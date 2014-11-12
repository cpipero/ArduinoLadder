namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class CoilRule : FunctionRule
	{
		public CoilRule()
		{
			ElementType = File.DrawingFile.ElementType.Coil;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.Out;
		}
	}
}

