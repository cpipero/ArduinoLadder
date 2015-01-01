namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class ResetCoilRule : FunctionRule
	{
		public ResetCoilRule()
		{
			ElementType = File.DrawingFile.ElementType.ResetCoil;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.Reset;
		}
	}
}

