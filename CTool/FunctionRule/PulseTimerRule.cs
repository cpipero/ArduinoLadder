namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class 
	PulseTimerRule : FunctionRule
	{
		public PulseTimerRule()
		{
			ElementType = File.DrawingFile.ElementType.PulseTimer;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.TimerPulse;
		}
	}
}

