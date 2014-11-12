namespace LadderLogic.Controller.State
{
	using Surface;

	public abstract class State
	{
		protected State (StateType type)
		{
			Type = type;
		}


		public virtual bool Handle (State previous, Segment prevSegment, Segment newSegment, bool left)
		{
			return true;
		}


		public virtual bool Leave()
		{
			return true;
		}


		public StateType Type { get; private set; }
	}
}

