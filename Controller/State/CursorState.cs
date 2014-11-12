namespace LadderLogic.Controller.State
{
	using Surface;

	public class CursorState : State
	{
		public CursorState () : base(StateType.CursorState)
		{
		}

		#region implemented abstract members of State

		public override bool Handle (State previous, Segment prevSegment, Segment newSegment, bool left)
		{
			base.Handle (previous, prevSegment, newSegment, left);
			return true;
		}

		#endregion
	}
}

