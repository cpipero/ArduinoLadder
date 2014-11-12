namespace LadderLogic.CTool.FunctionRule
{
	using Surface;

	public class OnTimerRule : FunctionRule
	{
		public OnTimerRule()
		{
			ElementType = File.DrawingFile.ElementType.OnTimer;
		}

		public override FunctionType Resolve(Segment s, out string commect)
		{
			commect = string.Empty;
			return FunctionType.TimerOn;
		}
	}
}

