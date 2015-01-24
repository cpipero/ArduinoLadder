namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class CycleTimerRule : FunctionRule
	{
		public CycleTimerRule()
		{
			ElementType = File.DrawingFile.ElementType.CycleTimer;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.TimerCycle;
		}
	}
}

