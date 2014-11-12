namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class OffTimerRule : FunctionRule
	{
		public OffTimerRule()
		{
			ElementType = File.DrawingFile.ElementType.OffTimer;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.TimerOff;
		}
	}
}

