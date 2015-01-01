namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class SetCoilRule : FunctionRule
	{
		public SetCoilRule()
		{
			ElementType = File.DrawingFile.ElementType.SetCoil;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.Set;
		}
	}
}

